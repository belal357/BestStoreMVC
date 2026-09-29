using BestStoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BestStoreMVC.Services
{
    public class ApplicationDbContext : DbContext
    {//this class extents Dbcontext (class of entity frame work )
        /* public ApplicationDbContexet()
         {    } */

        //This constructor is commonly used with ( Dependency Injection ) in ASP.NET Core.>> It allows ASP.NET Core to provide the database configuration, including the connection string, to your ApplicationDbContext.
        //receives the configuration/options for Entity Framework Core >> and passes them to the parent DbContext class.
        //                                                 // Send these options to the parent DbContext constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //basically tells Entity Framework Core: >>>> 
        //"Here are the settings you need to connect and work with my database."

        public DbSet<Product> Products { get; set; }


    }



}
