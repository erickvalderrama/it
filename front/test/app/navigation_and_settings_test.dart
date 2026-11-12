import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:petyei/app/router.dart';
import 'package:petyei/core/settings/app_settings.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../helpers/pump_app.dart';

void _setSize(WidgetTester tester, Size size) {
  tester.view.physicalSize = size;
  tester.view.devicePixelRatio = 1;
  addTearDown(tester.view.reset);
}

void main() {
  testWidgets('a deep link opens its section on compact screens', (
    tester,
  ) async {
    _setSize(tester, const Size(400, 800));
    await pumpApp(tester, location: AppRoutes.account);

    expect(find.byType(NavigationBar), findsOneWidget);
    expect(find.byType(NavigationRail), findsNothing);
    expect(find.text('Language'), findsOneWidget);
  });

  testWidgets('wide screens use a side rail and keep navigation', (
    tester,
  ) async {
    _setSize(tester, const Size(1200, 800));
    final container = await pumpApp(tester);

    expect(find.byType(NavigationRail), findsOneWidget);
    expect(
      find.text('There are no pets for adoption here yet.'),
      findsOneWidget,
    );

    await tester.tap(find.text('Publish'));
    await tester.pumpAndSettle();

    expect(find.text('Publish an adoption'), findsOneWidget);
    expect(
      container.read(routerProvider).routeInformationProvider.value.uri.path,
      AppRoutes.publish,
    );
  });

  testWidgets('changing the language updates the screen, keeps the route, '
      'and is stored on the device', (tester) async {
    _setSize(tester, const Size(400, 900));
    final container = await pumpApp(tester, location: AppRoutes.account);

    await tester.tap(find.text('Español'));
    await tester.pumpAndSettle();

    expect(find.text('Idioma'), findsOneWidget);
    expect(find.text('Cuenta'), findsWidgets);
    expect(
      container.read(routerProvider).routeInformationProvider.value.uri.path,
      AppRoutes.account,
    );
    final stored = await SharedPreferences.getInstance();
    expect(stored.getString('settings.languageCode'), 'es');
  });

  testWidgets('choosing dark mode applies it at once and stores it', (
    tester,
  ) async {
    _setSize(tester, const Size(400, 900));
    final container = await pumpApp(tester, location: AppRoutes.account);

    await tester.tap(find.text('Dark'));
    await tester.pumpAndSettle();

    final context = tester.element(find.text('Appearance'));
    expect(Theme.of(context).brightness, Brightness.dark);
    expect(container.read(appSettingsProvider).themeMode, ThemeMode.dark);
    final stored = await SharedPreferences.getInstance();
    expect(stored.getString('settings.themeMode'), 'dark');
  });

  testWidgets('stored settings are restored on start', (tester) async {
    _setSize(tester, const Size(400, 900));
    await pumpApp(
      tester,
      location: AppRoutes.account,
      preferences: {
        'settings.languageCode': 'es',
        'settings.themeMode': 'dark',
      },
    );

    final context = tester.element(find.text('Apariencia'));
    expect(Theme.of(context).brightness, Brightness.dark);
  });
}
