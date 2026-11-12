// ignore: unused_import
import 'package:intl/intl.dart' as intl;
import 'app_localizations.dart';

// ignore_for_file: type=lint

/// The translations for Spanish Castilian (`es`).
class AppLocalizationsEs extends AppLocalizations {
  AppLocalizationsEs([String locale = 'es']) : super(locale);

  @override
  String get appTitle => 'Petyei';

  @override
  String get countriesTitle => 'Países disponibles';

  @override
  String get countriesEmpty => 'Todavía no hay países disponibles.';

  @override
  String get loadErrorMessage =>
      'No pudimos cargar esta información. Revisa tu conexión e inténtalo de nuevo.';

  @override
  String get retryAction => 'Reintentar';

  @override
  String countryName(String code) {
    String _temp0 = intl.Intl.selectLogic(code, {
      'CO': 'Colombia',
      'other': '$code',
    });
    return '$_temp0';
  }
}
