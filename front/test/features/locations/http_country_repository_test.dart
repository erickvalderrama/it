import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/testing.dart';
import 'package:petyei/features/locations/data/http_country_repository.dart';
import 'package:petyei/features/locations/domain/country.dart';

void main() {
  test('listEnabled maps the API contract to countries', () async {
    final client = MockClient((request) async {
      expect(request.url.path, '/api/v1/locations/countries');
      return http.Response('[{"code":"CO"}]', 200);
    });

    final repository = HttpCountryRepository(
      baseUrl: 'http://api.test',
      client: client,
    );

    expect(await repository.listEnabled(), [const Country('CO')]);
  });

  test('listEnabled throws when the API fails', () async {
    final client = MockClient((_) async => http.Response('', 503));

    final repository = HttpCountryRepository(
      baseUrl: 'http://api.test',
      client: client,
    );

    await expectLater(
      repository.listEnabled(),
      throwsA(isA<CountryLoadException>()),
    );
  });
}
