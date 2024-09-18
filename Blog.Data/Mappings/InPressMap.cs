using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class InPressMap : IEntityTypeConfiguration<InPress>
    {
        public void Configure(EntityTypeBuilder<InPress> builder)
        {
            builder.ToTable("InPress");

            builder.HasKey(ip => ip.Id);

            builder.Property(ip => ip.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(ip => ip.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(ip => ip.ViewCount)
                .HasDefaultValue(0);

            builder.Property(ip => ip.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(ip => ip.Category)
                .WithMany(c => c.InPresses)
                .HasForeignKey(ip => ip.CategoryId);

            builder.HasOne(ip => ip.User)
                .WithMany(u => u.InPresses)
                .HasForeignKey(ip => ip.UserId);

            builder.HasOne(ip => ip.Image)
                .WithMany()
                .HasForeignKey(ip => ip.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(ip => ip.InPressVisitors)
                .WithOne(iv => iv.InPress)
                .HasForeignKey(iv => iv.InPressId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new InPress
                {
                    Id = Guid.Parse("2EB7FC6A-6194-4A5F-807A-A98A32AFAD32"),
                    Title = "Understanding Media Coverage",
                    Content = "An in-depth look into how media coverage influences public perception...",
                    UserId = Guid.Parse("455ADDEB-5E52-46CC-CFC8-08DCCCB5E0D3"),
                    CategoryId = Guid.Parse("A8060387-B90E-4C87-A572-D551B3E79D21"),
                    ImageId = Guid.Parse("DA9861C7-40AA-41EB-897B-15D36F27183D"),
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    ViewCount = 50
                }
            );
        }
    }
}
