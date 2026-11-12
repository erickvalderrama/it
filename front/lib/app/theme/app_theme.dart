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
      // Rounded, filled fields as in the mockups.
      inputDecorationTheme: InputDecorationTheme(
        filled: true,
        fillColor: colors.surface,
        border: OutlineInputBorder(borderRadius: BorderRadius.circular(12)),
        enabledBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12),
          borderSide: BorderSide(color: scheme.outlineVariant),
        ),
        focusedBorder: OutlineInputBorder(
          borderRadius: BorderRadius.circular(12),
          borderSide: BorderSide(color: colors.primary, width: 2),
        ),
      ),
    );
  }
}
