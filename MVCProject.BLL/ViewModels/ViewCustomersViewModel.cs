using System.Collections.Generic;
using System.Linq;

using MVCProject.DAL;

namespace MVCProject.BLL {
    public class ViewCustomersViewModel {
        private int cityID, countryID;

        public int CountryID {
            get => countryID;
            set {
                if (countryID == value)
                    return;

                countryID = value;
                CityID = GetFirstCityID();
            }
        }

        public int CityID {
            get => cityID;
            set {
                if (cityID == value)
                    return;

                cityID = value;
                Customers = new PagedList<CustomerViewModel>(GetCustomers()) {
                    //PageSize = 20,
                    CurrentPage = 1
                };
            }
        }

        public PagedList<CustomerViewModel> Customers { get; private set; }

        public ViewCustomersViewModel() {
            Countries = Repository.GetCountries();

            if (Countries.Count == 0)
                return;

            CountryID = Countries[0].IDDrzava;
        }

        private List<CustomerViewModel> GetCustomers() {
            return Repository.GetCustomersForCity(
                Repository.CityIsFromCountry(cityID, countryID, out Drzava country)
                ? cityID
                : GetFirstCityID())
                    .Select(kupac => CustomerViewModel.FromKupac(kupac, country))
                    .ToList();
        }

        private int GetFirstCityID()
            => Repository.GetCitiesForCountry(countryID).First().IDGrad;

        private int GetFirstCityID(int countryID)
            => Repository.GetCitiesForCountry(countryID).First().IDGrad;

        public List<Grad> Cities => Repository.GetCitiesForCountry(countryID);

        public List<Drzava> Countries { get; }
    }
}