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
    public class RoomRepo
    {
        private FuminiHotelManagementContext _context;
        public RoomRepo()
        {
            _context = new FuminiHotelManagementContext();
        }



        public List<RoomInformationDTO> GetAll()
        {
            var roomList = new List<RoomInformationDTO>();
            var list = _context.RoomInformations.ToList();

            foreach (var roomInformation in list)
            {
                roomList.Add(TransferModelsDTO.MapRoomDTO(roomInformation));
            }

            return roomList;
        }

        public void UpdateRoom(RoomInformation room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room), "Room cannot be null.");

            var existingRoom = _context.RoomInformations.FirstOrDefault(x => x.RoomId == room.RoomId);
            if (existingRoom == null)
                throw new Exception($"Room with ID {room.RoomId} not found.");

            try
            {
                existingRoom.RoomNumber = room.RoomNumber;
                existingRoom.RoomDetailDescription = room.RoomDetailDescription;
                existingRoom.RoomMaxCapacity = room.RoomMaxCapacity;
                existingRoom.RoomStatus = room.RoomStatus;
                existingRoom.RoomPricePerDay = room.RoomPricePerDay;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc throw lại nếu cần
                throw new Exception("An error occurred while updating the room.", ex);
            }
        }


        public void DeleteRoom(int roomId)
        {
            var room = _context.RoomInformations.FirstOrDefault(x => x.RoomId == roomId);
            _context.RoomInformations.Remove(room);
            _context.SaveChanges();
        }

        public void CreateRoom(RoomInformation room)
        {
            if (room == null)
                throw new ArgumentNullException(nameof(room), "Room cannot be null.");

            try
            {
                _context.RoomInformations.Add(room);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc throw lại nếu cần
                throw new Exception("An error occurred while creating the room.", ex);
            }
        }

        public RoomInformationDTO SearchById(int roomId)
        {

            RoomInformation room = _context.RoomInformations.FirstOrDefault(x => x.RoomId == roomId);
            if (room == null)
            {
                return null;
            }
            return TransferModelsDTO.MapRoomDTO(room);
        }


        public List<RoomInformationDTO> SearchByRoomNumber(string roomNumber)
        {

            List<RoomInformationDTO> name = new List<RoomInformationDTO>();
            foreach (var item in GetAll())
            {
                if (item.RoomNumber.Contains(roomNumber))
                {
                    name.Add(item);
                }

            }
            return name;
        }
        public List<RoomInformationDTO> SearchByRoomStatus(byte roomStatus)
        {
            List<RoomInformationDTO> status = new List<RoomInformationDTO>();
            foreach (var item in GetAll())
            {
                if (item.RoomStatus == roomStatus)
                {
                    status.Add(item);
                }
            }
            return status;
        }

    }


}
