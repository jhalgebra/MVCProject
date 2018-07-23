using System.Linq;

using MVCProject.DAL;

namespace MVCProject.BLL {
    public class ViewItemsViewModel {
        public int BillID { get; }
        public PagedList<Item> Items { get; }

        public ViewItemsViewModel(int billID, int page) {
            BillID = billID;

            Items = new PagedList<Item>(Repository.GetItemsForBill(billID)
                .Select(stavka => Item.FromStavka(stavka))
            )
            {
                CurrentPage = page
            };
        }
    }
}
