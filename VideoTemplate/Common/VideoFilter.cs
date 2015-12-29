using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTemplate.Common
{
    public enum FilterType
    {
        SORT = 0,
        AREA = 1,
        TYPE = 2,
        DATE = 3,
        PAY = 4
    }
    public class VideoFilter
    {
        public string filterValue { get; set; }
        public FilterType filterType { get; set; }
    }
}
