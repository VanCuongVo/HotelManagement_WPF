using Repository.UserRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.UserService
{
    public class BookingReservationService
    {
        private BookingReservationRepo repo;

        public BookingReservationService()
        {
            repo = new BookingReservationRepo();
        }

        public List<DataAccess.Models.BookingReservation> GetBookingReservationsByCustomerId(int customerId)
        {
            return repo.GetBookingReservationsByCustomerId(customerId);

        }
    }
}
