import 'package:flutter/material.dart';
import 'package:flutter_riverpod/flutter_riverpod.dart';
import 'package:shared_preferences/shared_preferences.dart';

/// Loaded before `runApp` and overridden in `main` and tests.
final sharedPreferencesProvider = Provider<SharedPreferences>(
  (ref) => throw UnimplementedError('Override sharedPreferencesProvider.'),
);

@immutable
class AppSettings {
  const AppSettings({this.languageCode, this.themeMode = ThemeMode.system});

  /// Null follows the device language, falling back to English.
  final String? languageCode;
  final ThemeMode themeMode;

  Locale? get locale => languageCode == null ? null : Locale(languageCode!);
}

final appSettingsProvider =
    NotifierProvider<AppSettingsController, AppSettings>(
      AppSettingsController.new,
    );

/// Keeps language and theme on the device. Syncing them to the account comes with sign-in.
class AppSettingsController extends Notifier<AppSettings> {
  static const _languageKey = 'settings.languageCode';
  static const _themeKey = 'settings.themeMode';

  SharedPreferences get _preferences => ref.read(sharedPreferencesProvider);

  @override
  AppSettings build() {
    final preferences = ref.watch(sharedPreferencesProvider);
    final storedTheme = preferences.getString(_themeKey);
    return AppSettings(
      languageCode: preferences.getString(_languageKey),
      themeMode: ThemeMode.values.firstWhere(
        (mode) => mode.name == storedTheme,
        orElse: () => ThemeMode.system,
      ),
    );
  }

  Future<void> setLanguage(String? languageCode) async {
    state = AppSettings(languageCode: languageCode, themeMode: state.themeMode);
    if (languageCode == null) {
      await _preferences.remove(_languageKey);
    } else {
      await _preferences.setString(_languageKey, languageCode);
    }
  }

  Future<void> setThemeMode(ThemeMode themeMode) async {
    state = AppSettings(languageCode: state.languageCode, themeMode: themeMode);
    await _preferences.setString(_themeKey, themeMode.name);
  }
}
