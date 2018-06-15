using System.Collections.Generic;

using MVCProject.DAL;

namespace MVCProject.BLL {
    public class EditCustomerViewModel {
        public CustomerViewModel Customer { get; set; }

        public EditCustomerViewModel(int customerID) {
            var kupac = Repository.GetCustomer(customerID);

            Customer = CustomerViewModel.FromKupac(kupac, Repository.GetCountryID(kupac.GradID).Value);
        }

        public List<Grad> GetGradovi()
            => Repository.GetCitiesForCountry(Customer.CountryID);
        public List<Drzava> GetDrzave() => Repository.GetCountries();
    }
}
