import 'package:flutter/material.dart';

import '../features/locations/presentation/countries_screen.dart';
import '../l10n/app_localizations.dart';
import 'theme/app_theme.dart';

class PetyeiApp extends StatelessWidget {
  const PetyeiApp({super.key, this.locale, this.themeMode = ThemeMode.system});

  /// Null follows the device language, falling back to English.
  final Locale? locale;
  final ThemeMode themeMode;

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      onGenerateTitle: (context) => AppLocalizations.of(context).appTitle,
      theme: AppTheme.light,
      darkTheme: AppTheme.dark,
      themeMode: themeMode,
      locale: locale,
      localizationsDelegates: AppLocalizations.localizationsDelegates,
      supportedLocales: AppLocalizations.supportedLocales,
      home: const CountriesScreen(),
    );
  }
}
