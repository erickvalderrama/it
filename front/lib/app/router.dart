import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:go_router/go_router.dart';

import '../features/account/presentation/account_screen.dart';
import '../features/explore/presentation/explore_screen.dart';
import '../features/locations/presentation/city_picker_page.dart';
import '../features/publish/presentation/publish_screen.dart';
import 'app_shell.dart';

abstract final class AppRoutes {
  static const explore = '/explore';
  static const publish = '/publish';
  static const account = '/account';
  static const accountCity = '/account/city';
}

/// Every screen has its own URL so links, refresh, and back navigation work on web.
final routerProvider = Provider<GoRouter>((ref) {
  final rootNavigatorKey = GlobalKey<NavigatorState>();
  final router = GoRouter(
    navigatorKey: rootNavigatorKey,
    initialLocation: AppRoutes.explore,
    routes: [
      StatefulShellRoute.indexedStack(
        builder: (context, state, shell) => AppShell(navigationShell: shell),
        branches: [
          StatefulShellBranch(
            routes: [
              GoRoute(
                path: AppRoutes.explore,
                builder: (_, _) => const ExploreScreen(),
              ),
            ],
          ),
          StatefulShellBranch(
            routes: [
              GoRoute(
                path: AppRoutes.publish,
                builder: (_, _) => const PublishScreen(),
              ),
            ],
          ),
          StatefulShellBranch(
            routes: [
              GoRoute(
                path: AppRoutes.account,
                builder: (_, _) => const AccountScreen(),
                routes: [
                  GoRoute(
                    path: 'city',
                    // Above the shell: full screen on mobile, dialog over everything on web.
                    parentNavigatorKey: rootNavigatorKey,
                    pageBuilder: (_, state) =>
                        CityPickerPage(key: state.pageKey),
                  ),
                ],
              ),
            ],
          ),
        ],
      ),
    ],
  );
  ref.onDispose(router.dispose);
  return router;
});
