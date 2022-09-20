using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public static implicit operator Student(MyResponseObject<Student> v)
        {
            throw new NotImplementedException();
        }

        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(new Student
            {
                //Uid = "admin",
                //Username = "Bin đẹp troai",
                //Password = "admin1",
                //Role = "Admin"
            });
        }
    }
}
