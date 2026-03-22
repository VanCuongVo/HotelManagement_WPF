using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO
{
    public class BookingDetailDTO
    {
        public int BookingReservationId { get; set; }

        public int RoomId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public double ActualPrice { get; set; }
        public DateTime? BookingDate { get; internal set; }
        public string RoomNumber { get; internal set; }
    }
}