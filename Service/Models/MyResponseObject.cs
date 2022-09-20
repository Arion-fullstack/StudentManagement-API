using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class MyResponseObject<T>
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public T Data { get; set; }
    }
}
