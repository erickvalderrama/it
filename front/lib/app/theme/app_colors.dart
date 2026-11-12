import 'package:flutter/material.dart';

/// Semantic color tokens for the official "Bosque y pistacho" palette.
@immutable
class AppColors {
  const AppColors({
    required this.brightness,
    required this.primary,
    required this.onPrimary,
    required this.accentContainer,
    required this.onAccentContainer,
    required this.canvas,
    required this.surface,
    required this.onSurface,
  });

  final Brightness brightness;
  final Color primary;
  final Color onPrimary;
  final Color accentContainer;
  final Color onAccentContainer;
  final Color canvas;
  final Color surface;
  final Color onSurface;

  static const light = AppColors(
    brightness: Brightness.light,
    primary: Color(0xFF2B5735),
    onPrimary: Color(0xFFFFFFFF),
    accentContainer: Color(0xFFD7EC9E),
    onAccentContainer: Color(0xFF243329),
    canvas: Color(0xFFF5F8EE),
    surface: Color(0xFFFFFFFF),
    onSurface: Color(0xFF243329),
  );

  static const dark = AppColors(
    brightness: Brightness.dark,
    primary: Color(0xFFB9D88A),
    onPrimary: Color(0xFF16231A),
    accentContainer: Color(0xFF2F4A33),
    onAccentContainer: Color(0xFFD7EC9E),
    canvas: Color(0xFF121A14),
    surface: Color(0xFF1C2620),
    onSurface: Color(0xFFE3ECDF),
  );
}
