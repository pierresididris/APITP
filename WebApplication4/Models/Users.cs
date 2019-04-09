using System;
using System.Collections.Generic;

namespace WebApplication4.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Login { get; set; }
        public string Pwd { get; set; }
        public string Role { get; set; }
    }
}
