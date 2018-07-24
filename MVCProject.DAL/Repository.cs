using System.Collections.Generic;
using System.Linq;

namespace MVCProject.DAL {
    public static class Repository {
        public static List<Drzava> GetCountries() {
            using (var model = new DataModel())
                return model.Drzava.ToList();
        }

        public static Kupac GetCustomer(int customerID) {
            using (var model = new DataModel())
                return model.Kupac.Include("Grad").First(customer => customer.IDKupac == customerID);
        }

        public static List<Grad> GetCitiesForCountry(int countryID) {
            using (var model = new DataModel())
                return model.Grad
                    .Where(grad => grad.DrzavaID == countryID)
                    .ToList();
        }

        public static Drzava GetCountry(int countryID, out int firstCityID) {
            using(var model = new DataModel()) {
                var country = model.Drzava.Find(countryID);

                if(country == null) {
                    firstCityID = -1;
                    return null;
                }

                firstCityID = model.Grad.First(grad => grad.DrzavaID == countryID).IDGrad;
                return country;
            }
        }

        public static bool UpdateCustomer(Kupac customer) {
            using(var model = new DataModel()) {
                var kupac = model.Kupac.Find(customer.IDKupac);

                kupac.Email = customer.Email;
                kupac.GradID = customer.GradID;
                kupac.Ime = customer.Ime;
                kupac.Prezime = customer.Prezime;
                kupac.Telefon = customer.Telefon;

                return model.SaveChanges() > 0;
            }
        }

        public static bool DeleteCustomer(int customerID)
        {
            using(var model = new DataModel())
            {
                var customer = model.Kupac.Find(customerID);

                var billsToRemove = model.Racun.Where(racun => racun.KupacID == customerID);
                var billsToRemoveIDs = billsToRemove.Select(bill => bill.IDRacun);
                model.Stavka.RemoveRange(model.Stavka.Where(stavka => billsToRemoveIDs.Contains(stavka.RacunID)));
                model.Racun.RemoveRange(billsToRemove);
                model.Kupac.Remove(customer);
                return model.SaveChanges() > 0;
            }
        }

        public static List<Kupac> GetCustomersForCity(int cityID) {
            using (var model = new DataModel())
                return model.Kupac
                    .Include("Grad")
                    .Where(kupac => kupac.GradID == cityID)
                    .ToList();
        }

        public static List<Kupac> GetCustomers() {
            using (var model = new DataModel())
                return model.Kupac.Include("Grad").ToList();
        }

        public static List<Kupac> GetCustomersFrom(int cityID, int countryID) {
            using (var model = new DataModel()) 
                return model.Kupac
                    .Include("Grad")
                    .Where(kupac => kupac.GradID == cityID && kupac.Grad.DrzavaID == countryID)
                    .ToList();
        }

        public static List<Racun> GetBillsForCustomer(int customerID) {
            using (var model = new DataModel())
                return model.Racun
                    .Include("Komercijalist")
                    .Include("KreditnaKartica")
                    .Where(racun => racun.KupacID == customerID)
                    .ToList();
        }

        public static List<Stavka> GetItemsForBill(int billID) {
            using (var model = new DataModel())
                return model.Stavka
                    .Include("Proizvod")
                    .Where(stavka => stavka.RacunID == billID)
                    .ToList();
        }

        public static Potkategorija GetSubcategory(int productID) {
            using (var model = new DataModel()) {
                var proizvod = model.Proizvod.Find(productID);

                return model.Potkategorija
                    .Include("Kategorija")
                    .First(potkategorija => potkategorija.IDPotkategorija == proizvod.PotkategorijaID);
            }
        }

        public static Drzava GetCountryByCityID(int? cityID) {
            using (var model = new DataModel())
                return model.Grad
                    .Include("Drzava")
                    .FirstOrDefault(grad => grad.IDGrad == cityID)?.Drzava;
        }

        public static Drzava GetCountry(int? countryID) {
            using (var model = new DataModel())
                return model.Drzava.Find(countryID);
        }

        public static bool CityIsFromCountry(int cityID, int countryID, out Drzava country) {
            using (var model = new DataModel()) {
                var city = model.Grad.Find(cityID);

                if (city?.DrzavaID == countryID) {
                    country = city?.Drzava;
                    return true;
                } else {
                    country = model.Drzava.Find(countryID);
                    return false;
                }
            }
        }

        //public static List<Racun> GetRacuniForKupac(int kupacID) {
        //    using (var model = new DataModel())
        //        return model.Racun
        //            .Include("Komercijalist")
        //            .Include("KreditnaKartica")
        //            .Where(racun => racun.KupacID == kupacID)
        //            .ToList();
        //}

        //public static List<Stavka> GetStavkeForRacun(int racunID) {
        //    using (var model = new DataModel())
        //        return model.Stavka
        //            .Include("Proizvod")
        //            .Where(stavka => stavka.RacunID == racunID)
        //            .ToList();
        //}
    }
}
