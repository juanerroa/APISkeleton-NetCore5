using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options){}

        public DbSet<Auth.Module> Modules {get; set;}
        public DbSet<Auth.Privilege> Privileges {get; set;}
        public DbSet<Auth.Role> Roles {get; set;}
        public DbSet<Auth.RolePrivilege> RolesPrivileges {get; set;}
        public DbSet<Auth.User> Users {get; set;}
        public DbSet<Auth.UserRole> UsersRoles {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auth.RolePrivilege>(opc => opc.HasIndex(c=> new {c.RoleId, c.PrivilegeId}).IsUnique());

            modelBuilder.Entity<Auth.UserRole>(opc => opc.HasIndex(c=> new {c.UserId, c.RoleId}).IsUnique());
        }
    }
}