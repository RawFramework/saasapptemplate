using demoApp.Models.DBModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demoApp.Repositories
{
    public class EFContext : DbContext
    {
        public static string ConnectionString { get; set; }
        public DbSet<Course> Course { get; set; }

        public EFContext()
        {
        }
        public EFContext(DbContextOptions options) : base(options)
        {
            
        }



        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(EFContext.ConnectionString);
        }
    }
}
