using System.Collections.Generic;
using System.Linq;
using MVCProject.DAL;

namespace MVCProject.BLL {
    public class ViewCustomersViewModel {
        public int? CityID {
            get;
            set;
        }
        public int? CountryID {
            get;
            set;
        }

        public ViewCustomersViewModel() {
            Countries = Repository.GetCountries();

            if (Countries.Count == 0)
                return;

            var first = Countries[0];
            CountryID = first.IDDrzava;

            CityID = GetFirstCityID(first.IDDrzava);
        }

        public List<CustomerViewModel> GetCustomers() {
            return Repository.GetCustomersForCity(
                Repository.CityIsFromCountry(CityID.Value, CountryID.Value)
                ? CityID.Value
                : GetFirstCityID(CountryID.Value))
                    .Select(kupac => CustomerViewModel.FromKupac(kupac, CountryID.Value))
                    .ToList();
        }

        private int GetFirstCityID(int countryID)
            => Repository.GetCitiesForCountry(countryID).First().IDGrad;

        public List<Grad> Cities => Repository.GetCitiesForCountry(CountryID.Value);

        public List<Drzava> Countries { get; }
    }
}
