using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entity.ViewModels.Others
{
    public class VMSocialMediaUpdate
    {
        public int Id { get; set; }
        public int SocialMediaType { get; set; }
        public string Link { get; set; }

        public string GetTypeName()
        {
            if (Id == 1)
                return "LinkedIn";
            else if (Id == 2)
                return "Instagram";
            else if (Id == 3)
                return "Facebook";
            else if (Id == 4)
                return "X";
            else return "Hata";
        }
    }
}
