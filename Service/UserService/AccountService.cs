using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using Repository.DTO;
using Repository.User;
using Service.Respone;

namespace Service.UserService
{
    public class AccountService
    {
        private AccountRepo repo;

        public AccountService()
        {
            repo = new AccountRepo();
        }

        public ResponeMes<CustomerDTO> Login(string email, string password)
        {
            ResponeMes<CustomerDTO> cus = new ResponeMes<CustomerDTO>();
            if (email.IsNullOrEmpty() && password.IsNullOrEmpty())
            {
                cus.Isuccess = false;

            }
            else
            {
                var p = repo.LoginWithCustomer(email, password);
                if (p != null)
                {
                    cus.Value = p;
                    cus.Isuccess = true;
                }
                else
                {
                    cus.Isuccess = false;
                }

            }
            return cus;

        }



        public bool AddCustomer(Customer customer)
        {
            if (customer.EmailAddress.Length < 8 || customer.Password.Length < 2)
            {
                return false;

            }
            else
            {
                repo.AddCustomer(customer);    
                return true;
            }
        }


        public ResponeMes<AdminDTO> LoginWithAdmin(string email, string password)
        {
            ResponeMes<AdminDTO> ad = new ResponeMes<AdminDTO>();//cái phễu
            if (email.IsNullOrEmpty() && password.IsNullOrEmpty())
            {
                ad.Isuccess = false;

            }
            else
            {
                var p = repo.LoginWithAdmin(email, password);
                if (p != null)
                {
                    ad.Value = p;
                    ad.Isuccess = true;
                }
                else
                {
                    ad.Isuccess = false;
                }
            }
            return ad;

        }








    }
}
