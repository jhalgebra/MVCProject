using System.Web.Mvc;

using MVCProject.BLL;

namespace MVCProject.WebClient.Controllers {
    public class AdventureWorksController : Controller {
        public ActionResult Index() => RedirectToAction("ViewCustomerData");

        public ActionResult EditCustomer(int customerID)
            => View(new EditCustomerViewModel(customerID));

        [HttpPost]
        public ActionResult EditCustomer(int? customerID, int? id, bool country) {
            //TODO: have both cityID and countryID in the parameters
            var vm = country
                ? new EditCustomerViewModel(customerID.Value, id.Value)
                : new EditCustomerViewModel(customerID.Value);

            return Json(new {
                Cities = vm.GetCities(),
                Countries = vm.GetCountries(),
                CityID = country ? vm.Customer.CityID : id.Value,
                vm.Customer.CountryID
            });
        }
            //=> View(country
            //        ? new EditCustomerViewModel(customerID.Value, id.Value)
            //        : new EditCustomerViewModel(customerID.Value)
            //);

        [HttpPost]
        public ActionResult FinishCustomerEdit(EditCustomerViewModel myViewModel) {
            myViewModel.UpdateCustomer();

            return RedirectToAction("ViewCustomerData");
        }

        public ActionResult ViewCustomerData() => View(new ViewCustomersViewModel());

        [HttpPost]
        public ActionResult ViewCustomerData(ViewCustomersViewModel viewModel) => View(viewModel);

        public ActionResult ViewBills(int customerID) => View(new ViewBillsViewModel() { CustomerID = customerID });
        [HttpPost]
        public ActionResult ViewBills(ViewBillsViewModel viewModel) => View(viewModel);

        public ActionResult ViewItems(int billID) => View(new ViewItemsViewModel(billID));

        public ActionResult LoadCustomers() => PartialView("_CustomerTablePartial");
    }
}