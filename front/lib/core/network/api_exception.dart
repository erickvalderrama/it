import 'dart:convert';

import 'package:http/http.dart' as http;

/// Error returned by the API as RFC 9457 problem details with a stable `code`.
class ApiException implements Exception {
  const ApiException({
    required this.code,
    required this.statusCode,
    this.field,
    this.correlationId,
  });

  /// Used when the response is not a problem document, such as a proxy error.
  static const unknownCode = 'common.unexpected_error';

  factory ApiException.fromResponse(http.Response response) {
    try {
      final body = jsonDecode(response.body) as Map<String, dynamic>;
      return ApiException(
        code: body['code'] as String? ?? unknownCode,
        statusCode: response.statusCode,
        field: body['field'] as String?,
        correlationId: body['correlationId'] as String?,
      );
    } on FormatException {
      return ApiException(code: unknownCode, statusCode: response.statusCode);
    } on TypeError {
      return ApiException(code: unknownCode, statusCode: response.statusCode);
    }
  }

  final String code;
  final int statusCode;
  final String? field;
  final String? correlationId;
}
