using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AstinIt
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Hometown { get; set; }
        public bool Active { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class UserDto : IDBObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }

    public class UserContext : IdentityDbContext<ApplicationUser>
    {
        public UserContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager
                    .ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    SqlCommand command =
                        new SqlCommand(
                            string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE",
                                Database.Connection.Database), connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
            }

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UserContext, Configuration>());

            try
            {
                using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager
                    .ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    SqlCommand command =
                        new SqlCommand(
                            string.Format("ALTER DATABASE [{0}] SET MULTI_USER WITH ROLLBACK IMMEDIATE",
                                Database.Connection.Database), connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException)
            {
            }
        }

        public static UserContext Create()
        {
            var context = new UserContext();
            return context;
        }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<ProjectCost>()
                .HasRequired<Project>(p => p.Project)
                .WithMany(c => c.ProjectCost)
                .HasForeignKey<Guid>(c => c.Id);      */  
            
            base.OnModelCreating(modelBuilder);
        }


        public void Add(IDBObject model)
        {
            Entry(model).State = EntityState.Added;
        }

        public void Update(IDBObject model)
        {
            Entry(model).State = EntityState.Modified;
        }


        public DbSet<Country> Country { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectCost> ProjectCost { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Info> Info { get; set; }
        public DbSet<PageData> PageData { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Vacancy> Vacancy { get; set; }
        public DbSet<UserTask> UserTask { get; set; }
    }
}