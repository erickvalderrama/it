import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:http/http.dart' as http;

import '../../../core/config/app_config.dart';
import '../data/http_country_repository.dart';
import '../domain/country.dart';
import '../domain/country_repository.dart';
import '../domain/list_enabled_countries.dart';
import '../domain/locality.dart';

final httpClientProvider = Provider<http.Client>((ref) {
  final client = http.Client();
  ref.onDispose(client.close);
  return client;
});

final countryRepositoryProvider = Provider<CountryRepository>(
  (ref) => HttpCountryRepository(
    baseUrl: AppConfig.apiBaseUrl,
    client: ref.watch(httpClientProvider),
  ),
);

/// Error states offer a retry action, so providers do not retry silently.
Duration? _noAutomaticRetry(int _, Object _) => null;

final countriesProvider = FutureProvider<List<Country>>(
  (ref) => ListEnabledCountries(ref.watch(countryRepositoryProvider))(),
  retry: _noAutomaticRetry,
);

final localitiesProvider =
    FutureProvider.family<List<DepartmentLocalities>, String>(
      (ref, countryCode) =>
          ref.watch(countryRepositoryProvider).listLocalities(countryCode),
      retry: _noAutomaticRetry,
    );
