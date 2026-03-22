using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DTO
{
    public class AdminDTO
    {

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int? RoleId { get; set; }

    }
}
