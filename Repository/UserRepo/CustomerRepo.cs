using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using Repository.DTO;
using Repository.Helper;

namespace Repository.User
{
    public class CustomerRepo
    {

        private FuminiHotelManagementContext context;

        public CustomerRepo()
        {
            context = new FuminiHotelManagementContext();
        }

        public List<CustomerDTO> GetAll()
        {
            List<CustomerDTO> cus = new List<CustomerDTO>();
            var list = context.Customers.ToList();
            if (list != null)
            {
                foreach (var customer in list)
                {
                    cus.Add(TransferModelsDTO.MappCustomerDTO(customer));
                }
            }
            return cus;
        }

        public void UpdateCustomer(Customer customer)
        {
            var p = context.Customers.FirstOrDefault(x => x.CustomerId == customer.CustomerId);
                p.CustomerFullName = customer.CustomerFullName;
                p.EmailAddress = customer.EmailAddress;
                p.Telephone = customer.Telephone;
                p.CustomerStatus = customer.CustomerStatus;
                p.CustomerBirthday = customer.CustomerBirthday;
                context.SaveChanges();
        }


        public void CreateCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            var p = context.Customers.FirstOrDefault(x => x.CustomerId == id);
            context.Customers.Remove(p);
            context.SaveChanges();
        }

        public CustomerDTO SearchCustomerID(int id)
        {
            var p = context.Customers.FirstOrDefault(x => x.CustomerId == id);
            return TransferModelsDTO.MappCustomerDTO(p);
        }

        public List<CustomerDTO> SearchByName(string name)
        {
            List<CustomerDTO> cus = new List<CustomerDTO>();
            foreach (var customer in GetAll())
            {
                if(customer.CustomerFullName.Contains(name))
                {
                    cus.Add(customer);
                }
            }
            return cus;
        }
    }
}
