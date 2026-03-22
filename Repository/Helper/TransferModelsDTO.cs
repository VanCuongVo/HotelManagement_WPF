
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Repository.DTO;

namespace Repository.Helper
{
    public class TransferModelsDTO
    {
        public static CustomerDTO MappCustomerDTO(Customer cus)
        {
            CustomerDTO customerDTO = new CustomerDTO();
            customerDTO.CustomerId = cus.CustomerId;
            customerDTO.CustomerFullName = cus.CustomerFullName;
            customerDTO.CustomerBirthday = cus.CustomerBirthday;
            customerDTO.EmailAddress = cus.EmailAddress;
            customerDTO.Telephone = cus.Telephone;
            customerDTO.CustomerStatus = cus.CustomerStatus;
            customerDTO.Password = cus.Password;
            return customerDTO;
        }

        public static Customer MappCustomer(CustomerDTO customerDTO)
        {
            Customer customer = new Customer();
            customer.CustomerId = customerDTO.CustomerId;
            customer.CustomerFullName = customerDTO.CustomerFullName;
            customer.CustomerBirthday = customerDTO.CustomerBirthday;
            customer.EmailAddress = customerDTO.EmailAddress;
            customer.Telephone = customerDTO.Telephone;
            customer.CustomerStatus = customerDTO.CustomerStatus;
            customer.Password = customerDTO.Password;
            return customer;

        }

        public static AdminDTO MappAdmin(Admin admin)
        {
            AdminDTO adminDTO = new AdminDTO();
            adminDTO.Email = admin.Email;
            adminDTO.Password = admin.Password;
            adminDTO.RoleId = admin.RoleId;
            return adminDTO;
        }


        public static RoomInformation MapRoom(RoomInformationDTO dto)
        {
            return new RoomInformation
            {
                RoomId = dto.RoomId,
                RoomNumber = dto.RoomNumber,
                RoomDetailDescription = dto.RoomDetailDescription,
                RoomPricePerDay = dto.RoomPricePerDay,
                RoomTypeId = dto.RoomTypeId,
                RoomMaxCapacity = dto.RoomMaxCapacity,
                RoomStatus = dto.RoomStatus
            };
        }
        public static RoomInformationDTO MapRoomDTO(RoomInformation entity)
        {
            return new RoomInformationDTO
            {
                RoomId = entity.RoomId,
                RoomNumber = entity.RoomNumber,
                RoomDetailDescription = entity.RoomDetailDescription,
                RoomPricePerDay = entity.RoomPricePerDay,
                RoomTypeId = entity.RoomTypeId,
                RoomMaxCapacity = entity.RoomMaxCapacity,
                RoomStatus = entity.RoomStatus
            };
        }

        public static RoomTypeDTO MappRoomTypeDTO(RoomType roomType)
        {
            RoomTypeDTO roomTypeDTO = new RoomTypeDTO();
            roomTypeDTO.RoomTypeName = roomType.RoomTypeName;
            roomTypeDTO.RoomTypeId = roomType.RoomTypeId;
            roomTypeDTO.TypeNote = roomType.TypeNote;
            roomTypeDTO.TypeDescription = roomType.TypeDescription;
            return roomTypeDTO;
        }

        public static RoomType MappRoomType(RoomTypeDTO roomTypeDTO)
        {
            RoomType roomType = new RoomType();
            roomType.RoomTypeName = roomTypeDTO.RoomTypeName;
            roomType.RoomTypeId = roomTypeDTO.RoomTypeId;
            roomType.TypeDescription = roomTypeDTO.TypeDescription;
            roomType.TypeNote = roomTypeDTO.TypeNote;
            return roomType;
        }


    }
}
