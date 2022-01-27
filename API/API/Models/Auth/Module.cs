using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Auth
{
    public class Module
    {
        public Module(){
            Privileges = new HashSet<Privilege>();
        }
        public int Id {get; set;}
        public int Name {get; set;}

        ICollection<Privilege> Privileges {get; set;}
    }
}