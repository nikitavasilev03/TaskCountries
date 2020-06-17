using DomainCore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DomainCore.Context
{
    public partial class CountriesDBContext : DbContext
    {
        //Строка подключения
        public string ConnectionString
        {
            get => connectionString;
            set => connectionString = value;
        }

        public CountriesDBContext(string connectString) : base()
        {
            connectionString = connectString;
        }
        //Работа с последовательностью
        public int NextSequence(string seq_name)
        {
            SqlParameter result = new SqlParameter("@result", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            Database.ExecuteSqlCommand("SELECT @result = (NEXT VALUE FOR dbo." + seq_name + ")", result);
            return (int)result.Value;
        }

    }
    
    public partial class CountriesDBContext : DbContext
    {
        private string connectionString;

        public CountriesDBContext()
        {
        }

        public CountriesDBContext(DbContextOptions<CountriesDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Region> Regions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Area).HasColumnName("area");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("code")
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);

                entity.Property(e => e.Population).HasColumnName("population");

                entity.Property(e => e.RegionId).HasColumnName("regionId");

                entity.Property(e => e.CapitalId).HasColumnName("capitalId");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Countries)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK__Countries__regio__75A278F5");

                entity.HasOne(d => d.Сapital)
                    .WithMany(p => p.Countries)
                    .HasForeignKey(d => d.CapitalId)
                    .HasConstraintName("FK__Countries__capit__74AE54BC");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255);
            });

            modelBuilder.HasSequence("SEQ_Cities");

            modelBuilder.HasSequence("SEQ_Countries");

            modelBuilder.HasSequence("SEQ_Regions");

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
