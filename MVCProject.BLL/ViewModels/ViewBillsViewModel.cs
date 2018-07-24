using System;
using System.Collections.Generic;
using System.Linq;

using MVCProject.DAL;

namespace MVCProject.BLL
{
    public class ViewBillsViewModel
    {
        #region Fields

        private int customerID;
        private BillSort sort;
        private bool ascendingSort;

        #endregion

        #region Properties

        public int CustomerID
        {
            get => customerID;
            set
            {
                if (customerID == value)
                    return;

                customerID = value;
                SortBills();
            }
        }
        public BillSort Sort
        {
            get => sort;
            set
            {
                if (sort != value)
                {
                    sort = value;
                    SortBills();
                }
            }
        }
        public bool AscendingSort
        {
            get => ascendingSort;
            set
            {
                if (ascendingSort != value)
                {
                    ascendingSort = value;
                    SortBills();
                }
            }
        }

        public PagedList<Bill> Bills { get; }

        #endregion

        #region Constructors

        public ViewBillsViewModel(int currentPage)
        {

            Bills = new PagedList<Bill>(null)
            {
                CurrentPage = currentPage
            };
        }

        public ViewBillsViewModel() : this(1) { } 

        #endregion

        #region Sort helpers

        private void SortBills()
        {
            Bills.SetCollection(SortBills(
                Repository.GetBillsForCustomer(CustomerID)
                .Select(racun => Bill.FromRacun(racun))
                .Where(racun => racun != null)
            ).ToList());
        }

        private IEnumerable<Bill> SortBills(IEnumerable<Bill> bills)
        {
            return AscendingSort
                ? bills.OrderBy(KeySelector)
                : bills.OrderByDescending(KeySelector);
        }

        private object KeySelector(Bill bill)
        {
            switch (Sort)
            {

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
