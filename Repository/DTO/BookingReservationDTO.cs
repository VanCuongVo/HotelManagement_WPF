using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO
{
    public class BookingReservationDTO
    {
        public int BookingReservationId { get; set; }
        public int CustomerID { get; set; }
        public DateTime BookingDate { get; set; }

        public byte? BookingStatus { get; set; }
        public List<BookingDetailDTO> BookingDetails { get; set; } = new List<BookingDetailDTO>();
        public decimal TotalPrice { get; set; }
    }
}
