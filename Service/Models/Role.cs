using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
