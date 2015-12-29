using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel.Core;
using VideoTemplate.Common;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace VideoTemplate.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Dictionary<object, MenuInfo> menuInfomation = new Dictionary<object, MenuInfo>();
        public static MainPage _currentHandle;
        private Dictionary<ListViewItem, Type> listviewNaviTypes = new Dictionary<ListViewItem, Type>();
        private bool navigate = true;
        private bool isMyInfoPage = false;
        public MainPage()
        {
            this.InitializeComponent();
            _currentHandle = this;            
            InitMainwindow();           
            if (!Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Mobile"))//Windows.Desktop
            {
                //tabletBackButtonPanel.Height = 32;// CoreApplication.MainView.TitleBar.Height;
                //backButton.Width = backButton.Height = 32;
                //CoreApplication.MainView.TitleBar.ExtendViewIntoTitleBar = true;
                //appName.Text = Windows.ApplicationModel.Package.Current.DisplayName;
                //Window.Current.SetTitleBar(pageName);                  
                SetTitleBar();           

            }
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }       
        }

        private void SetTitleBar()
        {
            var coreTitleBar = Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().TitleBar;
            var appTitleBar= Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            appTitleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;          
            Window.Current.SetTitleBar(myTitlebar);
            appName.Text = "在线视频";//Windows.ApplicationModel.Package.Current.DisplayName;
        }
       
        private void CoreTitleBar_LayoutMetricsChanged(Windows.ApplicationModel.Core.CoreApplicationViewTitleBar sender,object args)
        {
            backButton.Height = backButton.Width = tabletBackButtonPanel.Height = sender.Height;
            //tabletBackButtonPanel.Padding
            //  = new Thickness(
            //          sender.SystemOverlayLeftInset,
            //          0.0,
            //          sender.SystemOverlayRightInset,
            //          0.0
            //        );
        }


        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            e.Handled = true;
            NavigateBack();
           
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateBack();
        }

        private void NavigateBack()
        {
            if(isMyInfoPage)
            {
                object page = workFrame.Content;
                if(!(page as MyInfo).NavigateBack())
                {
                    return;
                }
            }
            if (workFrame.BackStack.Count > 0)
            {
                Windows.UI.Xaml.Navigation.PageStackEntry page = workFrame.BackStack[workFrame.BackStack.Count - 1];
                string typeName = page.SourcePageType.FullName;
                foreach (var item in listviewNaviTypes)
                {
                    if (item.Value.ToString().Equals(typeName))
                    {
                        navigate = false;
                        if (menuList.SelectedItem.Equals(item.Key))
                        {
                            ResetListView(item.Key);
                        }
                        else
                        {
                            menuList.SelectedItem = item.Key;
                        }
                        break;
                    }
                }
                workFrame.GoBack();
            }
        }
        private void InitMainwindow()
        {
            listviewNaviTypes.Add(homeItem,typeof(home));
            listviewNaviTypes.Add(liveItem,typeof(Live));
            listviewNaviTypes.Add(movieItem, typeof(Movie));
            listviewNaviTypes.Add(sitcomsItem, typeof(Sitcoms));
            MenuInfo infoHome = new MenuInfo();
            infoHome.t = typeof(home);
            infoHome.normalImage = "ms-appx:///Assets/MenuIco/H1.1.png";
            infoHome.selectedImage = "ms-appx:///Assets/MenuIco/H1.png";
            menuInfomation.Add(homeItemImage, infoHome);
            MenuInfo infolive = new MenuInfo();
            infolive.t = typeof(Live);
            infolive.normalImage = "ms-appx:///Assets/MenuIco/H1.2.png";
            infolive.selectedImage = "ms-appx:///Assets/MenuIco/H2.png";
            menuInfomation.Add(liveItemImage, infolive);
            MenuInfo infomovie = new MenuInfo();
            infomovie.t = typeof(Movie);
            infomovie.normalImage = "ms-appx:///Assets/MenuIco/H1.3.png";
            infomovie.selectedImage = "ms-appx:///Assets/MenuIco/H3.png";
            menuInfomation.Add(movieItemImage, infomovie);
            MenuInfo infositcoms = new MenuInfo();
            infositcoms.t = typeof(Sitcoms);
            infositcoms.normalImage = "ms-appx:///Assets/MenuIco/H1.4.png";
            infositcoms.selectedImage = "ms-appx:///Assets/MenuIco/H4.png";
            menuInfomation.Add(sitcomsImage, infositcoms);
            MenuInfo infovariety = new MenuInfo();
            infovariety.t = typeof(Variety);
            infovariety.normalImage = "ms-appx:///Assets/MenuIco/H1.5.png";
            infovariety.selectedImage = "ms-appx:///Assets/MenuIco/H5.png";
            menuInfomation.Add(varietyItemImage, infovariety);
            MenuInfo infoentertainment = new MenuInfo();
            infoentertainment.t = typeof(Entertainment);
            infoentertainment.normalImage = "ms-appx:///Assets/MenuIco/H1.6.png";
            infoentertainment.selectedImage = "ms-appx:///Assets/MenuIco/H6.png";
            menuInfomation.Add(entertainmentItemImage, infoentertainment);
            MenuInfo infosports = new MenuInfo();
            infosports.t = typeof(Sports);
            infosports.normalImage = "ms-appx:///Assets/MenuIco/H1.7.png";
            infosports.selectedImage = "ms-appx:///Assets/MenuIco/H7.png";
            menuInfomation.Add(sportsItemImage, infosports);
            MenuInfo infomusic = new MenuInfo();
            infomusic.t = typeof(Music);
            infomusic.normalImage = "ms-appx:///Assets/MenuIco/H1.8.png";
            infomusic.selectedImage = "ms-appx:///Assets/MenuIco/H8.png";
            menuInfomation.Add(musicItemImage, infomusic);                   
            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/sample.jpg"));
            myPicEllipse.Fill = brush;
            //navigate to home
            menuList.SelectedIndex = 0;
        }

        private void ResetListView(object listviewItem)
        {
            foreach(var item in menuList.Items)
            {
                var content = (item as ListViewItem).Content;
                var childImage = VisualTreeHelper.GetChild(content as UIElement, 0);
                var childText = VisualTreeHelper.GetChild(content as UIElement, 1);
                if (item.Equals(listviewItem))
                {
                    (childImage as Image).Source = new BitmapImage(new Uri(menuInfomation[childImage].selectedImage));
                    (childText as TextBlock).Foreground = new SolidColorBrush(Color.FromArgb(255, 25, 220, 255));
                    if (navigate)
                    {
                        foreach (var naviType in listviewNaviTypes)
                        {
                            if (naviType.Key.Equals(item))
                            {
                                workFrame.Navigate(menuInfomation[childImage].t, menuInfomation[childImage].naviParam);
                                break;
                            }
                        }
                    }
                    else
                    {
                        navigate = true;
                    }
                }
                else
                {
                    (childImage as Image).Source = new BitmapImage(new Uri(menuInfomation[childImage].normalImage));
                    (childText as TextBlock).Foreground = new SolidColorBrush(Color.FromArgb(255, 238, 238, 238));
                }
            }        
        }
 
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView list = sender as ListView;
            ResetListView(list.SelectedItem);
            if(MySplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                MySplitView.IsPaneOpen = false;
            }
        }

        private void toggleBtn_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void search_LostFocus(object sender, RoutedEventArgs e)
        {
            if(Logo.Visibility == Visibility.Collapsed)
            {
                Logo.Visibility = Visibility.Visible;
                search.Width = 30;
            }
        }

        private void search_GotFocus(object sender, RoutedEventArgs e)
        {
            if(search.Width == 30)
            {
                //Storyboard storyBoard = (Storyboard)this.Resources["searchBoxVisible"];
                //storyBoard.Begin();

                ////search.Width = 150;
                //storyBoard.Begin();
                Logo.Visibility = Visibility.Collapsed;
                search.Width = 150;              
            }            
        }

        public void Navigate(Type t, object param)
        {
            foreach (var item in menuInfomation)
            {
                if(item.Value.t.Equals(t))
                {
                    item.Value.naviParam = param;
                    break;
                }
            }
            //workFrame.Navigate(t, param);
            foreach(var item in listviewNaviTypes)
            {
                if(item.Value.Equals(t))
                {
                    if(menuList.SelectedItem.Equals(item.Key))
                    {
                        ResetListView(item.Key);
                    }
                    else
                    {
                        menuList.SelectedItem = item.Key;
                    }                    
                    break;
                }                
            }
        }  
        
        public void ResetMainWindow(bool fullScreenWorkspace)
        {
            isMyInfoPage = fullScreenWorkspace;
            if (fullScreenWorkspace)
            {
                menuList.Width = 0;
                MySplitView.OpenPaneLength = 0;
                MySplitView.CompactPaneLength = 0;
                topMenuPanel.Height = 0;
            } 
            else
            {
                menuList.Width = MySplitView.OpenPaneLength = 180;               
                MySplitView.CompactPaneLength = 70;
                topMenuPanel.Height = 50;
            }           
        }

        public void ResetListviewMenu()
        {

        }

        private void userInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            workFrame.Navigate(typeof(MyInfo));
        }
    }
}
