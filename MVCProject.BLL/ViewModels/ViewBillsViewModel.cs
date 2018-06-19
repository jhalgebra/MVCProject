using System;
using System.Collections.Generic;
using System.Linq;

using MVCProject.DAL;

namespace MVCProject.BLL {
    public class ViewBillsViewModel {
        #region Properties

        public int CustomerID { get; set; }
        public BillSort Sort { get; set; }
        public bool AscendingSort { get; set; }

        #endregion

        #region Methods

        public List<Bill> GetBills() => SortBills(
            Repository.GetBillsForCustomer(CustomerID)
            .Select(racun => Bill.FromRacun(racun))
        ).ToList();

        private IEnumerable<Bill> SortBills(IEnumerable<Bill> bills) {
            return AscendingSort
                ? bills.OrderBy(KeySelector)
                : bills.OrderByDescending(KeySelector);
        }

        private object KeySelector(Bill bill) {
            switch (Sort) {

                case BillSort.Date:
                    return bill.DateIssued;

                case BillSort.Commercialist:
                    return bill.Commercialist.FullName;

                case BillSort.CreditCardType:
                    return bill.CreditCard.Type;

                default:
                    throw new Exception($"Uknown {nameof(BillSort)} enumeration");

            }
        }

        #endregion
    }
}
