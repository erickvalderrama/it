import 'package:flutter/material.dart';

import 'app_colors.dart';

abstract final class AppTheme {
  static ThemeData get light => build(AppColors.light);

  static ThemeData get dark => build(AppColors.dark);

  static ThemeData build(AppColors colors) {
    final scheme =
        ColorScheme.fromSeed(
          seedColor: colors.primary,
          brightness: colors.brightness,
        ).copyWith(
          primary: colors.primary,
          onPrimary: colors.onPrimary,
          primaryContainer: colors.accentContainer,
          onPrimaryContainer: colors.onAccentContainer,
          surface: colors.surface,
          onSurface: colors.onSurface,
        );

    return ThemeData(
      colorScheme: scheme,
      scaffoldBackgroundColor: colors.canvas,
      appBarTheme: AppBarTheme(
        backgroundColor: colors.canvas,
        foregroundColor: colors.onSurface,
      ),
    );
  }
}
