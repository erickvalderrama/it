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
  String get navExplore => 'Explorar';

  @override
  String get navPublish => 'Publicar';

  @override
  String get navAccount => 'Cuenta';

  @override
  String get exploreTitle => 'Mascotas en adopción';

  @override
  String get exploreEmpty => 'Todavía no hay mascotas en adopción aquí.';

  @override
  String get publishTitle => 'Publicar una adopción';

  @override
  String get publishIntro =>
      'Crea una publicación para que una familia encuentre a esta mascota.';

  @override
  String get accountTitle => 'Cuenta';

  @override
  String get languageLabel => 'Idioma';

  @override
  String get languageSystem => 'Idioma del dispositivo';

  @override
  String get themeLabel => 'Apariencia';

  @override
  String get themeSystem => 'Según el dispositivo';

  @override
  String get themeLight => 'Claro';

  @override
  String get themeDark => 'Oscuro';

  @override
  String get countriesEmpty => 'Todavía no hay países disponibles.';

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

  @override
  String apiErrorMessage(String code) {
    String _temp0 = intl.Intl.selectLogic(code, {
      'locations_invalid_country_code': 'Elige un país válido.',
      'locations_country_not_enabled':
          'Petyei todavía no está disponible en este país.',
      'locations_locality_not_available': 'Elige una ciudad de la lista.',
      'common_not_found': 'No encontramos lo que buscabas.',
      'other':
          'No pudimos cargar esta información. Revisa tu conexión e inténtalo de nuevo.',
    });
    return '$_temp0';
  }

  @override
  String get cityLabel => 'Tu ciudad';

  @override
  String get cityNotSet => 'Elige tu ciudad para ver mascotas cerca de ti.';

  @override
  String get changeAction => 'Cambiar';

  @override
  String get chooseAction => 'Elegir';

  @override
  String get cityPickerTitle => 'Elige tu ciudad';

  @override
  String get countryLabel => 'País';

  @override
  String get citySearchHint => 'Buscar ciudad o municipio';

  @override
  String citySearchEmpty(String query) {
    return 'No encontramos «$query».';
  }

  @override
  String get citySearchEmptyHint =>
      'Revisa cómo se escribe o busca por el nombre del municipio.';

  @override
  String get clearSearchAction => 'Borrar búsqueda';

  @override
  String cityLocation(String department, String country) {
    return '$department, $country';
  }
}
