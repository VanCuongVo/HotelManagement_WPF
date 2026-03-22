using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Repository.DTO;
using Repository.Helper;
using Repository.RoomRepo;

namespace Service.RoomService
{
    public class RoomTypeService
    {
        private RoomTypeRepo roomtypeRepo;

        public RoomTypeService()
        {
            roomtypeRepo = new RoomTypeRepo();
        }

        public List<RoomTypeDTO> ViewRoomType()
        {
            return roomtypeRepo.GetAll();
        }

        public List<RoomTypeDTO> SearchRoomType(string name, int id)
        {
            List<RoomTypeDTO> list = new List<RoomTypeDTO>();
            if (id > 0)
            {
                var p = roomtypeRepo.SearchById(id);
                list.Add(p);
                return list;
            }
            if (name != null)
            {
                var listCustomer = roomtypeRepo.SearchByName(name);
                return listCustomer;
            }
            return null;
        }

        public bool RemoveRoomType(int id)
        {
            if (id == null) return false;
            roomtypeRepo.DeleteRoomType(id);
            return true;


        }

        public void AddRoomType(RoomTypeDTO room)
        {
            if (room == null) return;
            roomtypeRepo.CreateRoomType(TransferModelsDTO.MappRoomType(room));
        }

        public void UpdateRoomType(RoomType room)
        {
            if (room == null) return;
            roomtypeRepo.UpdateRoomType(room);
        }

    }
}
