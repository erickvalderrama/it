import 'package:flutter/foundation.dart';

@immutable
class Locality {
  const Locality({
    required this.id,
    required this.code,
    required this.displayName,
  });

  final String id;

  /// Official code, such as DIVIPOLA "47001".
  final String code;
  final String displayName;
}

@immutable
class DepartmentLocalities {
  const DepartmentLocalities({
    required this.code,
    required this.displayName,
    required this.localities,
  });

  final String code;
  final String displayName;
  final List<Locality> localities;
}

/// The city a person chose, kept on the device until it is stored in the account.
@immutable
class SelectedCity {
  const SelectedCity({
    required this.countryCode,
    required this.localityCode,
    required this.displayName,
    required this.departmentName,
  });

  factory SelectedCity.fromJson(Map<String, dynamic> json) => SelectedCity(
    countryCode: json['countryCode'] as String,
    localityCode: json['localityCode'] as String,
    displayName: json['displayName'] as String,
    departmentName: json['departmentName'] as String,
  );

  final String countryCode;
  final String localityCode;
  final String displayName;
  final String departmentName;

  Map<String, dynamic> toJson() => {
    'countryCode': countryCode,
    'localityCode': localityCode,
    'displayName': displayName,
    'departmentName': departmentName,
  };
}

/// Folds accents and case so "bogota" matches "Bogotá". Keeps one character
/// per input character, so match positions map back to the original text.
String foldForSearch(String text) {
  const from = 'áàäâãéèëêíìïîóòöôõúùüûñçÁÀÄÂÃÉÈËÊÍÌÏÎÓÒÖÔÕÚÙÜÛÑÇ';
  const to = 'aaaaaeeeeiiiiooooouuuuncaaaaaeeeeiiiiooooouuuunc';
  final buffer = StringBuffer();
  for (final char in text.split('')) {
    final index = from.indexOf(char);
    buffer.write(index >= 0 ? to[index] : char.toLowerCase());
  }
  return buffer.toString();
}
