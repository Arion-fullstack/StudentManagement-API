using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models
{
    public class UserResponse
    {
        public string? Token {get; set;}
        public string? RefreshToken { get; set; }
        public string? Username { get; set; }
    }
}
