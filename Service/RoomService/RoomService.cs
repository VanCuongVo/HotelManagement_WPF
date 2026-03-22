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
    public class RoomService
    {
        private RoomRepo roomRepo;
        public RoomService()
        {
            roomRepo = new RoomRepo();
        }

        public List<RoomInformationDTO> ViewRoom()
        {
            return roomRepo.GetAll();
        }


        public List<RoomInformationDTO> SearchRoom(string? roomNumber, byte? roomStatus)
        {
            var rooms = roomRepo.GetAll();

            if (!string.IsNullOrEmpty(roomNumber))
            {
                rooms = rooms.Where(r => r.RoomNumber.Contains(roomNumber)).ToList();
            }

            if (roomStatus.HasValue)
            {
                rooms = rooms.Where(r => r.RoomStatus == roomStatus.Value).ToList();
            }

            return rooms;
        }



        public bool RemoveRoom(int id)
        {
            if (id == null) return false;
            roomRepo.DeleteRoom(id);
            return true;


        }

        public bool AddRoom(RoomInformationDTO room)
        {
            if (room == null) return false;

            try
            {
                var entity = TransferModelsDTO.MapRoom(room);
                roomRepo.CreateRoom(entity);
                return true;
            }
            catch (Exception ex)
            {
                // Log exception nếu có hệ thống logging
                return false;
            }
        }

        public bool UpdateRoom(RoomInformationDTO roomDTO)
        {
            if (roomDTO == null) return false;

            try
            {
                var room = TransferModelsDTO.MapRoom(roomDTO);
                roomRepo.UpdateRoom(room);
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log nếu có logging
                return false;
            }
        }

    }
}
