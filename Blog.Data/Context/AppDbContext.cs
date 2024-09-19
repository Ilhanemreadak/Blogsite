using Blog.Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Blog.Data.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        protected AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }    
        public DbSet<Educational> Educationals { get; set; }    
        public DbSet<InPress> InPresses { get; set; }    
        public DbSet<Category> Category { get;  set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<About> About { get; set; }
        public DbSet<SocialMedia> SocialMedia { get; set; }
        public DbSet<ContactMessages> Messages { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<ArticleVisitor> ArticleVisitors { get; set; }
        public DbSet<EducationalVisitor> EducationalVisitors{ get; set; }
        public DbSet<InPressVisitor> InPressVisitors{ get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
