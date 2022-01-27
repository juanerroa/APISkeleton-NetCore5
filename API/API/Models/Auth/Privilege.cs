using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Auth
{
    public class Privilege
    {
        public Privilege(){
            Roles = new HashSet<RolePrivilege>();
        }
        public int Id {get; set;}
        public String Name {get; set;}
        public int ModuleId {get; set;}

        public Module Module {get; set;}
        public ICollection<RolePrivilege> Roles {get; set;}
    }
}