using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO
{
    public  class RoomTypeDTO
    {
        public int RoomTypeId { get; set; }

        public string RoomTypeName { get; set; } = null!;

        public string? TypeDescription { get; set; }

        public string? TypeNote { get; set; }

    }
}
