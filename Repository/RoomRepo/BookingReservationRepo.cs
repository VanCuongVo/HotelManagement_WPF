using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using Repository.DTO;

namespace Repository.UserRepo
{
    public class BookingReservationRepo
    {
        private FuminiHotelManagementContext context;

        public BookingReservationRepo()
        {
            context = new FuminiHotelManagementContext();
        }

        public List<BookingReservation> GetBookingReservationsByCustomerId(int customerId)
        {
            return context.BookingReservations
                .Where(br => br.CustomerId == customerId)
                .ToList();
        }
    }
}
