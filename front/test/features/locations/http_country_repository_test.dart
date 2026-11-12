import 'package:flutter_test/flutter_test.dart';
import 'package:http/http.dart' as http;
import 'package:http/testing.dart';
import 'package:petyei/core/network/api_exception.dart';
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
      throwsA(
        isA<ApiException>().having(
          (e) => e.code,
          'code',
          ApiException.unknownCode,
        ),
      ),
    );
  });

  test('listEnabled exposes the stable code of a problem response', () async {
    final client = MockClient(
      (_) async => http.Response(
        '{"status":400,"code":"locations.invalid_country_code","field":"code","correlationId":"abc12345"}',
        400,
      ),
    );

    final repository = HttpCountryRepository(
      baseUrl: 'http://api.test',
      client: client,
    );

    await expectLater(
      repository.listEnabled(),
      throwsA(
        isA<ApiException>()
            .having((e) => e.code, 'code', 'locations.invalid_country_code')
            .having((e) => e.field, 'field', 'code')
            .having((e) => e.correlationId, 'correlationId', 'abc12345'),
      ),
    );
  });

  test('listLocalities maps departments and localities', () async {
    final client = MockClient((request) async {
      expect(request.url.path, '/api/v1/locations/countries/CO/localities');
      return http.Response.bytes(
        '[{"code":"47","name":"MAGDALENA","displayName":"Magdalena",'
                '"localities":[{"id":"x","code":"47001","name":"SANTA MARTA",'
                '"displayName":"Santa Marta"}]}]'
            .codeUnits,
        200,
      );
    });

    final repository = HttpCountryRepository(
      baseUrl: 'http://api.test',
      client: client,
    );
    final groups = await repository.listLocalities('CO');

    expect(groups.single.displayName, 'Magdalena');
    expect(groups.single.localities.single.code, '47001');
  });
}
