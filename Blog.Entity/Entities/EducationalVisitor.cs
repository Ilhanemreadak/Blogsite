using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entity.Entities
{
    public class EducationalVisitor : IEntityBase
    {
        public EducationalVisitor()
        {

        }
        public EducationalVisitor(Guid educationalId, int visitorId)
        {
            EducationalId = educationalId;
            VisitorId = visitorId;
        }

        public Guid EducationalId { get; set; }
        public Educational Educational { get; set; }
        public int VisitorId { get; set; }
        public Visitor Visitor { get; set; }

    }
}
