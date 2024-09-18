using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class EducationalVisitorMap : IEntityTypeConfiguration<EducationalVisitor>
    {
        public void Configure(EntityTypeBuilder<EducationalVisitor> builder)
        {
            builder.HasKey(ev => new { ev.EducationalId, ev.VisitorId });

            builder.HasOne(ev => ev.Educational)
                .WithMany(e => e.EducationalVisitors)
                .HasForeignKey(ev => ev.EducationalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ev => ev.Visitor)
                .WithMany(v => v.EducationalVisitors)
                .HasForeignKey(ev => ev.VisitorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new EducationalVisitor
                {
                    EducationalId = Guid.Parse("A6AB4D04-FBD8-4F87-88B8-C9CFA86C516F"),
                    VisitorId = 1
                }
            );
        }
    }
}
