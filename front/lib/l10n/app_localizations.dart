import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:intl/intl.dart' as intl;

import 'app_localizations_en.dart';
import 'app_localizations_es.dart';

// ignore_for_file: type=lint

/// Callers can lookup localized strings with an instance of AppLocalizations
/// returned by `AppLocalizations.of(context)`.
///
/// Applications need to include `AppLocalizations.delegate()` in their app's
/// `localizationDelegates` list, and the locales they support in the app's
/// `supportedLocales` list. For example:
///
/// ```dart
/// import 'l10n/app_localizations.dart';
///
/// return MaterialApp(
///   localizationsDelegates: AppLocalizations.localizationsDelegates,
///   supportedLocales: AppLocalizations.supportedLocales,
///   home: MyApplicationHome(),
/// );
/// ```
///
/// ## Update pubspec.yaml
///
/// Please make sure to update your pubspec.yaml to include the following
/// packages:
///
/// ```yaml
/// dependencies:
///   # Internationalization support.
///   flutter_localizations:
///     sdk: flutter
///   intl: any # Use the pinned version from flutter_localizations
///
///   # Rest of dependencies
/// ```
///
/// ## iOS Applications
///
/// iOS applications define key application metadata, including supported
/// locales, in an Info.plist file that is built into the application bundle.
/// To configure the locales supported by your app, you’ll need to edit this
/// file.
///
/// First, open your project’s ios/Runner.xcworkspace Xcode workspace file.
/// Then, in the Project Navigator, open the Info.plist file under the Runner
/// project’s Runner folder.
///
/// Next, select the Information Property List item, select Add Item from the
/// Editor menu, then select Localizations from the pop-up menu.
///
/// Select and expand the newly-created Localizations item then, for each
/// locale your application supports, add a new item and select the locale
/// you wish to add from the pop-up menu in the Value field. This list should
/// be consistent with the languages listed in the AppLocalizations.supportedLocales
/// property.
abstract class AppLocalizations {
  AppLocalizations(String locale)
    : localeName = intl.Intl.canonicalizedLocale(locale.toString());

  final String localeName;

  static AppLocalizations of(BuildContext context) {
    return Localizations.of<AppLocalizations>(context, AppLocalizations)!;
  }

  static const LocalizationsDelegate<AppLocalizations> delegate =
      _AppLocalizationsDelegate();

  /// A list of this localizations delegate along with the default localizations
  /// delegates.
  ///
  /// Returns a list of localizations delegates containing this delegate along with
  /// GlobalMaterialLocalizations.delegate, GlobalCupertinoLocalizations.delegate,
  /// and GlobalWidgetsLocalizations.delegate.
  ///
  /// Additional delegates can be added by appending to this list in
  /// MaterialApp. This list does not have to be used at all if a custom list
  /// of delegates is preferred or required.
  static const List<LocalizationsDelegate<dynamic>> localizationsDelegates =
      <LocalizationsDelegate<dynamic>>[
        delegate,
        GlobalMaterialLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
      ];

  /// A list of this localizations delegate's supported locales.
  static const List<Locale> supportedLocales = <Locale>[
    Locale('en'),
    Locale('es'),
  ];

  /// No description provided for @appTitle.
  ///
  /// In en, this message translates to:
  /// **'Petyei'**
  String get appTitle;

  /// No description provided for @navExplore.
  ///
  /// In en, this message translates to:
  /// **'Explore'**
  String get navExplore;

  /// No description provided for @navPublish.
  ///
  /// In en, this message translates to:
  /// **'Publish'**
  String get navPublish;

  /// No description provided for @navAccount.
  ///
  /// In en, this message translates to:
  /// **'Account'**
  String get navAccount;

  /// No description provided for @exploreTitle.
  ///
  /// In en, this message translates to:
  /// **'Pets for adoption'**
  String get exploreTitle;

  /// No description provided for @exploreEmpty.
  ///
  /// In en, this message translates to:
  /// **'There are no pets for adoption here yet.'**
  String get exploreEmpty;

  /// No description provided for @publishTitle.
  ///
  /// In en, this message translates to:
  /// **'Publish an adoption'**
  String get publishTitle;

  /// No description provided for @publishIntro.
  ///
  /// In en, this message translates to:
  /// **'Create a listing so a family can find this pet.'**
  String get publishIntro;

  /// No description provided for @accountTitle.
  ///
  /// In en, this message translates to:
  /// **'Account'**
  String get accountTitle;

  /// No description provided for @languageLabel.
  ///
  /// In en, this message translates to:
  /// **'Language'**
  String get languageLabel;

  /// No description provided for @languageSystem.
  ///
  /// In en, this message translates to:
  /// **'Device language'**
  String get languageSystem;

  /// No description provided for @themeLabel.
  ///
  /// In en, this message translates to:
  /// **'Appearance'**
  String get themeLabel;

  /// No description provided for @themeSystem.
  ///
  /// In en, this message translates to:
  /// **'Device setting'**
  String get themeSystem;

  /// No description provided for @themeLight.
  ///
  /// In en, this message translates to:
  /// **'Light'**
  String get themeLight;

  /// No description provided for @themeDark.
  ///
  /// In en, this message translates to:
  /// **'Dark'**
  String get themeDark;

  /// No description provided for @countriesEmpty.
  ///
  /// In en, this message translates to:
  /// **'No countries are available yet.'**
  String get countriesEmpty;

  /// No description provided for @retryAction.
  ///
  /// In en, this message translates to:
  /// **'Try again'**
  String get retryAction;

  /// Country name for an ISO 3166-1 alpha-2 code.
  ///
  /// In en, this message translates to:
  /// **'{code, select, CO{Colombia} other{{code}}}'**
  String countryName(String code);

  /// Message for a stable API error code. Codes use underscores instead of dots.
  ///
  /// In en, this message translates to:
  /// **'{code, select, locations_invalid_country_code{Choose a valid country.} locations_country_not_enabled{Petyei is not available in this country yet.} locations_locality_not_available{Choose a city from the list.} common_not_found{We couldn\'t find what you were looking for.} other{We couldn\'t load this information. Check your connection and try again.}}'**
  String apiErrorMessage(String code);

  /// No description provided for @cityLabel.
  ///
  /// In en, this message translates to:
  /// **'Your city'**
  String get cityLabel;

  /// No description provided for @cityNotSet.
  ///
  /// In en, this message translates to:
  /// **'Choose your city to see pets near you.'**
  String get cityNotSet;

  /// No description provided for @changeAction.
  ///
  /// In en, this message translates to:
  /// **'Change'**
  String get changeAction;

  /// No description provided for @chooseAction.
  ///
  /// In en, this message translates to:
  /// **'Choose'**
  String get chooseAction;

  /// No description provided for @cityPickerTitle.
  ///
  /// In en, this message translates to:
  /// **'Choose your city'**
  String get cityPickerTitle;

  /// No description provided for @countryLabel.
  ///
  /// In en, this message translates to:
  /// **'Country'**
  String get countryLabel;

  /// No description provided for @citySearchHint.
  ///
  /// In en, this message translates to:
  /// **'Search city or municipality'**
  String get citySearchHint;

  /// No description provided for @citySearchEmpty.
  ///
  /// In en, this message translates to:
  /// **'We couldn\'t find “{query}”.'**
  String citySearchEmpty(String query);

  /// No description provided for @citySearchEmptyHint.
  ///
  /// In en, this message translates to:
  /// **'Check the spelling or search by municipality name.'**
  String get citySearchEmptyHint;

  /// No description provided for @clearSearchAction.
  ///
  /// In en, this message translates to:
  /// **'Clear search'**
  String get clearSearchAction;

  /// No description provided for @cityLocation.
  ///
  /// In en, this message translates to:
  /// **'{department}, {country}'**
  String cityLocation(String department, String country);
}

class _AppLocalizationsDelegate
    extends LocalizationsDelegate<AppLocalizations> {
  const _AppLocalizationsDelegate();

  @override
  Future<AppLocalizations> load(Locale locale) {
    return SynchronousFuture<AppLocalizations>(lookupAppLocalizations(locale));
  }

  @override
  bool isSupported(Locale locale) =>
      <String>['en', 'es'].contains(locale.languageCode);

  @override
  bool shouldReload(_AppLocalizationsDelegate old) => false;
}

AppLocalizations lookupAppLocalizations(Locale locale) {
  // Lookup logic when only language code is specified.
  switch (locale.languageCode) {
    case 'en':
      return AppLocalizationsEn();
    case 'es':
      return AppLocalizationsEs();
  }

  throw FlutterError(
    'AppLocalizations.delegate failed to load unsupported locale "$locale". This is likely '
    'an issue with the localizations generation tool. Please file an issue '
    'on GitHub with a reproducible sample app and the gen-l10n configuration '
    'that was used.',
  );
}
