using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Account_Loging.DAL.Model;

#nullable disable

namespace AccountLog.DAL.DbContainer
{
    public partial class AccountLogContext : DbContext
    {
        public AccountLogContext()
        {
        }

        public AccountLogContext(DbContextOptions<AccountLogContext> options)
            : base(options)
        {
        }

         
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
 
        }
  
    }
}
