import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../core/l10n/l10n.dart';
import '../../../core/network/error_messages.dart';
import '../../../core/widgets/status_message.dart';
import '../domain/locality.dart';
import 'countries_controller.dart';
import 'selected_city_controller.dart';

/// Country and city picker (mockup documents/mockups/1A.2-02-city-picker).
/// Colombia is preselected; tapping a city stores it and closes the picker.
class CityPicker extends ConsumerStatefulWidget {
  const CityPicker({super.key, required this.onSelected});

  /// Called after the city is stored, to close the page or dialog.
  final VoidCallback onSelected;

  static const defaultCountryCode = 'CO';

  @override
  ConsumerState<CityPicker> createState() => _CityPickerState();
}

class _CityPickerState extends ConsumerState<CityPicker> {
  final _search = TextEditingController();
  late String _countryCode =
      ref.read(selectedCityProvider)?.countryCode ??
      CityPicker.defaultCountryCode;

  @override
  void dispose() {
    _search.dispose();
    super.dispose();
  }

  Future<void> _choose(
    DepartmentLocalities department,
    Locality locality,
  ) async {
    await ref
        .read(selectedCityProvider.notifier)
        .select(
          SelectedCity(
            countryCode: _countryCode,
            localityCode: locality.code,
            displayName: locality.displayName,
            departmentName: department.displayName,
          ),
        );
    widget.onSelected();
  }

  @override
  Widget build(BuildContext context) {
    final l10n = context.l10n;
    final countries = ref.watch(countriesProvider).value ?? const [];
    final localities = ref.watch(localitiesProvider(_countryCode));

    return Column(
      crossAxisAlignment: CrossAxisAlignment.stretch,
      children: [
        LayoutBuilder(
          builder: (context, constraints) {
            final countryField = SizedBox(
              width: 200,
              child: DropdownButtonFormField<String>(
                initialValue: _countryCode,
                decoration: InputDecoration(labelText: l10n.countryLabel),
                items: [
                  for (final code in {
                    _countryCode,
                    ...countries.map((country) => country.code),
                  })
                    DropdownMenuItem(
                      value: code,
                      child: Text(l10n.countryName(code)),
                    ),
                ],
                onChanged: (code) =>
                    setState(() => _countryCode = code ?? _countryCode),
              ),
            );
            final searchField = TextField(
              controller: _search,
              onChanged: (_) => setState(() {}),
              decoration: InputDecoration(
                prefixIcon: const Icon(Icons.search),
                hintText: l10n.citySearchHint,
                suffixIcon: _search.text.isEmpty
                    ? null
                    : IconButton(
                        tooltip: l10n.clearSearchAction,
                        icon: const Icon(Icons.close),
                        onPressed: () => setState(_search.clear),
                      ),
              ),
            );
            // Side by side when there is room (web dialog); stacked on phones.
            if (constraints.maxWidth >= 480) {
              return Row(
                children: [
                  countryField,
                  const SizedBox(width: 12),
                  Expanded(child: searchField),
                ],
              );
            }
            return Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [countryField, const SizedBox(height: 8), searchField],
            );
          },
        ),
        const SizedBox(height: 8),
        Expanded(
          child: localities.when(
            loading: () => const Center(child: CircularProgressIndicator()),
            error: (error, _) => StatusMessage(
              message: errorMessage(l10n, error),
              actionLabel: l10n.retryAction,
              onAction: () => ref.invalidate(localitiesProvider(_countryCode)),
            ),
            data: (groups) => _Results(
              groups: groups,
              query: _search.text.trim(),
              selectedCode: ref.watch(selectedCityProvider)?.localityCode,
              onChoose: _choose,
              onClearSearch: () => setState(_search.clear),
            ),
          ),
        ),
      ],
    );
  }
}

class _Results extends StatelessWidget {
  const _Results({
    required this.groups,
    required this.query,
    required this.selectedCode,
    required this.onChoose,
    required this.onClearSearch,
  });

  final List<DepartmentLocalities> groups;
  final String query;
  final String? selectedCode;
  final void Function(DepartmentLocalities, Locality) onChoose;
  final VoidCallback onClearSearch;

  @override
  Widget build(BuildContext context) {
    final l10n = context.l10n;
    final folded = foldForSearch(query);
    final rows = <Widget>[];
    for (final group in groups) {
      final matches = group.localities
          .where((l) => foldForSearch(l.displayName).contains(folded))
          .toList();
      if (matches.isEmpty) {
        continue;
      }
      rows.add(_DepartmentHeader(group.displayName));
      for (final locality in matches) {
        rows.add(
          _LocalityTile(
            locality: locality,
            query: folded,
            selected: locality.code == selectedCode,
            onTap: () => onChoose(group, locality),
          ),
        );
      }
    }

    if (rows.isEmpty) {
      return StatusMessage(
        message: query.isEmpty
            ? l10n.countriesEmpty
            : '${l10n.citySearchEmpty(query)}\n${l10n.citySearchEmptyHint}',
        actionLabel: query.isEmpty ? null : l10n.clearSearchAction,
        onAction: query.isEmpty ? null : onClearSearch,
      );
    }
    return ListView(children: rows);
  }
}

class _DepartmentHeader extends StatelessWidget {
  const _DepartmentHeader(this.name);

  final String name;

  @override
  Widget build(BuildContext context) {
    final theme = Theme.of(context);
    return Padding(
      padding: const EdgeInsetsDirectional.fromSTEB(4, 12, 4, 4),
      child: Semantics(
        header: true,
        child: Text(
          name.toUpperCase(),
          style: theme.textTheme.labelLarge?.copyWith(
            color: theme.colorScheme.primary,
            fontWeight: FontWeight.w700,
          ),
        ),
      ),
    );
  }
}

class _LocalityTile extends StatelessWidget {
  const _LocalityTile({
    required this.locality,
    required this.query,
    required this.selected,
    required this.onTap,
  });

  final Locality locality;
  final String query;
  final bool selected;
  final VoidCallback onTap;

  @override
  Widget build(BuildContext context) {
    final scheme = Theme.of(context).colorScheme;
    return Material(
      color: selected ? scheme.primaryContainer : scheme.surface,
      child: ListTile(
        selected: selected,
        selectedColor: scheme.onPrimaryContainer,
        title: _highlight(context, locality.displayName),
        trailing: selected ? Icon(Icons.check, color: scheme.primary) : null,
        onTap: onTap,
      ),
    );
  }

  /// Bolds the matched part. Folding keeps character positions unchanged.
  Widget _highlight(BuildContext context, String name) {
    final start = query.isEmpty ? -1 : foldForSearch(name).indexOf(query);
    if (start < 0) {
      return Text(name);
    }
    final strong = TextStyle(
      fontWeight: FontWeight.w700,
      color: Theme.of(context).colorScheme.primary,
    );
    return Text.rich(
      TextSpan(
        children: [
          TextSpan(text: name.substring(0, start)),
          TextSpan(
            text: name.substring(start, start + query.length),
            style: strong,
          ),
          TextSpan(text: name.substring(start + query.length)),
        ],
      ),
    );
  }
}
