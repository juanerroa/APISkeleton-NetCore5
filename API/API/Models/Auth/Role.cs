using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Auth
{
    public class Role
    {
        public Role(){
            Users = new HashSet<User>();
            Privileges = new HashSet<Privilege>();
        }

        public int Id {get; set;}
        public String Name {get; set;}
        public int UserId {get; set;}

        public ICollection<User> Users {get; set;}
        public ICollection<Privilege> Privileges {get; set;}
    }
}