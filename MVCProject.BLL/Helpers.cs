namespace MVCProject.BLL
{
    public static class Helpers
    {
        public static bool DeleteCustomer(int customerID)
            => DAL.Repository.DeleteCustomer(customerID);
    }
}
