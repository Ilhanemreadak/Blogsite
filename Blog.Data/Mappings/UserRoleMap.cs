using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            // Primary key
            builder.HasKey(r => new { r.UserId, r.RoleId });

            // Maps to the AspNetUserRoles table
            builder.ToTable("AspNetUserRoles");

            builder.HasData(new AppUserRole
            {
                UserId = Guid.Parse("C118D34E-5077-4F02-8C68-6BFBC61A4DF0"),
                RoleId = Guid.Parse("14269F97-E34F-44F0-8F09-165E17058617")
            },
            new AppUserRole
            {
                UserId = Guid.Parse("20978C52-0503-4800-A4B5-68927E3D1EE0"),
                RoleId = Guid.Parse("1928A271-274D-43DE-BF56-45B195CA0E59")
            });
        }
    }
}
