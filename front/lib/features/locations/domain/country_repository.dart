import 'country.dart';
import 'locality.dart';

abstract interface class CountryRepository {
  Future<List<Country>> listEnabled();

  /// Selectable localities of [countryCode], grouped by department.
  Future<List<DepartmentLocalities>> listLocalities(String countryCode);
}
