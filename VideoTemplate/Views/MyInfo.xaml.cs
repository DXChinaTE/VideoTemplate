using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using VideoTemplate.Common;
using VideoTemplate.Common.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VideoTemplate.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyInfo : Page
    {
        private Dictionary<ListViewItem, MenuInfo> menuInfomation = new Dictionary<ListViewItem, MenuInfo>();
        private double videoInfoPicWidth = 180;
        private double videoInfoPicHeight = 102;
        bool isMobile = false;
        public MyInfo()
        {           
            this.InitializeComponent();
            MySplitView.OpenPaneLength = 180;
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Mobile"))//Windows.Desktop
            {
                isMobile = true;
                videoInfoPicWidth = (Window.Current.Bounds.Width - 50) / 2;
                videoInfoPicHeight = videoInfoPicWidth * 102 / 180;
                MySplitView.OpenPaneLength = Window.Current.Bounds.Width;
            }
            InitData();
            InitMenuInfo();
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/sample.jpg"));
            userPic.Fill = brush;           
        }

        public bool NavigateBack()
        {
            if(MySplitView.IsPaneOpen)
            {
                return true;
            }
            else
            {
                MySplitView.IsPaneOpen = true;
                return false;
            }
        }
        private void InitMenuInfo()
        {
            MenuInfo infohistory = new MenuInfo();
            infohistory.t = null;
            infohistory.normalImage = "ms-appx:///Assets/MenuIco/design-07_01.png";
            infohistory.selectedImage = "ms-appx:///Assets/MenuIco/design07 01.png";
            menuInfomation.Add(history, infohistory);
            MenuInfo infomyCache = new MenuInfo();
            infomyCache.t = null;
            infomyCache.normalImage = "ms-appx:///Assets/MenuIco/design-07_03.png";
            infomyCache.selectedImage = "ms-appx:///Assets/MenuIco/design07 03.png";
            menuInfomation.Add(myCache, infomyCache);
            MenuInfo infomySitcoms = new MenuInfo();
            infomySitcoms.t = null;
            infomySitcoms.normalImage = "ms-appx:///Assets/MenuIco/design-07_02.png";
            infomySitcoms.selectedImage = "ms-appx:///Assets/MenuIco/design07 02.png";
            menuInfomation.Add(mySitcoms, infomySitcoms);
            MenuInfo infosetting = new MenuInfo();
            infosetting.t = null;
            infosetting.normalImage = "ms-appx:///Assets/MenuIco/design-07_04.png";
            infosetting.selectedImage = "ms-appx:///Assets/MenuIco/design07 04.png";
            menuInfomation.Add(setting, infosetting);
            if (!isMobile && Window.Current.Bounds.Width > 500)
            {
                memberListView.SelectedIndex = 0;
                MySplitView.IsPaneOpen = true;
            }
        }
        private void InitData()
        {
            ObservableCollection<SitcomsOverview> sitcoms = new ObservableCollection<SitcomsOverview>();
            for (int i = 0; i < 10; i++)
            {
                SitcomsOverview info = new SitcomsOverview();
                double count = 2.5;
                Random indexRandomizer = new Random(i);
                count += indexRandomizer.Next(0, 20) * 0.1;
                info.playCount = Convert.ToString(count) + "万";
                info.picHeight = videoInfoPicHeight;
                info.picWidth = videoInfoPicWidth;
                info.sitcomsId = indexRandomizer.Next(0, 2);
                info.sitcomsName = string.Format("视频名称{0}", i + 1);
                info.picURL = string.Format("/Assets/SitcomsImages/sitcoms{0}.jpg", i + 1);//"/Assets/FilmImages/sitcom.png";
                sitcoms.Add(info);
            }           
            todayGrid.ItemsSource = sitcoms;
            yesterdayGrid.ItemsSource = sitcoms;                           
        }
      

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!(Window.Current.Bounds.Width > 500) && isMobile)
            {               
                MySplitView.IsPaneOpen = true;
            }
            else if(!(Window.Current.Bounds.Width > 500))
            {
                MySplitView.OpenPaneLength = Window.Current.Bounds.Width;
                MySplitView.IsPaneOpen = true;
            }
            else
            {
                MySplitView.OpenPaneLength = 180;
                MySplitView.IsPaneOpen = true;
            }            
            double width = memberListView.ActualWidth;           
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Mobile"))//Windows.Desktop
            {
                width = Window.Current.Bounds.Width;
            }
            foreach (ListViewItem item in memberListView.Items)
            {
                RelativePanel contentPanel = item.Content as RelativePanel;
                contentPanel.Width = width;
                int childCount = VisualTreeHelper.GetChildrenCount(contentPanel);
                for(int i=0; i< childCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(contentPanel,i);
                    if(child.ToString().Equals((typeof(RelativePanel)).ToString()))
                    {
                        (child as RelativePanel).Width = width;
                        var scroll = VisualTreeHelper.GetChild(child,0);
                        if(scroll != null && scroll.ToString().Equals((typeof(ScrollViewer)).ToString()))
                        {
                            (scroll as ScrollViewer).Width = width;
                        }
                    }
                }
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            MainPage._currentHandle.ResetMainWindow(false);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPage._currentHandle.ResetMainWindow(true);
        }

        private void memberListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView list = sender as ListView;
            if(!(Window.Current.Bounds.Width > 500) && list.SelectedItem.Equals(history))
            {
                MySplitView.IsPaneOpen = false;
            }
            foreach(var item in menuInfomation)
            {
                var content = (item.Key as ListViewItem).Content;
                var childImage = VisualTreeHelper.GetChild(content as UIElement, 0);
                var childText = VisualTreeHelper.GetChild(content as UIElement, 1);
                if (item.Key.Equals(list.SelectedItem))
                {
                    (childImage as Image).Source = new BitmapImage(new Uri(item.Value.selectedImage));
                    (childText as TextBlock).Foreground = new SolidColorBrush(Color.FromArgb(255, 25, 220, 255));
                }
                else
                {
                    (childImage as Image).Source = new BitmapImage(new Uri(item.Value.normalImage));
                    (childText as TextBlock).Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 238, 238));
                }
            } 
        }

    }
}
