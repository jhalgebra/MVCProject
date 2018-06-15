using System;
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
                return model.Kupac.Find(customerID);//ToList().FirstOrDefault(kupac => kupac.IDKupac == customerID);
        }

        public static List<Grad> GetCitiesForCountry(int countryID) {
            using (var model = new DataModel())
                return model.Grad
                    .Where(grad => grad.DrzavaID == countryID)
                    .ToList();
        }

        public static List<Kupac> GetCustomersForCity(int cityID) {
            using (var model = new DataModel())
                return model.Kupac
                    .Where(kupac => kupac.GradID == cityID)
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

        public static int? GetCountryID(int? gradID) {
            using (var model = new DataModel())
                return model.Grad.FirstOrDefault(grad => grad.IDGrad == gradID)?.DrzavaID;
        }

        public static bool CityIsFromCountry(int cityID, int countryID) {
            using (var model = new DataModel())
                return model.Grad.Find(cityID)?.DrzavaID == countryID;
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
