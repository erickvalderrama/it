import 'country.dart';
import 'country_repository.dart';

class ListEnabledCountries {
  const ListEnabledCountries(this._repository);

  final CountryRepository _repository;

  Future<List<Country>> call() => _repository.listEnabled();
}
