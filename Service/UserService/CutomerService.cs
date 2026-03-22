using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.DTO;
using Repository.Helper;
using Repository.User;

namespace Service.UserService
{
    public class CutomerService
    {
        private CustomerRepo repo;

        public CutomerService()
        {
            repo = new CustomerRepo();
        }

        public List<CustomerDTO> ViewCustomer()
        {
            return repo.GetAll();
        }

        public List<CustomerDTO> SearchCustomer(string name, int id)
        {
            List<CustomerDTO> list = new List<CustomerDTO> ();
            if (id > 0)
            {
                var p = repo.SearchCustomerID(id);
                list.Add(p);
                return list;
            }
            if (name != null)
            {
                var listCustomer = repo.SearchByName(name);
                return listCustomer; 
            }
            return null;
        }

        public bool RemoveCutomer(int id)
        {
            if(id == null) return false ;
            repo.DeleteCustomer(id);
            return true;
            

        }

        public void AddCutomer(CustomerDTO customer)
        {
            if(customer == null) return;
            repo.CreateCustomer(TransferModelsDTO.MappCustomer(customer));
        }

        public void UpdateCutomer(CustomerDTO customer)
        {
            if(customer == null) return;
            repo.UpdateCustomer(TransferModelsDTO.MappCustomer(customer));
        }


    }
}
