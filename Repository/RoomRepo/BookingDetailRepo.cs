using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Repository.DTO;

namespace Repository.RoomRepo
{
    public class BookingDetailRepo
    {
        private FuminiHotelManagementContext context;

        public BookingDetailRepo()
        {
            context = new FuminiHotelManagementContext();
        }
        public List<BookingDetailDTO> BookingDetailsReport()
        {
            return context.BookingDetails
                .Include(bd => bd.BookingReservation)
                .Include(bd => bd.Room)
                .OrderByDescending(bd => bd.StartDate)
                .ThenByDescending(bd => bd.EndDate)
                .Select(bd => new BookingDetailDTO
                {
                    BookingReservationId = bd.BookingReservationId,
                    RoomId = bd.RoomId,
                    StartDate = bd.StartDate,
                    EndDate = bd.EndDate,
                    ActualPrice = bd.ActualPrice,
                    BookingDate = bd.BookingReservation.BookingDate,
                    RoomNumber = bd.Room.RoomNumber
                }).ToList();
        }
    }
}