import 'country.dart';

abstract interface class CountryRepository {
  Future<List<Country>> listEnabled();
}
