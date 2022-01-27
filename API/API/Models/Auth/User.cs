using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Auth
{
    public class User
    {
        public User(){
            Roles = new HashSet<Role>();
        }

        public int Id {get; set;}
        public  String UserName {get; set;}
        public String Password {get; set;}
        public String Email {get; set;}

        public ICollection<Role> Roles {get; set;}
    }
}