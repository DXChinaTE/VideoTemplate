using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTemplate.Common
{
    public class MenuInfo
    {
        public string normalImage { get; set; }
        public string selectedImage { get; set; }
        public Type t { get; set; }
        public object naviParam { get; set; }
    }
}
