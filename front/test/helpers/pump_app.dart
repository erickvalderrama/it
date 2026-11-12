import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:flutter_riverpod/misc.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:petyei/app/app.dart';
import 'package:petyei/app/router.dart';
import 'package:petyei/core/settings/app_settings.dart';
import 'package:shared_preferences/shared_preferences.dart';

/// Starts the real app at [location] with stored [preferences] and provider overrides.
Future<ProviderContainer> pumpApp(
  WidgetTester tester, {
  String location = AppRoutes.explore,
  Map<String, Object> preferences = const {},
  List<Override> overrides = const [],
}) async {
  SharedPreferences.setMockInitialValues(preferences);
  final stored = await SharedPreferences.getInstance();
  final container = ProviderContainer(
    overrides: [
      sharedPreferencesProvider.overrideWithValue(stored),
      ...overrides,
    ],
  );
  addTearDown(container.dispose);

  container.read(routerProvider).go(location);
  await tester.pumpWidget(
    UncontrolledProviderScope(container: container, child: const PetyeiApp()),
  );
  await tester.pumpAndSettle();
  return container;
}
