import '../../l10n/app_localizations.dart';
import 'api_exception.dart';

/// Localized message for any error shown to the user. Unknown codes and network
/// failures fall back to a generic message.
String errorMessage(AppLocalizations l10n, Object error) {
  final code = error is ApiException ? error.code : ApiException.unknownCode;
  // ICU select keys cannot contain dots, so `locations.invalid_country_code`
  // is looked up as `locations_invalid_country_code`.
  return l10n.apiErrorMessage(code.replaceAll('.', '_'));
}
