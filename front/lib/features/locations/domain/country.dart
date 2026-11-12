import 'package:flutter/foundation.dart';

@immutable
class Country {
  const Country(this.code);

  /// ISO 3166-1 alpha-2 code returned by the API.
  final String code;

  @override
  bool operator ==(Object other) => other is Country && other.code == code;

  @override
  int get hashCode => code.hashCode;
}
