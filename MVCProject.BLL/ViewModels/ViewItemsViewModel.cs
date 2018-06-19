using MVCProject.DAL;
using System.Collections.Generic;
using System.Linq;

namespace MVCProject.BLL {
    public class ViewItemsViewModel {
        public List<Item> Items { get; }

        public ViewItemsViewModel(int billID) {
            Items = Repository.GetItemsForBill(billID)
                .Select(stavka => Item.FromStavka(stavka))
                .ToList();
        }
    }
}
