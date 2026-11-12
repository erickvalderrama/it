// ignore: unused_import
import 'package:intl/intl.dart' as intl;
import 'app_localizations.dart';

// ignore_for_file: type=lint

/// The translations for English (`en`).
class AppLocalizationsEn extends AppLocalizations {
  AppLocalizationsEn([String locale = 'en']) : super(locale);

  @override
  String get appTitle => 'Petyei';

  @override
  String get countriesTitle => 'Available countries';

  @override
  String get countriesEmpty => 'No countries are available yet.';

  @override
  String get loadErrorMessage =>
      'We couldn\'t load this information. Check your connection and try again.';

  @override
  String get retryAction => 'Try again';

  @override
  String countryName(String code) {
    String _temp0 = intl.Intl.selectLogic(code, {
      'CO': 'Colombia',
      'other': '$code',
    });
    return '$_temp0';
  }
}
