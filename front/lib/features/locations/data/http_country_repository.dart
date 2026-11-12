import 'dart:convert';

import 'package:http/http.dart' as http;

import '../domain/country.dart';
import '../domain/country_repository.dart';

class CountryLoadException implements Exception {
  const CountryLoadException(this.statusCode);

  final int statusCode;
}

class HttpCountryRepository implements CountryRepository {
  HttpCountryRepository({required this.baseUrl, required http.Client client})
    : _client = client;

  final String baseUrl;
  final http.Client _client;

  @override
  Future<List<Country>> listEnabled() async {
    final response = await _client.get(
      Uri.parse('$baseUrl/api/v1/locations/countries'),
    );
    if (response.statusCode != 200) {
      throw CountryLoadException(response.statusCode);
    }
    final body = jsonDecode(response.body) as List<dynamic>;
    return [
      for (final item in body.cast<Map<String, dynamic>>())
        Country(item['code'] as String),
    ];
  }
}
