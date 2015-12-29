using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTemplate.Common.Data
{
    public class FilmOverView
    {
        public string filmId { get; set; }
        public string filmName { get; set; }
        public string picURL { get; set; }
        public string playCount { get; set; }
        public double picWidth { get; set; }
        public double picHeight { get; set; }
        public FilmOverView()
        {
            picWidth = 180;
            picHeight = 102;
        }
    }

    public class FilmInfo
    {
        public string filmId { get; set; }
        public string filmName { get; set; }
        public string filmDetail { get; set; }
        public string filmPlayers { get; set; }
        public string filmType { get; set; }
        public string filmScore { get; set; }
        public string filmDate { get; set; }
        public string filmCountry { get; set; }
        public string picURL { get; set; }
        public string playCount { get; set; }
        public string commentCount { get; set; }
    }
}
