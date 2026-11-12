import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../../../app/router.dart';

import '../../../core/l10n/l10n.dart';
import '../../../core/settings/app_settings.dart';
import '../../../l10n/app_localizations.dart';
import '../../locations/presentation/selected_city_controller.dart';

/// City, language, and appearance can be changed before signing in
/// (mockup documents/mockups/1A.2-02-city-picker).
class AccountScreen extends ConsumerWidget {
  const AccountScreen({super.key});

  /// Language names are shown in their own language so anyone can find theirs.
  static const _languageNames = {'en': 'English', 'es': 'Español'};

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final l10n = context.l10n;
    final settings = ref.watch(appSettingsProvider);
    final controller = ref.read(appSettingsProvider.notifier);

    return Scaffold(
      appBar: AppBar(title: Text(l10n.accountTitle)),
      body: ListView(
        padding: const EdgeInsetsDirectional.all(16),
        children: [
          Text(l10n.cityLabel, style: Theme.of(context).textTheme.titleMedium),
          const SizedBox(height: 6),
          const _CityCard(),
          const SizedBox(height: 16),
          Text(
            l10n.languageLabel,
            style: Theme.of(context).textTheme.titleMedium,
          ),
          RadioGroup<String?>(
            groupValue: settings.languageCode,
            onChanged: controller.setLanguage,
            child: Column(
              children: [
                RadioListTile<String?>(
                  value: null,
                  title: Text(l10n.languageSystem),
                ),
                for (final locale in AppLocalizations.supportedLocales)
                  RadioListTile<String?>(
                    value: locale.languageCode,
                    title: Text(
                      _languageNames[locale.languageCode] ??
                          locale.languageCode,
                    ),
                  ),
              ],
            ),
          ),
          const SizedBox(height: 16),
          Text(l10n.themeLabel, style: Theme.of(context).textTheme.titleMedium),
          RadioGroup<ThemeMode>(
            groupValue: settings.themeMode,
            onChanged: (mode) =>
                controller.setThemeMode(mode ?? ThemeMode.system),
            child: Column(
              children: [
                RadioListTile(
                  value: ThemeMode.system,
                  title: Text(l10n.themeSystem),
                ),
                RadioListTile(
                  value: ThemeMode.light,
                  title: Text(l10n.themeLight),
                ),
                RadioListTile(
                  value: ThemeMode.dark,
                  title: Text(l10n.themeDark),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }
}

class _CityCard extends ConsumerWidget {
  const _CityCard();

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final l10n = context.l10n;
    final city = ref.watch(selectedCityProvider);
    return Card(
      margin: EdgeInsets.zero,
      child: ListTile(
        title: Text(city?.displayName ?? l10n.cityPickerTitle),
        subtitle: Text(
          city == null
              ? l10n.cityNotSet
              : l10n.cityLocation(
                  city.departmentName,
                  l10n.countryName(city.countryCode),
                ),
        ),
        trailing: TextButton(
          onPressed: () => context.go(AppRoutes.accountCity),
          child: Text(city == null ? l10n.chooseAction : l10n.changeAction),
        ),
        onTap: () => context.go(AppRoutes.accountCity),
      ),
    );
  }
}
