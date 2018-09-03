using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentDataService.Model
{
    public class User
    {
        public long UserId { get; set; } 
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string DOB { get; set; }
        public string PhoneNumber { get; set; }
        public string HAddress { get; set; }
        public string EmailAddress { get; set; }

    }
}