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
    public class EducationalVisitorMap : IEntityTypeConfiguration<EducationalVisitor>
    {
        public void Configure(EntityTypeBuilder<EducationalVisitor> builder)
        {
            builder.HasKey(x => new { x.EducationalId, x.VisitorId });
        }
    }
}
