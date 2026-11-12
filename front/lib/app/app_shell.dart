import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';

import '../core/l10n/l10n.dart';

/// Main navigation: a bottom bar on compact widths and a side rail on wider ones.
class AppShell extends StatelessWidget {
  const AppShell({super.key, required this.navigationShell});

  /// Width from which the side rail replaces the bottom bar.
  static const wideBreakpoint = 840.0;

  final StatefulNavigationShell navigationShell;

  void _select(int index) => navigationShell.goBranch(
    index,
    initialLocation: index == navigationShell.currentIndex,
  );

  @override
  Widget build(BuildContext context) {
    final l10n = context.l10n;
    final destinations = [
      (Icons.search, l10n.navExplore),
      (Icons.add_circle_outline, l10n.navPublish),
      (Icons.person_outline, l10n.navAccount),
    ];

    return LayoutBuilder(
      builder: (context, constraints) {
        if (constraints.maxWidth >= wideBreakpoint) {
          return Scaffold(
            body: Row(
              children: [
                NavigationRail(
                  selectedIndex: navigationShell.currentIndex,
                  onDestinationSelected: _select,
                  labelType: NavigationRailLabelType.all,
                  destinations: [
                    for (final (icon, label) in destinations)
                      NavigationRailDestination(
                        icon: Icon(icon),
                        label: Text(label),
                      ),
                  ],
                ),
                const VerticalDivider(width: 1),
                Expanded(child: navigationShell),
              ],
            ),
          );
        }
        return Scaffold(
          body: navigationShell,
          bottomNavigationBar: NavigationBar(
            selectedIndex: navigationShell.currentIndex,
            onDestinationSelected: _select,
            destinations: [
              for (final (icon, label) in destinations)
                NavigationDestination(icon: Icon(icon), label: label),
            ],
          ),
        );
      },
    );
  }
}
