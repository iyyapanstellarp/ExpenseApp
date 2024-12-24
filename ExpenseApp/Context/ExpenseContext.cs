using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ExpenseApp.Models;
namespace ExpenseApp.Context
{
    public class ExpenseContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExpenseContext() { }

        public ExpenseContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<ExpenseMasterModel> EXPexpensemaster { get; set; }

        public DbSet<UserMaster> EXPusermaster { get; set; }

        public DbSet<ExpenseGroupModel> EXPgroupmaster { get; set; }

        public DbSet<ExpenseFormModel> EXPexpense { get; set; }

        public DbSet<ExpenseMemberMasterModel> EXPmembermaster { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=StellarBilling;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Expense

            modelBuilder.Entity<ExpenseGroupModel>()
              .HasKey(i => new { i.GroupName });


            modelBuilder.Entity<UserMaster>()
                .HasKey(i => i.Username);


            modelBuilder.Entity<ExpenseMemberMasterModel>()
            .HasKey(i => new { i.Groupname, i.Membersname });


            modelBuilder.Entity<ExpenseGroupModel>()
                .HasOne<UserMaster>()
                .WithMany()
                .HasForeignKey(e => e.CreatedBy)
                .HasPrincipalKey(u => u.Username)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ExpenseMasterModel>().HasKey(i => new { i.ExpenseName });


            modelBuilder.Entity<ExpenseFormModel>()
    .HasKey(e => new { e.ExpenseName, e.GroupName });


            modelBuilder.Entity<ExpenseFormModel>()
               .HasOne<ExpenseMasterModel>()
               .WithMany()
               .HasForeignKey(e => e.ExpenseName)
               .HasPrincipalKey(u => u.ExpenseName)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpenseFormModel>()
              .HasOne<ExpenseGroupModel>()
              .WithMany()
              .HasForeignKey(e => e.GroupName)
              .HasPrincipalKey(u => u.GroupName)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExpenseFormModel>()
        .Property(e => e.Spent)
        .HasColumnType("money");

            modelBuilder.Entity<ExpenseGroupModel>()
      .Property(e => e.Budget)
      .HasColumnType("money");

            modelBuilder.Entity<ExpenseMemberMasterModel>()
     .Property(e => e.Contributionamaount)
     .HasColumnType("money");

            modelBuilder.Entity<ExpenseGroupModel>()
    .Property(e => e.GroupID)
    .ValueGeneratedOnAdd();

            modelBuilder.Entity<ExpenseMasterModel>()
   .Property(e => e.ExpenseID)
   .ValueGeneratedOnAdd();


        }
    }
}