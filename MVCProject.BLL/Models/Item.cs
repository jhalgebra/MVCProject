using MVCProject.DAL;

namespace MVCProject.BLL {
    public class Item {
        public short Quantity { get; set; }
        public decimal CostPerUnit { get; set; }
        public decimal DiscountInPercentage { get; set; }
        public decimal TotalPrice { get; set; }
        public Product Product { get; set; }

        public static Item FromStavka(Stavka stavka) {
            var subcategory = Repository.GetSubcategory(stavka.ProizvodID);

            return new Item {
                CostPerUnit = stavka.CijenaPoKomadu,
                DiscountInPercentage = stavka.PopustUPostocima,
                Quantity = stavka.Kolicina,
                TotalPrice = stavka.UkupnaCijena,
                Product = new Product {
                    Color = stavka.Proizvod.Boja,
                    MinumalStoredQuantity = stavka.Proizvod.MinimalnaKolicinaNaSkladistu,
                    Name = stavka.Proizvod.Naziv,
                    Number = stavka.Proizvod.BrojProizvoda,
                    PriceWithoutVAT = stavka.Proizvod.CijenaBezPDV,
                    Category = subcategory.Kategorija.Naziv,
                    Subcategory = subcategory.Naziv
                }
            };
        }
    }
}
