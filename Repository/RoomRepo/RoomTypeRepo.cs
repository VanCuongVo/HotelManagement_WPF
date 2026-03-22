using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Repository.DTO;
using Repository.Helper;

namespace Repository.RoomRepo
{
    public class RoomTypeRepo
    {
        private FuminiHotelManagementContext context;

        public RoomTypeRepo()
        {
            context = new FuminiHotelManagementContext();
        }

            

        public List<RoomTypeDTO> GetAll()
        {
            List<RoomTypeDTO> room = new List<RoomTypeDTO>();
            var list = context.RoomTypes.ToList();
            if (list != null)
            {
                foreach (var roomType in list)
                {
                    room.Add(TransferModelsDTO.MappRoomTypeDTO(roomType));
                }          
            }
            return room;
        }

        public void UpdateRoomType(RoomType roomType)
        {
            var p = context.RoomTypes.FirstOrDefault(x => x.RoomTypeId == roomType.RoomTypeId);
                p.RoomTypeName = roomType.RoomTypeName;
                p.TypeDescription = roomType.TypeDescription;
                p.TypeNote = roomType.TypeNote;
                context.SaveChanges();

        }


        public void DeleteRoomType(int roomTypeId)
        {
            var room = context.RoomTypes.FirstOrDefault(x => x.RoomTypeId == roomTypeId);
            context.RoomTypes.Remove(room);
            context.SaveChanges();
        }

        public void CreateRoomType(RoomType roomType)
        {
            context.RoomTypes.Add(roomType);
            context.SaveChanges();
        }


        public RoomTypeDTO SearchById(int roomTypeId)
        {
            RoomType roomType = new();
            RoomType room = context.RoomTypes.FirstOrDefault(x => x.RoomTypeId == roomTypeId);
            return TransferModelsDTO.MappRoomTypeDTO(room);
        }


        public List<RoomTypeDTO> SearchByName(string name)
        {

            List<RoomTypeDTO> cus = new List<RoomTypeDTO>();
            foreach (var item in GetAll())
            {
                if(item.RoomTypeName.Contains(name))
                {
                    cus.Add(item);
                }

            }
            return cus;
        }



    }
}
