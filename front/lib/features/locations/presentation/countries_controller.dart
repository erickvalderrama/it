import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:http/http.dart' as http;

import '../../../core/config/app_config.dart';
import '../data/http_country_repository.dart';
import '../domain/country.dart';
import '../domain/country_repository.dart';
import '../domain/list_enabled_countries.dart';

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

final countriesControllerProvider =
    AsyncNotifierProvider<CountriesController, List<Country>>(
      CountriesController.new,
      // The error state offers a retry action; avoid silent automatic retries.
      retry: (_, _) => null,
    );

class CountriesController extends AsyncNotifier<List<Country>> {
  @override
  Future<List<Country>> build() =>
      ListEnabledCountries(ref.watch(countryRepositoryProvider))();

  Future<void> retry() async {
    state = const AsyncLoading();
    state = await AsyncValue.guard(build);
  }
}
