using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Prog_POE_Part2.Models;

namespace Prog_POE_Part2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LecturerTb> Lecturers { get; set; }

        public DbSet<ClaimTb> Claims { get; set; }

    }
}

//----------------------------------------------End Of File--------------------------------------------\\