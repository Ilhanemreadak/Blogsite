using Blog.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Data.Mappings
{
    public class EducationalMap : IEntityTypeConfiguration<Educational>
    {
        public void Configure(EntityTypeBuilder<Educational> builder)
        {
            builder.ToTable("Educationals");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(e => e.Content)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.ViewCount)
                .HasDefaultValue(0);

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(e => e.Category)
                .WithMany(c => c.Educationals)
                .HasForeignKey(e => e.CategoryId);

            builder.HasOne(e => e.User)
                .WithMany(u => u.Educationals)
                .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.Image)
                .WithMany()
                .HasForeignKey(e => e.ImageId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(e => e.EducationalVisitors)
                .WithOne(ev => ev.Educational)
                .HasForeignKey(ev => ev.EducationalId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Educational
                {
                    Id = Guid.Parse("A6AB4D04-FBD8-4F87-88B8-C9CFA86C516F"),
                    Title = "Introduction to C#",
                    Content = "This is a beginner guide to learning C# programming language...",
                    UserId = Guid.Parse("455ADDEB-5E52-46CC-CFC8-08DCCCB5E0D3"),
                    CategoryId = Guid.Parse("A8060387-B90E-4C87-A572-D551B3E79D21"),
                    ImageId = Guid.Parse("DA9861C7-40AA-41EB-897B-15D36F27183D"),
                    CreatedBy = "Admin",
                    CreatedDate = DateTime.Now,
                    ViewCount = 100
                }
            );

        }
    }
}
