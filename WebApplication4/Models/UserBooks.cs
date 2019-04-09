using System;
using System.Collections.Generic;

namespace WebApplication4.Models
{
    public partial class UserBooks
    {
        public int Id { get; set; }
        public int Usersid { get; set; }
        public int Booksid { get; set; }
        public DateTime Date { get; set; }
    }
}
