﻿using ChatSystemMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatSystemMVC.ApplicationDBcontext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Friends> Friends { get; set; }
    }
}