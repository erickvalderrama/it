import 'dart:convert';

import 'package:flutter_riverpod/flutter_riverpod.dart';

import '../../../core/settings/app_settings.dart';
import '../domain/locality.dart';

final selectedCityProvider =
    NotifierProvider<SelectedCityController, SelectedCity?>(
      SelectedCityController.new,
    );

/// City chosen by a visitor or user. Stored on the device; registration will
/// send it to the server, which validates it.
class SelectedCityController extends Notifier<SelectedCity?> {
  static const _key = 'location.selectedCity';

  @override
  SelectedCity? build() {
    final stored = ref.watch(sharedPreferencesProvider).getString(_key);
    if (stored == null) {
      return null;
    }
    try {
      return SelectedCity.fromJson(jsonDecode(stored) as Map<String, dynamic>);
    } on FormatException {
      return null;
    } on TypeError {
      return null;
    }
  }

  Future<void> select(SelectedCity city) async {
    state = city;
    await ref
        .read(sharedPreferencesProvider)
        .setString(_key, jsonEncode(city.toJson()));
  }
}
