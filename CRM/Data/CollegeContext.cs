using CRM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Data
{
    public class CollegeContext : DbContext
    {
        public CollegeContext(DbContextOptions<CollegeContext> options)
            : base(options)
        {
        }
        public DbSet<MstClass> MstClass { get; set; }

        public DbSet<MstCourse> MstCourse { get; set; }
        public DbSet<Student> Student { get; set; }

    }
}
