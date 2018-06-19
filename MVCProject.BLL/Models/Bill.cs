using MVCProject.DAL;
using System;

namespace MVCProject.BLL {
    public class Bill {
        public int ID { get; }
        public DateTime DateIssued { get; set; }
        public string Number { get; set; }
        public Commercialist Commercialist { get; set; }
        public CreditCard CreditCard { get; set; }

        public Bill(int id) {
            ID = id;
        }

        public static Bill FromRacun(Racun racun)
            => new Bill(racun.IDRacun) {
                DateIssued = racun.DatumIzdavanja,
                Number = racun.BrojRacuna,
                Commercialist = new Commercialist {
                    FullName = $"{racun.Komercijalist.Ime} {racun.Komercijalist.Prezime}",
                    FullTime = racun.Komercijalist.StalniZaposlenik.Value
                },
                CreditCard = new CreditCard {
                    ExpiryDate = new DateTime(
                       racun.KreditnaKartica.IstekGodina,
                       racun.KreditnaKartica.IstekMjesec,
                       1
                   ),
                    Number = racun.KreditnaKartica.Broj,
                    Type = racun.KreditnaKartica.Tip
                }
            };
    }
}
