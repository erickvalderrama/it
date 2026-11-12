import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import '../../../app/app_shell.dart';
import '../../../core/l10n/l10n.dart';
import 'city_picker.dart';

/// Full screen on compact widths and a dialog over the current screen on wide
/// ones, both with their own URL so browser back closes them.
class CityPickerPage extends Page<void> {
  const CityPickerPage({super.key});

  @override
  Route<void> createRoute(BuildContext context) {
    final wide = MediaQuery.sizeOf(context).width >= AppShell.wideBreakpoint;
    if (wide) {
      return DialogRoute<void>(
        context: context,
        settings: this,
        builder: (_) => const _CityPickerDialog(),
      );
    }
    return MaterialPageRoute<void>(
      settings: this,
      builder: (_) => const _CityPickerScreen(),
    );
  }
}

class _CityPickerScreen extends StatelessWidget {
  const _CityPickerScreen();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text(context.l10n.cityPickerTitle)),
      body: Padding(
        padding: const EdgeInsetsDirectional.fromSTEB(12, 8, 12, 0),
        child: CityPicker(onSelected: () => context.pop()),
      ),
    );
  }
}

class _CityPickerDialog extends StatelessWidget {
  const _CityPickerDialog();

  @override
  Widget build(BuildContext context) {
    final l10n = context.l10n;
    return Dialog(
      backgroundColor: Theme.of(context).scaffoldBackgroundColor,
      child: ConstrainedBox(
        constraints: const BoxConstraints(maxWidth: 560, maxHeight: 600),
        child: Padding(
          padding: const EdgeInsetsDirectional.fromSTEB(22, 12, 22, 20),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              Row(
                children: [
                  Expanded(
                    child: Text(
                      l10n.cityPickerTitle,
                      style: Theme.of(context).textTheme.titleLarge,
                    ),
                  ),
                  IconButton(
                    tooltip: MaterialLocalizations.of(
                      context,
                    ).closeButtonTooltip,
                    icon: const Icon(Icons.close),
                    onPressed: () => context.pop(),
                  ),
                ],
              ),
              const SizedBox(height: 8),
              Expanded(child: CityPicker(onSelected: () => context.pop())),
            ],
          ),
        ),
      ),
    );
  }
}
