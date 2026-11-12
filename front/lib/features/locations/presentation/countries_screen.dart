import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../core/l10n/l10n.dart';
import 'countries_controller.dart';

class CountriesScreen extends ConsumerWidget {
  const CountriesScreen({super.key});

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final countries = ref.watch(countriesControllerProvider);
    final l10n = context.l10n;

    return Scaffold(
      appBar: AppBar(title: Text(l10n.countriesTitle)),
      body: countries.when(
        loading: () => const Center(child: CircularProgressIndicator()),
        error: (_, _) => Center(
          child: Padding(
            padding: const EdgeInsetsDirectional.all(24),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Text(l10n.loadErrorMessage, textAlign: TextAlign.center),
                const SizedBox(height: 16),
                FilledButton(
                  onPressed: () =>
                      ref.read(countriesControllerProvider.notifier).retry(),
                  child: Text(l10n.retryAction),
                ),
              ],
            ),
          ),
        ),
        data: (items) => items.isEmpty
            ? Center(child: Text(l10n.countriesEmpty))
            : ListView(
                children: [
                  for (final country in items)
                    ListTile(title: Text(l10n.countryName(country.code))),
                ],
              ),
      ),
    );
  }
}
