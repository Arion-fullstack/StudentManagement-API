using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class Paginate
    {
        public int TotalPage { get; set; }
        public int CurentPage { get; set; }
        public int? NextPage { get; set; }
        public int? PrePage { get; set; }
        public bool IsNextPage { get; set; }
        public bool IsPrePage { get; set; }
    }
}
