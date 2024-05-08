using Assesmentpaksod.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Assesmentpaksod.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options, DbSet<ResultAnswer> answers, DbSet<ResultCreator> creators) : base() {  
       
            Answers = answers;
            Creators = creators;
        }
        public DbSet<ResultAnswer> Answers { get; set; }
        public DbSet<ResultSubject> Subjects { get; set; }
        public DbSet<ResultCreator> Creators { get; set; }
    }
}