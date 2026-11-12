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
  String get navExplore => 'Explore';

  @override
  String get navPublish => 'Publish';

  @override
  String get navAccount => 'Account';

  @override
  String get exploreTitle => 'Pets for adoption';

  @override
  String get exploreEmpty => 'There are no pets for adoption here yet.';

  @override
  String get publishTitle => 'Publish an adoption';

  @override
  String get publishIntro => 'Create a listing so a family can find this pet.';

  @override
  String get accountTitle => 'Account';

  @override
  String get languageLabel => 'Language';

  @override
  String get languageSystem => 'Device language';

  @override
  String get themeLabel => 'Appearance';

  @override
  String get themeSystem => 'Device setting';

  @override
  String get themeLight => 'Light';

  @override
  String get themeDark => 'Dark';

  @override
  String get countriesEmpty => 'No countries are available yet.';

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

  @override
  String apiErrorMessage(String code) {
    String _temp0 = intl.Intl.selectLogic(code, {
      'locations_invalid_country_code': 'Choose a valid country.',
      'locations_country_not_enabled':
          'Petyei is not available in this country yet.',
      'locations_locality_not_available': 'Choose a city from the list.',
      'common_not_found': 'We couldn\'t find what you were looking for.',
      'other':
          'We couldn\'t load this information. Check your connection and try again.',
    });
    return '$_temp0';
  }

  @override
  String get cityLabel => 'Your city';

  @override
  String get cityNotSet => 'Choose your city to see pets near you.';

  @override
  String get changeAction => 'Change';

  @override
  String get chooseAction => 'Choose';

  @override
  String get cityPickerTitle => 'Choose your city';

  @override
  String get countryLabel => 'Country';

  @override
  String get citySearchHint => 'Search city or municipality';

  @override
  String citySearchEmpty(String query) {
    return 'We couldn\'t find “$query”.';
  }

  @override
  String get citySearchEmptyHint =>
      'Check the spelling or search by municipality name.';

  @override
  String get clearSearchAction => 'Clear search';

  @override
  String cityLocation(String department, String country) {
    return '$department, $country';
  }
}
