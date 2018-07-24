using System.Web.Mvc;

using MVCProject.BLL;

namespace MVCProject.WebClient.Controllers
{
    public class AdventureWorksController : Controller
    {
        public ActionResult Index() => RedirectToAction("ViewCustomerData");

        #region Edit & Delete

        [Authorize(Roles = "Administrator")]
        public ActionResult EditCustomer(int customerID)
            => View(new EditCustomerViewModel(customerID));

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult PerformEdit(CustomerViewModel customer)
        {
            var viewModel = new EditCustomerViewModel()
            {
                Customer = customer
            };

            return Json(new
            {
                Success = ModelState.IsValid && viewModel.UpdateCustomer(),
                DisplayName = $"{customer}"
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult UpdateCityAndCountryDDLs(int customerID, int cityID, int countryID, bool country)
        {
            var vm = new EditCustomerViewModel(customerID, countryID);

            return Json(new
            {
                Cities = vm.GetCities(),
                Countries = vm.GetCountries(),
                CityID = country ? vm.Customer.CityID : cityID,
                vm.Customer.CountryID
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult UpdateCustomerDropDown(int customerID)
        {
            var vm = new EditCustomerViewModel(customerID);

            return Json(new
            {
                vm.Customer,
                Cities = vm.GetCities(),
                Countries = vm.GetCountries()
            });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult FinishCustomerEdit(EditCustomerViewModel myViewModel)
        {
            myViewModel.UpdateCustomer();

            return RedirectToAction("ViewCustomerData");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteCustomer(int customerID)
        {
            if (Helpers.DeleteCustomer(customerID))
                return Json(new
                {
                    Success = true,
                    RedirectUrl = Utils.UrlHelper.Action(
                        actionName: "ViewCustomerData",
                        controllerName: "AdventureWorks"
                    )
                });
            else
                return Json(new
                {
                    Success = false
                });
        }

        #endregion

        #region Customer Data

        public ActionResult ViewCustomerData(int? page)
        {
            var viewModel = new ViewCustomersViewModel();

            if (page.HasValue)
                viewModel.Customers.CurrentPage = page.Value;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ViewCustomerData(ViewCustomersViewModel viewModel) => View(viewModel);

        [HttpPost]
        public ActionResult PageCustomerData(int? cityID, int? countryID, int? page)
        {
            var viewModel = new ViewCustomersViewModel()
            {
                CityID = cityID.Value,
                CountryID = countryID.Value
            };

            viewModel.Customers.CurrentPage = page.Value;

            return View(viewModel);
        }

        #endregion

        #region Bills

        public ActionResult ViewBills(int customerID, int? page, int? sort, bool? ascendingSort)
        {
            var viewModel = new ViewBillsViewModel(page ?? 1)
            {
                CustomerID = customerID
            };

            if (sort.HasValue)
                viewModel.Sort = (BillSort)sort;
            if (ascendingSort.HasValue)
                viewModel.AscendingSort = ascendingSort.Value;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ViewBills(ViewBillsViewModel viewModel) => View(viewModel);

        #endregion

        public ActionResult ViewItems(int customerID, int billID, int? page)
            => View(new ViewItemsViewModel(customerID, billID, page ?? 1));

        #region Checks

        public ActionResult CheckBills(int customerID)
        {
            var viewModel = new ViewBillsViewModel() { CustomerID = customerID };

            if (viewModel.Bills.Count != 0)
                return Json(new
                {
                    Error = false,
                    RedirectUrl = Utils.UrlHelper.Action(
                        controllerName: "AdventureWorks",
                        actionName: "ViewBills",
                        routeValues: new { customerID }
                    )
                });
            else
                return Json(new
                {
                    Error = true,
                    Title = "No bills found",
                    Body = "The customer you selected has no bills"
                });
        }

        public ActionResult CheckItems(int customerID, int billID)
        {
            var viewModel = new ViewItemsViewModel(customerID, billID, 1);

            if (viewModel.Items.Count != 0)
                return Json(new
                {
                    Error = false,
                    RedirectUrl = Utils.UrlHelper.Action(
                        controllerName: "AdventureWorks",
                        actionName: "ViewItems",
                        routeValues: new { customerID, billID }
                    )
                });
            else
                return Json(new
                {
                    Error = true,
                    Title = "No items found",
                    Body = "The bill you selected has no items"
                });
        }

        #endregion
    }
}