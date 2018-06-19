using System.Collections.Generic;
using System.Linq;
using MVCProject.DAL;

namespace MVCProject.BLL {
    public class EditCustomerViewModel {
        public CustomerViewModel Customer { get; set; }

        public EditCustomerViewModel() { }

        public EditCustomerViewModel(int customerID) {
            var kupac = Repository.GetCustomer(customerID);

            Customer = CustomerViewModel.FromKupac(kupac, Repository.GetCountryByCityID(kupac.GradID));
        }

        public EditCustomerViewModel(int customerID, int countryID) {
            var kupac = Repository.GetCustomer(customerID);

            var country = Repository.GetCountry(countryID, out int firstCityID);
            kupac.GradID = firstCityID;

            Customer = CustomerViewModel.FromKupac(kupac, country);
        }

        public void UpdateCustomer() {
            Repository.UpdateCustomer(new Kupac {
                IDKupac = Customer.ID,
                Email = Customer.Email,
                GradID = Customer.CityID,
                Ime = Customer.Name,
                Prezime = Customer.Surname,
                Telefon = Customer.Telephone
            });
        }

        public IEnumerable<KeyValuePair<int, string>> GetCities()
            => GetGradovi().Select(grad => new KeyValuePair<int, string>(grad.IDGrad, grad.Naziv));

        public List<Grad> GetGradovi()
            => Repository.GetCitiesForCountry(Customer.CountryID);

        public List<Drzava> GetDrzave() => Repository.GetCountries();

        public IEnumerable<KeyValuePair<int, string>> GetCountries()
            => GetDrzave().Select(drzava => new KeyValuePair<int, string>(drzava.IDDrzava, drzava.Naziv));
    }
}
