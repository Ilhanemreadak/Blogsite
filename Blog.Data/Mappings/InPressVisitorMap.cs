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
    public class InPressVisitorMap : IEntityTypeConfiguration<InPressVisitor>
    {
        public void Configure(EntityTypeBuilder<InPressVisitor> builder)
        {
            builder.HasKey(x => new { x.InPressId, x.VisitorId });
        }
    }
}
