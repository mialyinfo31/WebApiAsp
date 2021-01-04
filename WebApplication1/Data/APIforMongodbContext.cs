using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIforMongodb.Models;

namespace APIforMongodb.Data
{
    public class APIforMongodbContext : DbContext
    {
        public APIforMongodbContext (DbContextOptions<APIforMongodbContext> options)
            : base(options)
        {
        }

        public DbSet<APIforMongodb.Models.Book> Book { get; set; }
    }
}
