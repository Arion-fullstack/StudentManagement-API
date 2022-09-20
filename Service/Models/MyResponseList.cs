using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class MyResponseList<T>
    {
        public string Message { get; set; }
        public int Code { get; set; }
        public List<T>? Data { get; set; }
        public Paginate? Paginate { get; set; }
    }
}
