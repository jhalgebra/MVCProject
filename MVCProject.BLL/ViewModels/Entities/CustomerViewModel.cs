using System.ComponentModel.DataAnnotations;

using MVCProject.DAL;

namespace MVCProject.BLL {
    public class CustomerViewModel {
        [Required]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(25)]
        public string Telephone { get; set; }

        [Required]
        [Display(Name = "City")]
        public int CityID { get; set; }
        public string City { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryID { get; set; }
        public string Country { get; set; }

        public static CustomerViewModel FromKupac(Kupac kupac, Drzava country) 
            => new CustomerViewModel {
                ID = kupac.IDKupac,
                Name = kupac.Ime,
                Surname = kupac.Prezime,
                Email = kupac.Email,
                Telephone = kupac.Telefon,
                CityID = kupac.GradID ?? -1,
                City = kupac.Grad.Naziv,
                CountryID = country.IDDrzava,
                Country = country.Naziv
            };
        public static CustomerViewModel FromKupac(Kupac kupac) => FromKupac(
            kupac,
            Repository.GetCountryByCityID(kupac.GradID)
        );

        public override string ToString() => $"{Name} {Surname}";

    }
}
