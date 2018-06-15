using System.Collections.Generic;
using System.Linq;

using MVCProject.DAL;

namespace MVCProject.BLL {
    public class ViewBillsViewModel {
        public List<Bill> Bills { get; }

        public ViewBillsViewModel(int customerID) {
            Bills = Repository.GetBillsForCustomer(customerID)
                .Select(racun => Bill.FromRacun(racun))
                .ToList();
        }
    }
}
