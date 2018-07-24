using System.Linq;

using MVCProject.DAL;

namespace MVCProject.BLL {
    public class ViewItemsViewModel {
        public int BillID { get; }
        public int CustomerID { get; }
        public PagedList<Item> Items { get; }

        public ViewItemsViewModel(int customerID, int billID, int page) {
            BillID = billID;
            CustomerID = customerID;

            Items = new PagedList<Item>(Repository.GetItemsForBill(billID)
                .Select(stavka => Item.FromStavka(stavka))
            ) { CurrentPage = page };
        }
    }
}
