using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Repository.DTO;
using Repository.Helper;
using DataAccess;


namespace Repository.User
{
    public  class AccountRepo
    {
        private FuminiHotelManagementContext context;

        public AccountRepo()
        {
            context = new FuminiHotelManagementContext();
        }

        public CustomerDTO LoginWithCustomer(string email, string password)
        {
            var p = context.Customers.FirstOrDefault(x => x.EmailAddress == email && x.Password == password);
            if (p == null)
            {
                return null;
            }
            else
            {
                return TransferModelsDTO.MappCustomerDTO(p);
            }
        }



        public bool AddCustomer(Customer customer)
        {
            var p = context.Customers.FirstOrDefault(x => x.EmailAddress.Equals(customer.EmailAddress));
            if (p != null)
            {
                return false;
            }
            else
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return true;
            }
        }



        public AdminDTO LoginWithAdmin(string email, string password)
        {

            var p = context.Admins.FirstOrDefault(x => x.Email == email && x.Password == password);
            if (p != null)
            {
                return TransferModelsDTO.MappAdmin(p);
            }
            else
            {
                return null;
            }
        }     
    }
}
