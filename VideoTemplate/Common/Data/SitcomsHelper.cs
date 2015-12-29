using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace VideoTemplate.Common.Data
{
    public class SitcomsOverview
    {
        public int sitcomsId { get; set; }
        public string sitcomsName { get; set; }
        public string picURL { get; set; }
        public string playCount { get; set; }
        public double picWidth { get; set; }
        public double picHeight { get; set; }
        public SitcomsOverview()
        {
            picWidth = 180;
            picHeight = 102;
        }
    }
    public class SitcomsInfo
    {
        public int sitcomsId { get; set; }
        public string sitcomsName { get; set; }
        public string sitcomsDetail { get; set; }
        public int sitcomsCount { get; set; }
        public string sitcomPlayers { get; set; }
        public string sitcomsType { get; set; }
        public string sitcomsScore { get; set; }
        public string sitcomsDate { get; set; }
        public string sitcomsCountry { get; set; }
        public string picURL { get; set; }
        public string playCount { get; set; }
        public string commentCount { get; set; }
    }

    public class Comments
    {
        public ImageBrush userPcic { get; set; }
        public string comment { get; set; }
        public string date { get; set; }
        public string userName { get; set; }
    }

    public class SitcomsList
    {
        public string id { get; set; }
    }
    public class SuggestVideo
    {
        public string picURL { get; set; }
        public string videoName { get; set; }
        public string playTimes { get; set; }
        public string videoId { get; set; }
    }

    public static class SitcomsHelper
    {
        private static List<SitcomsInfo> sitcomsList = null;
        public static void GetSitcoms(out List<SitcomsInfo> infos)
        {
            if(null == sitcomsList)
            {
                sitcomsList = new List<SitcomsInfo>();
                SitcomsInfo info1 = new SitcomsInfo();
                info1.sitcomPlayers = "凯文·史派西，罗宾·怀特，迈克尔·凯利，拉斯·米科尔森";
                info1.sitcomsCount = 55;
                info1.sitcomsName = "纸牌屋";
                info1.sitcomsCountry = "美国";
                info1.sitcomsDate = "2013-2-1";
                info1.sitcomsDetail = "弗兰西斯·安德伍德是一位极具政治野心的南卡罗来纳州民主党众议员、众议院多数党党鞭。他帮助加勒特·沃克赢得大选成为第45任美国总统，而沃克作为回报则许诺将任命他为国务卿。然而，在沃克宣誓就职之前，白宫幕僚长琳达·瓦斯奎兹却告知安德伍德，称总统希望将他留在国会推动法案通过，因而将不会提名他作国务卿。安德伍德对总统的背叛之举愤怒不已，他决定单干倒戈，逐步实现自己的总统梦想。";
                info1.sitcomsId = 0;
                info1.sitcomsScore = "9.0分";
                info1.playCount = "35,587";
                info1.commentCount = "10,532";
                info1.sitcomsType = "悬疑/政治";
                info1.picURL = "/Assets/FilmImages/movie.png";
                sitcomsList.Add(info1);
                SitcomsInfo info2 = new SitcomsInfo();
                info2.sitcomPlayers = "布莱恩·克兰斯顿，亚伦·保尔，安娜·冈，迪恩·诺里";
                info2.sitcomsCount = 40;
                info2.sitcomsName = "绝命毒师";
                info2.sitcomsCountry = "美国";
                info2.sitcomsDate = "2008-1-20";
                info2.sitcomsDetail = "故事发生在新墨西哥州，高中化学老师沃尔特-怀特是一位平凡得不能再平凡的小人物。沃特本是大学研究所化学高材生，并参与过诺贝尔化学奖的合作，但为了过平凡的生活而放弃成为化学家，在一所高中担任化学老师。但是他却被诊断出患了中晚期肺癌。面对突如其来的不幸，他首先想到的是家人未来的生计问题，因为他家中有一位40多岁怀有身孕的妻子和患有轻度脑瘫的儿子。而他是家里唯一的经济来源。";
                info2.sitcomsId = 1;
                info2.sitcomsScore = "9.0分";
                info2.playCount = "65,162";
                info2.commentCount = "8,615";
                info2.sitcomsType = "剧情/犯罪";
                info2.picURL = "/Assets/FilmImages/movie.png";
                sitcomsList.Add(info2);
            }
            infos = new List<SitcomsInfo>();
            foreach(var item in sitcomsList)
            {
                infos.Add(item);
            }
            return;
        }
    }

    public static class CommentHelper
    {
        private static List<Comments> _comments = null;
        public static void GetComments(out ObservableCollection<Comments> comments)
        {
            if(null == _comments)
            {
                _comments = new List<Comments>();
                for (int i=1; i< 4; i++)
                {
                    Comments comm = new Comments();
                    comm.userName = "user" + Convert.ToString(i);
                    comm.comment = "评论" + Convert.ToString(i);
                    ImageBrush brush = new ImageBrush();
                    brush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/UserPic/" + Convert.ToString(i) + ".jpg"));
                    comm.userPcic = brush;
                    DateTime time = DateTime.Now;
                    time.AddDays(-i);
                    comm.date = time.ToString();
                    _comments.Add(comm);
                }
            }
            comments = new ObservableCollection<Comments>();
            foreach(var item in _comments)
            {
                comments.Add(item);
            }
        }
    }
}
