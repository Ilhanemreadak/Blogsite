using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class InPressVisitorMap : IEntityTypeConfiguration<InPressVisitor>
    {
        public void Configure(EntityTypeBuilder<InPressVisitor> builder)
        {
            builder.HasKey(ipv => new { ipv.InPressId, ipv.VisitorId });

            builder.HasOne(ipv => ipv.InPress)
                .WithMany(ip => ip.InPressVisitors)
                .HasForeignKey(ipv => ipv.InPressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ipv => ipv.Visitor)
                .WithMany(v => v.InPressVisitors)
                .HasForeignKey(ipv => ipv.VisitorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new InPressVisitor
                {
                    InPressId = Guid.Parse("2EB7FC6A-6194-4A5F-807A-A98A32AFAD32"),
                    VisitorId = 2
                }
            );

        }
    }
}
