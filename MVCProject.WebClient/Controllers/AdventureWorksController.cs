using System.Web.Mvc;

using MVCProject.BLL;

namespace MVCProject.WebClient.Controllers {
    public class AdventureWorksController : Controller
    {
        public ActionResult Index() => RedirectToAction("ViewCustomerData"); //View();

        public ActionResult EditCustomer(int customerID) 
            => View(new EditCustomerViewModel(customerID));

        [HttpPost]
        public ActionResult EditCustomer()
            => RedirectToAction("ViewCustomerData");

        public ActionResult ViewCustomerData() => View(new ViewCustomersViewModel());

        [HttpPost]
        public ActionResult ViewCustomerData(ViewCustomersViewModel viewModel) => View(viewModel);

        public ActionResult ViewBills(int customerID) => View(new ViewBillsViewModel(customerID));

        public ActionResult ViewItems(int billID) => View(new ViewItemsViewModel(billID));

        public ActionResult LoadCustomers() => PartialView("_CustomerTablePartial");
    }
}