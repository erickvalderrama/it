import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:petyei/app/router.dart';
import 'package:petyei/core/network/api_exception.dart';
import 'package:petyei/features/locations/domain/country.dart';
import 'package:petyei/features/locations/domain/country_repository.dart';
import 'package:petyei/features/locations/domain/locality.dart';
import 'package:petyei/features/locations/presentation/countries_controller.dart';
import 'package:petyei/features/locations/presentation/selected_city_controller.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../../helpers/pump_app.dart';

const _groups = [
  DepartmentLocalities(
    code: '08',
    displayName: 'Atlántico',
    localities: [Locality(id: 'a', code: '08001', displayName: 'Barranquilla')],
  ),
  DepartmentLocalities(
    code: '11',
    displayName: 'Bogotá, D.C.',
    localities: [Locality(id: 'b', code: '11001', displayName: 'Bogotá, D.C.')],
  ),
  DepartmentLocalities(
    code: '47',
    displayName: 'Magdalena',
    localities: [
      Locality(id: 'c', code: '47053', displayName: 'Aracataca'),
      Locality(id: 'd', code: '47001', displayName: 'Santa Marta'),
    ],
  ),
];

class _FakeRepository implements CountryRepository {
  _FakeRepository({this.failFirst = false});

  final bool failFirst;
  var localityCalls = 0;

  @override
  Future<List<Country>> listEnabled() async => const [Country('CO')];

  @override
  Future<List<DepartmentLocalities>> listLocalities(String countryCode) async {
    localityCalls++;
    if (failFirst && localityCalls == 1) {
      throw const ApiException(
        code: 'common.unexpected_error',
        statusCode: 503,
      );
    }
    return _groups;
  }
}

void _compact(WidgetTester tester) {
  tester.view.physicalSize = const Size(400, 900);
  tester.view.devicePixelRatio = 1;
  addTearDown(tester.view.reset);
}

Future<_FakeRepository> _openPicker(
  WidgetTester tester, {
  bool failFirst = false,
  Map<String, Object> preferences = const {},
}) async {
  final repository = _FakeRepository(failFirst: failFirst);
  await pumpApp(
    tester,
    location: AppRoutes.accountCity,
    preferences: preferences,
    overrides: [countryRepositoryProvider.overrideWithValue(repository)],
  );
  return repository;
}

void main() {
  testWidgets('Colombia is preselected and cities are grouped by department', (
    tester,
  ) async {
    _compact(tester);
    await _openPicker(tester);

    expect(find.text('Choose your city'), findsOneWidget);
    expect(find.text('Colombia'), findsOneWidget);
    expect(find.text('MAGDALENA'), findsOneWidget);
    expect(find.text('Santa Marta'), findsOneWidget);
  });

  testWidgets('search ignores accents and case', (tester) async {
    _compact(tester);
    await _openPicker(tester);

    await tester.enterText(find.byType(TextField), 'BOGOTA');
    await tester.pumpAndSettle();

    expect(find.text('BOGOTÁ, D.C.'), findsOneWidget);
    expect(find.text('MAGDALENA'), findsNothing);
  });

  testWidgets('an unmatched search offers to clear it', (tester) async {
    _compact(tester);
    await _openPicker(tester);

    await tester.enterText(find.byType(TextField), 'santa martha');
    await tester.pumpAndSettle();
    expect(
      find.textContaining("We couldn't find “santa martha”."),
      findsOneWidget,
    );

    await tester.tap(find.widgetWithText(FilledButton, 'Clear search'));
    await tester.pumpAndSettle();

    expect(find.text('Santa Marta'), findsOneWidget);
  });

  testWidgets('choosing a city stores it and returns to Account', (
    tester,
  ) async {
    _compact(tester);
    await _openPicker(tester);

    await tester.tap(find.text('Santa Marta'));
    await tester.pumpAndSettle();

    expect(find.text('Account'), findsWidgets);
    expect(find.text('Santa Marta'), findsOneWidget);
    expect(find.text('Magdalena, Colombia'), findsOneWidget);
    final stored = await SharedPreferences.getInstance();
    expect(stored.getString('location.selectedCity'), contains('"47001"'));
  });

  testWidgets('the stored city is marked as selected', (tester) async {
    _compact(tester);
    await _openPicker(
      tester,
      preferences: {
        'location.selectedCity':
            '{"countryCode":"CO","localityCode":"47001",'
            '"displayName":"Santa Marta","departmentName":"Magdalena"}',
      },
    );

    expect(find.byIcon(Icons.check), findsOneWidget);
  });

  testWidgets('a loading error offers retry', (tester) async {
    _compact(tester);
    final repository = await _openPicker(tester, failFirst: true);

    expect(find.text('Try again'), findsOneWidget);

    await tester.tap(find.text('Try again'));
    await tester.pumpAndSettle();

    expect(find.text('Santa Marta'), findsOneWidget);
    expect(repository.localityCalls, 2);
  });

  testWidgets('wide screens open the picker as a dialog over the app', (
    tester,
  ) async {
    tester.view.physicalSize = const Size(1280, 800);
    tester.view.devicePixelRatio = 1;
    addTearDown(tester.view.reset);

    await _openPicker(tester);

    expect(find.byType(Dialog), findsOneWidget);
    expect(find.byType(NavigationRail), findsOneWidget);
  });

  test('foldForSearch keeps positions and removes accents', () {
    expect(foldForSearch('Ciénaga'), 'cienaga');
    expect(foldForSearch('BOGOTÁ').length, 'BOGOTÁ'.length);
  });

  testWidgets('a broken stored city is ignored', (tester) async {
    _compact(tester);
    final container = await pumpApp(
      tester,
      location: AppRoutes.account,
      preferences: {'location.selectedCity': 'not json'},
    );

    expect(container.read(selectedCityProvider), isNull);
    expect(find.text('Choose your city to see pets near you.'), findsOneWidget);
  });
}
