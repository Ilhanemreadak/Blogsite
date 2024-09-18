using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entity.Entities
{
    public class InPressVisitor : IEntityBase
    {
        public InPressVisitor()
        {

        }
        public InPressVisitor(Guid inPressId, int visitorId)
        {
            InPressId = inPressId;
            VisitorId = visitorId;
        }

        public Guid InPressId { get; set; }
        public InPress InPress { get; set; }
        public int VisitorId { get; set; }
        public Visitor Visitor { get; set; }
    }
}
