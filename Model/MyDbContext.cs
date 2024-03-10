using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamifyX.Model
{
	//the setup for the database
	public class MyDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Exam> Exams { get; set; }
		public DbSet<Question> Questions { get; set; }

		public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ExamifyXDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.HasIndex(u => u.Username)
				.IsUnique();

			modelBuilder.Entity<User>()
				.HasIndex(e => e.Email)
				.IsUnique();

			modelBuilder.Entity<Exam>()
			.HasMany(e => e.Questions)
			.WithOne(q => q.Exam)
			.HasForeignKey(q => q.ExamId)
			.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
