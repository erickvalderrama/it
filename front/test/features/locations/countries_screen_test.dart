import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:petyei/app/app.dart';
import 'package:petyei/features/locations/domain/country.dart';
import 'package:petyei/features/locations/domain/country_repository.dart';
import 'package:petyei/features/locations/presentation/countries_controller.dart';

class _FakeRepository implements CountryRepository {
  _FakeRepository(this._responses);

  final List<Future<List<Country>> Function()> _responses;
  var _calls = 0;

  @override
  Future<List<Country>> listEnabled() => _responses[_calls++]();
}

Widget _app(
  CountryRepository repository, {
  Locale locale = const Locale('en'),
  ThemeMode themeMode = ThemeMode.light,
}) {
  return ProviderScope(
    overrides: [countryRepositoryProvider.overrideWithValue(repository)],
    child: PetyeiApp(locale: locale, themeMode: themeMode),
  );
}

void main() {
  testWidgets('shows enabled countries in English', (tester) async {
    await tester.pumpWidget(
      _app(
        _FakeRepository([
          () async => const [Country('CO')],
        ]),
      ),
    );
    await tester.pumpAndSettle();

    expect(find.text('Available countries'), findsOneWidget);
    expect(find.text('Colombia'), findsOneWidget);
  });

  testWidgets('shows enabled countries in Spanish', (tester) async {
    await tester.pumpWidget(
      _app(
        _FakeRepository([
          () async => const [Country('CO')],
        ]),
        locale: const Locale('es'),
      ),
    );
    await tester.pumpAndSettle();

    expect(find.text('Países disponibles'), findsOneWidget);
    expect(find.text('Colombia'), findsOneWidget);
  });

  testWidgets('uses the dark palette in dark mode', (tester) async {
    await tester.pumpWidget(
      _app(
        _FakeRepository([
          () async => const [Country('CO')],
        ]),
        themeMode: ThemeMode.dark,
      ),
    );
    await tester.pumpAndSettle();

    final context = tester.element(find.text('Colombia'));
    expect(Theme.of(context).brightness, Brightness.dark);
    expect(Theme.of(context).colorScheme.primary, const Color(0xFFB9D88A));
  });

  testWidgets('shows an empty state', (tester) async {
    await tester.pumpWidget(_app(_FakeRepository([() async => const []])));
    await tester.pumpAndSettle();

    expect(find.text('No countries are available yet.'), findsOneWidget);
  });

  testWidgets('recovers from an error with retry', (tester) async {
    await tester.pumpWidget(
      _app(
        _FakeRepository([
          () async => throw Exception('offline'),
          () async => const [Country('CO')],
        ]),
      ),
    );
    await tester.pumpAndSettle();

    expect(find.text('Try again'), findsOneWidget);

    await tester.tap(find.text('Try again'));
    await tester.pumpAndSettle();

    expect(find.text('Colombia'), findsOneWidget);
  });
}
