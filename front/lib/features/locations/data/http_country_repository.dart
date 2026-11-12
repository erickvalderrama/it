import 'dart:convert';

import 'package:http/http.dart' as http;

import '../../../core/network/api_exception.dart';
import '../domain/country.dart';
import '../domain/country_repository.dart';
import '../domain/locality.dart';

class HttpCountryRepository implements CountryRepository {
  HttpCountryRepository({required this.baseUrl, required http.Client client})
    : _client = client;

  final String baseUrl;
  final http.Client _client;

  @override
  Future<List<Country>> listEnabled() async {
    final body = await _getList('/api/v1/locations/countries');
    return [
      for (final item in body.cast<Map<String, dynamic>>())
        Country(item['code'] as String),
    ];
  }

  @override
  Future<List<DepartmentLocalities>> listLocalities(String countryCode) async {
    final country = Uri.encodeComponent(countryCode);
    final body = await _getList(
      '/api/v1/locations/countries/$country/localities',
    );
    return [
      for (final group in body.cast<Map<String, dynamic>>())
        DepartmentLocalities(
          code: group['code'] as String,
          displayName: group['displayName'] as String,
          localities: [
            for (final item
                in (group['localities'] as List<dynamic>)
                    .cast<Map<String, dynamic>>())
              Locality(
                id: item['id'] as String,
                code: item['code'] as String,
                displayName: item['displayName'] as String,
              ),
          ],
        ),
    ];
  }

  Future<List<dynamic>> _getList(String path) async {
    final response = await _client.get(Uri.parse('$baseUrl$path'));
    if (response.statusCode != 200) {
      throw ApiException.fromResponse(response);
    }
    return jsonDecode(response.body) as List<dynamic>;
  }
}
