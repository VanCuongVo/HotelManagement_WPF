using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.DTO;
using Repository.RoomRepo;

namespace Service.RoomService
{
    public class BookingDetailService
    {
        private BookingDetailRepo bookingDetailRepo;

        public BookingDetailService()
        {
            bookingDetailRepo = new BookingDetailRepo();
        }

        public List<BookingDetailDTO> GetBookingDetailsReport()
        {
            return bookingDetailRepo.BookingDetailsReport();
        }


    }
}