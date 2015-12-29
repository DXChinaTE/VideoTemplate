using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VideoTemplate.Common.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VideoTemplate.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class home : Page
    {
        private List<int> toolList = new List<int>();
        private Dictionary<FlipView, int> flipViewSelectedIndexs = new Dictionary<FlipView, int>();
        private int flipviewItemsCount = 0;
        private bool isMobile = false;
        private double videoInfoPicWidth = 180;
        private double videoInfoPicHeight = 102;

        public home()
        {
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Mobile"))//Windows.Desktop
            {
                isMobile = true;
                videoInfoPicWidth = (Window.Current.Bounds.Width - 50) / 2;
                videoInfoPicHeight = videoInfoPicWidth * 102 / 180;
            }

            this.InitializeComponent();          
            var sampleData = new SampleDataSource();
            flipviewItemsCount = sampleData.Items.Count;
            flipView2.ItemsSource = sampleData.Items;
            flipView1.ItemsSource = sampleData.Items;
            flipView3.ItemsSource = sampleData.Items;
            for (int i = 0; i < sampleData.Items.Count-2; i++)
            {
                toolList.Add(i);
            }
            ContextControl.ItemsSource = toolList;            
            flipView2.SelectionChanged += FlipView_SelectionChanged;
            flipView1.SelectionChanged += FlipView1_SelectionChanged;
            flipView3.SelectionChanged += FlipView3_SelectionChanged;         
            flipViewSelectedIndexs.Add(flipView2,1);
            flipViewSelectedIndexs.Add(flipView1,flipviewItemsCount-2);
            flipViewSelectedIndexs.Add(flipView3,2);
            flipView2.SelectedIndex = 1;
            flipView1.SelectedIndex = flipviewItemsCount - 2;
            flipView3.SelectedIndex = 2;
            ContextControl.SelectedIndex = 0;
            ContextControl.SelectionChanged += ContextControl_SelectionChanged;

            //ObservableCollection<FilmData> films = new ObservableCollection<FilmData>();
            //for(int i=0; i<10; i++)
            //{
            //    FilmData data = new FilmData();
            //    data.amount = "3.1万";
            //    data.picUri = "/Assets/FilmImages/movie.png";
            //    films.Add(data);
            //}
            //FilmsCVS.Source = films;
            ObservableCollection<SitcomsOverview> sitcoms = new ObservableCollection<SitcomsOverview>();           
            for (int i = 0; i<10; i++)
            {
                SitcomsOverview info = new SitcomsOverview();
                double count = 2.5;
                Random indexRandomizer = new Random(i);
                count += indexRandomizer.Next(0, 20)*0.1;
                info.playCount = Convert.ToString(count) + "万";
                info.sitcomsId = indexRandomizer.Next(0,2);
                info.picHeight = videoInfoPicHeight;
                info.picWidth = videoInfoPicWidth;
                info.sitcomsName = string.Format("电视剧名称{0}",i+1);
                info.picURL =  string.Format("/Assets/SitcomsImages/sitcoms{0}.jpg",i+1);//"/Assets/FilmImages/sitcom.png";
                sitcoms.Add(info);
            }
            sitcomsGrid.ItemsSource = sitcoms;
            ObservableCollection<FilmOverView> films = new ObservableCollection<FilmOverView>();
            for(int i=0; i< 10; i++)
            {

                FilmOverView info = new FilmOverView();
                double count = 2.5;
                Random indexRandomizer = new Random(i);
                count += indexRandomizer.Next(0, 20) * 0.1;
                info.picWidth = videoInfoPicWidth;
                info.picHeight = videoInfoPicHeight;
                info.playCount = Convert.ToString(count) + "万";
                info.filmId = Convert.ToString(i);
                info.filmName = string.Format("电影名称{0}",i+1);
                info.picURL = string.Format("/Assets/FilmImages/film{0}.jpg",i+1);
                films.Add(info);
            }            
            filmsGrid.ItemsSource = films;
        }

        private void FlipView3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FlipView flip = sender as FlipView;
            if (flipViewSelectedIndexs[flip] == flip.SelectedIndex)
            {
                return;
            }
            if (flipviewItemsCount < 3)
            {
                return;
            }
            if (flip.SelectedIndex == 0)
            {
                flip.SelectedIndex = flipviewItemsCount - 2;
            }
            else if (flip.SelectedIndex == flipviewItemsCount - 1)
            {
                flip.SelectedIndex = 1;
            }
            int index = flip.SelectedIndex;
            flipViewSelectedIndexs[flip] = index;
            ResetFlipVewAndListBox(new KeyValuePair<FlipView, int>(flip, index));
        }

        private void FlipView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FlipView flip = sender as FlipView;
            if (flipViewSelectedIndexs[flip] == flip.SelectedIndex)
            {
                return;
            }
            if (flipviewItemsCount < 3)
            {
                return;
            }
            if (flip.SelectedIndex == 0)
            {
                flip.SelectedIndex = flipviewItemsCount - 2;
            }
            else if (flip.SelectedIndex == flipviewItemsCount - 1)
            {
                flip.SelectedIndex = 1;
            }
            int index = flip.SelectedIndex;
            flipViewSelectedIndexs[flip] = index;
            ResetFlipVewAndListBox(new KeyValuePair<FlipView, int>(flip, index));
        }

        private void ResetFlipVewAndListBox(KeyValuePair<FlipView,int> pair)
        {
            if(pair.Key.Equals(flipView1))
            {
                int index1 = pair.Value;
                int index2 = ++index1;
                if (index2 < flipviewItemsCount)
                {
                    if (index2 == 0)
                    {
                        index2 = flipviewItemsCount - 2;
                    }
                    else if (index2 == flipviewItemsCount - 1)
                    {
                        index2 = 1;
                    }                    
                }
                else
                {
                    index2 = 1;
                }
                flipViewSelectedIndexs[flipView2] = index2;
                flipView2.SelectedIndex = index2;
                int index3 = ++index2;
                if (index3 < flipviewItemsCount)
                {
                    if (index3 == 0)
                    {
                        index3 = flipviewItemsCount - 2;
                    }
                    else if (index3 == flipviewItemsCount - 1)
                    {
                        index3 = 1;
                    }
                }
                else
                {
                    index3 = 1;
                }
                flipViewSelectedIndexs[flipView3] = index3;
                flipView3.SelectedIndex = index3;
            }
            else if(pair.Key.Equals(flipView2))
            {
                int index2 = pair.Value;
                int index1 = index2-1;
                if(index1 >= 0)
                {
                    if (index1 == 0)
                    {
                        index1 = flipviewItemsCount - 2;
                    }
                    else if (index2 == flipviewItemsCount - 1)
                    {
                        index1 = 1;
                    }
                }
                else
                {
                    index1 = flipviewItemsCount - 3;
                }
                flipViewSelectedIndexs[flipView1] = index1;
                flipView1.SelectedIndex = index1;
                int index3 = index2+1;
                if (index3 < flipviewItemsCount)
                {
                    if (index3 == 0)
                    {
                        index3 = flipviewItemsCount - 2;
                    }
                    else if (index3 == flipviewItemsCount - 1)
                    {
                        index3 = 1;
                    }
                }
                else
                {
                    index3 = 1;
                }
                flipViewSelectedIndexs[flipView3] = index3;
                flipView3.SelectedIndex = index3;
            }
            else//flipview3
            {
                int index3 = pair.Value;
                int index2 = --index3;
                if (index2 >= 0)
                {
                    if (index2 == 0)
                    {
                        index2 = flipviewItemsCount - 2;
                    }
                    else if (index2 == flipviewItemsCount - 1)
                    {
                        index2 = 1;
                    }
                }
                else
                {
                    index2 = flipviewItemsCount - 3;
                }
                flipViewSelectedIndexs[flipView2] = index2;
                flipView2.SelectedIndex = index2;
                int index1 = --index2;
                if (index1 >= 0)
                {
                    if (index1 == 0)
                    {
                        index1 = flipviewItemsCount - 2;
                    }
                    else if (index1 == flipviewItemsCount - 1)
                    {
                        index1 = 1;
                    }
                }
                else
                {
                    index1 = flipviewItemsCount - 3;
                }
                flipViewSelectedIndexs[flipView1] = index1;
                flipView1.SelectedIndex = index1;
            }
            ContextControl.SelectedIndex = flipView2.SelectedIndex - 1;           
        }
        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FlipView flip = sender as FlipView;
            if(flipViewSelectedIndexs[flip] == flip.SelectedIndex)
            {
                return;
            }            
            if(flipviewItemsCount < 3)
            {
                return;
            }            
            if (flip.SelectedIndex == 0)
            {
                flip.SelectedIndex = flipviewItemsCount - 2;
            }
            else if (flip.SelectedIndex == flipviewItemsCount - 1)
            {
                flip.SelectedIndex = 1;
            }
            int index = flip.SelectedIndex;
            flipViewSelectedIndexs[flip] = index;
            ResetFlipVewAndListBox(new KeyValuePair<FlipView, int>(flip, index));
        }

        void ContextControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
           // flipView2.Focus(Windows.UI.Xaml.FocusState.Pointer);
            int index = (sender as ListBox).SelectedIndex;
            flipView2.SelectedIndex = index + 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void SitcomsButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (sender as Button).Name;
            MainPage._currentHandle.Navigate(typeof(Sitcoms),id);
            return;
        }
        private void FilmsButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (sender as Button).Name;
            MainPage._currentHandle.Navigate(typeof(Movie), id);
            return;
        }

        private void moreFilmBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPage._currentHandle.Navigate(typeof(Movie),null);
        }

        private void RelativePanel_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if(isMobile)
            {
                return;
            }
            leftFlipPanel.Visibility = rightFlipPanel.Visibility = Visibility.Visible;
        }

        private void RelativePanel_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (isMobile)
            {
                return;
            }
            leftFlipPanel.Visibility = rightFlipPanel.Visibility = Visibility.Collapsed;
        }

        private void rightFlip_Click(object sender, RoutedEventArgs e)
        {
            int index = flipView2.SelectedIndex;
            ++index;
            if (index < flipviewItemsCount)
            {
                if (index == 0)
                {
                    index = flipviewItemsCount - 2;
                }
                else if (index == flipviewItemsCount - 1)
                {
                    index = 1;
                }
            }
            else
            {
                index = 1;
            }
            flipViewSelectedIndexs[flipView2] = index;
            flipView2.SelectedIndex = index;
            ResetFlipVewAndListBox(new KeyValuePair<FlipView, int>(flipView2, index));
        }

        private void leftFlip_Click(object sender, RoutedEventArgs e)
        {
            int index = flipView2.SelectedIndex;
            --index;
            if (index >= 0)
            {
                if (index == 0)
                {
                    index = flipviewItemsCount - 2;
                }
                else if (index == flipviewItemsCount - 1)
                {
                    index = 1;
                }
            }
            else
            {
                index = flipviewItemsCount - 3;
            }
            flipViewSelectedIndexs[flipView2] = index;
            flipView2.SelectedIndex = index;
            ResetFlipVewAndListBox(new KeyValuePair<FlipView, int>(flipView2,index));
        }

        private void moreSitcomsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainPage._currentHandle.Navigate(typeof(Sitcoms),null);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            leftFlipPanel.Height = rightFlipPanel.Height = flipView1.Height = flipView2.Height = flipView3.Height = flipView2.ActualWidth * 270 / 480;
            flipViewPanel1.Width = flipViewPanel2.Width = flipView1.Width = flipView3.Width = flipView2.Width;
        }
    }
}
