using MVCProject.DAL;
using System.ComponentModel.DataAnnotations;

namespace MVCProject.BLL {
    public class CustomerViewModel {
        public int ID { get; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(25)]
        public string Telephone { get; set; }

        [Required]
        [Display(Name = "City")]
        public int CityID { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryID { get; set; }

        public CustomerViewModel(int id) {
            ID = id;
        }

        public static CustomerViewModel FromKupac(Kupac kupac, int countryID) 
            => new CustomerViewModel(kupac.IDKupac) {
                Name = kupac.Ime,
                Surname = kupac.Prezime,
                Email = kupac.Email,
                Telephone = kupac.Telefon,
                CityID = kupac.GradID ?? -1,
                CountryID = countryID
            };
    }
}
