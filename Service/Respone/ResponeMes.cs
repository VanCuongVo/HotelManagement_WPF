using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Respone
{
    public class ResponeMes<T>    
    {
        public T Value { get; set; }
        public bool Isuccess { get; set; }

    }
}
