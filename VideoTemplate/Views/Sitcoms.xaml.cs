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
using Windows.UI.Xaml.Navigation;
using Windows.Media.Casting;
using VideoTemplate.Common;
using VideoTemplate.Common.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VideoTemplate.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Sitcoms : Page
    {
        private CastingDevicePicker picker = null;
        private VideoMetaData video = null;
        private CastingConnection connection;
        private int sitcomsId = -1;
        private string sitcomsName = string.Empty;
        private string sitcomsDetail = string.Empty;
        private int sitcomsCount = 0;
        private string sitcomPlayers = string.Empty;
        private string sitcomsType = string.Empty;
        private string sitcomsScore = string.Empty;
        private string sitcomsDate = string.Empty;
        private string sitcomsCountry = string.Empty;
        private string playCount = string.Empty;
        private string commentCount = string.Empty;
        private List<SitcomsInfo> sitcomsList = null;

        private double videoInfoPicWidth = 180;
        private double videoInfoPicHeight = 102;
        public Sitcoms()
        {                       
            this.InitializeComponent();                   
        }

        private void InitPlayer()
        {
            player.MediaOpened += Player_MediaOpened;
            player.MediaFailed += Player_MediaFailed;
            player.CurrentStateChanged += Player_CurrentStateChanged;

            // Get an Azure hosted video
            VideoDataProvider dataProvider = new VideoDataProvider();
            video = dataProvider.GetRandomVideo();

            //Set the source on the player           
            player.Source = video.VideoLink;

            //Subscribe for the clicked event on the custom cast button
            //((MediaTransportControlsWithCustomCastButton)this.player.TransportControls).CastButtonClicked += TransportControls_CastButtonClicked;

            // Instantiate the Device Picker
            picker = new CastingDevicePicker();

            // Generate the filter based on the content in the MediaElement
            picker.Filter.SupportedCastingSources.Add(player.GetAsCastingSource());

            //Hook up device selected event
            picker.CastingDeviceSelected += Picker_CastingDeviceSelected;

            //Hook up device disconnected event
            picker.CastingDevicePickerDismissed += Picker_CastingDevicePickerDismissed;

            //Set the Appearence of the picker
            picker.Appearance.BackgroundColor = Colors.Black;
            picker.Appearance.ForegroundColor = Colors.White;
            picker.Appearance.AccentColor = Colors.Gray;

            picker.Appearance.SelectedAccentColor = Colors.Gray;

            picker.Appearance.SelectedForegroundColor = Colors.White;
            picker.Appearance.SelectedBackgroundColor = Colors.Black;
        }

        private void InitPageData()
        {           
            SitcomsInfo info = null;
            SitcomsHelper.GetSitcoms(out sitcomsList);
            foreach (var item in sitcomsList)
            {
                if (null == info)
                {
                    info = item;
                }
                if (item.sitcomsId.Equals(sitcomsId))
                {
                    info = item;
                    break;
                }
            }
            sitcomsList.Remove(info);

            sitComsNameOfHidePanel.Text = sitComsName.Text = sitcomsName = info.sitcomsName;
            sitcomsDetaiText.Text = sitcomsDetail = info.sitcomsDetail;
            sitcomsCount = info.sitcomsCount;
            players.Text = sitcomPlayers = info.sitcomPlayers;
            type.Text = sitcomsType = info.sitcomsType;
            score.Text = sitcomsScore = info.sitcomsScore;
            publicDate.Text = sitcomsDate = info.sitcomsDate;
            area.Text = sitcomsCountry = info.sitcomsCountry;
            playCounts.Text = playCount = info.playCount;
            commentsNumber.Text = commentCount = info.commentCount;
           
            ObservableCollection<SuggestVideo> suggestVideos = new ObservableCollection<SuggestVideo>();
            foreach (var item in sitcomsList)
            {
                SuggestVideo video1 = new SuggestVideo();
                video1.videoName = item.sitcomsName;
                video1.playTimes = "播放" + item.playCount;
                video1.videoId = Convert.ToString(item.sitcomsId);
                video1.picURL = item.picURL;
                suggestVideos.Add(video1);
            }
            suggestVideo.ItemsSource = suggestVideos;
            suggestVideo1.ItemsSource = suggestVideos;

            ObservableCollection<Comments> comments = null;
            CommentHelper.GetComments(out comments);
            userComments.ItemsSource = comments;
            userComments1.ItemsSource = comments;
            ObservableCollection<SitcomsList> list = new ObservableCollection<SitcomsList>();
            for (int i = 1; i <= sitcomsCount; i++)
            {
                SitcomsList item = new SitcomsList();
                item.id = Convert.ToString(i);
                list.Add(item);
            }          
            xuanji.ItemsSource = list;           
            xuanji1.ItemsSource = list;
            xuanji.SelectedIndex = 0;
            xuanji1.SelectedIndex = 0;
        }
        private void TransportControls_CastButtonClicked(object sender, EventArgs e)
        {          
            //Pause Current Playback
            player.Pause();

            //Get the custom transport controls
            MediaTransportControlsWithCustomCastButton mtc = (MediaTransportControlsWithCustomCastButton)this.player.TransportControls;

            //Retrieve the location of the casting button
            GeneralTransform transform = mtc.CastButton.TransformToVisual(Window.Current.Content as UIElement);
            Point pt = transform.TransformPoint(new Point(0, 0));

            //Show the picker above our custom cast button
            picker.Show(new Rect(pt.X, pt.Y, mtc.CastButton.ActualWidth, mtc.CastButton.ActualHeight), Windows.UI.Popups.Placement.Above);
        }

        private async void Picker_CastingDevicePickerDismissed(CastingDevicePicker sender, object args)
        {
            // This dispatches the casting calls to the UI thread.
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                player.Play();
            });
        }
        private async void Picker_CastingDeviceSelected(CastingDevicePicker sender, CastingDeviceSelectedEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                try
                {                   
                    //DateTime t1 = DateTime.Now;
                    //DeviceInformation mydevice = await DeviceInformation.CreateFromIdAsync(args.SelectedCastingDevice.Id);
                    //DateTime t2 = DateTime.Now;

                    //TimeSpan ts = new TimeSpan(t2.Ticks - t1.Ticks);

                    //System.Diagnostics.Debug.WriteLine(string.Format("DeviceInformation.CreateFromIdAsync took '{0} seconds'", ts.TotalSeconds));

                    //Create a casting conneciton from our selected casting device
                    //rootPage.NotifyUser(string.Format("Creating connection for '{0}'", args.SelectedCastingDevice.FriendlyName), NotifyType.StatusMessage);
                    connection = args.SelectedCastingDevice.CreateCastingConnection();

                    //Hook up the casting events
                    connection.ErrorOccurred += Connection_ErrorOccurred;
                    connection.StateChanged += Connection_StateChanged;

                    // Get the casting source from the MediaElement
                    CastingSource source = null;

                    try
                    {
                        // Get the casting source from the Media Element
                        source = player.GetAsCastingSource();

                        // Start Casting                        
                        CastingConnectionErrorStatus status = await connection.RequestStartCastingAsync(source);

                        if (status == CastingConnectionErrorStatus.Succeeded)
                        {
                            player.Play();                          
                        }

                    }
                    catch
                    {                        
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine(ex.Message);
#endif
                }
            });
        }

        #region Casting Connection Status Methods
        private async void Connection_StateChanged(CastingConnection sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {               
            });
        }
        private async void Connection_ErrorOccurred(CastingConnection sender, CastingConnectionErrorOccurredEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {                
            });
        }

        #endregion
        private void Player_CurrentStateChanged(object sender, RoutedEventArgs e)
        {            
        }
        private void Player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
           
        }
        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void detailBtn_Click(object sender, RoutedEventArgs e)
        {
            sitcomsInfoGrid.Visibility = Visibility.Visible;
            introducePanel.Visibility = Visibility.Visible;
            detaiHidingPanel.Visibility = Visibility.Collapsed;
        }

        private void closeDetail_Click(object sender, RoutedEventArgs e)
        {
            sitcomsInfoGrid.Visibility = Visibility.Collapsed;
            introducePanel.Visibility = Visibility.Collapsed;
            detaiHidingPanel.Visibility = Visibility.Visible;
        }

        private void playVideo_Click(object sender, RoutedEventArgs e)
        {
            string id = (sender as Button).Name;
            MainPage._currentHandle.Navigate(typeof(Sitcoms),id);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(e.Parameter != null)
            {
                string id = e.Parameter as string;
                sitcomsId = Convert.ToInt32(id);
                videoPlayerPage.Visibility = Visibility.Visible;
                filterPage.Visibility = Visibility.Collapsed;
                InitPlayer();
                InitPageData();
            }    
            else
            {
                SetFilterValueSource();
                InitSitcoms();
                filterPage.Visibility = Visibility.Visible;
                videoPlayerPage.Visibility = Visibility.Collapsed;
            }      
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
                      
            return;
        }

        private void xuanji1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView grid = sender as GridView;
            xuanji.SelectedIndex = grid.SelectedIndex;
        }

        private void xuanji_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView grid = sender as GridView;           
            xuanji1.SelectedIndex = grid.SelectedIndex;
        }
        #region filterGrid
        private void SetFilterValueSource()
        {
            List<VideoFilter> sortFilters = new List<VideoFilter>();
            string filterString = "新上架;最热门;好评榜";
            string[] filters = filterString.Split(new char[] { ';' });
            foreach (var item in filters)
            {
                VideoFilter filter = new VideoFilter();
                filter.filterValue = item;
                filter.filterType = FilterType.SORT;
                sortFilters.Add(filter);
            }
            sortListBox.ItemsSource = sortFilters;
            List<VideoFilter> areaFilters = new List<VideoFilter>();
            string areaString = "全部;中国;美国;意大利;韩国;英国;日本;德国;加拿大;法国;香港;台湾";
            string[] areaFilterValues = areaString.Split(new char[] { ';' });
            foreach (var item in areaFilterValues)
            {
                VideoFilter filter = new VideoFilter();
                filter.filterType = FilterType.AREA;
                filter.filterValue = item;
                areaFilters.Add(filter);
            }
            areaListBox.ItemsSource = areaFilters;
            List<VideoFilter> typeFilterValue = new List<VideoFilter>();
            string typeFilterString = "全部;犯罪;科幻;悬疑;惊悚;战争;剧情;警匪";
            string[] typeFilters = typeFilterString.Split(new char[] { ';' });
            foreach (var item in typeFilters)
            {
                VideoFilter filter = new VideoFilter();
                filter.filterType = FilterType.TYPE;
                filter.filterValue = item;
                typeFilterValue.Add(filter);
            }
            typeListBox.ItemsSource = typeFilterValue;
            List<VideoFilter> dateFilterValue = new List<VideoFilter>();
            string dateFilterString = "全部;2015;2014;2013;2012;2011;2010;00年代;90年代;80年代;70年代;60年代";
            string[] dateFilters = dateFilterString.Split(new char[] { ';' });
            foreach (var item in dateFilters)
            {
                VideoFilter filter = new VideoFilter();
                filter.filterType = FilterType.DATE;
                filter.filterValue = item;
                dateFilterValue.Add(filter);
            }
            dateListBox.ItemsSource = dateFilterValue;
            List<VideoFilter> payFilterValue = new List<VideoFilter>();
            string payFilterString = "全部;免费;付费;会员";
            string[] payFilterValues = payFilterString.Split(new char[] { ';' });
            foreach (var item in payFilterValues)
            {
                VideoFilter filter = new VideoFilter();
                filter.filterType = FilterType.PAY;
                filter.filterValue = item;
                payFilterValue.Add(filter);
            }
            payListBox.ItemsSource = payFilterValue;
        }
        private void sortListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetListBox(sender as ListBox);
        }

        private void areaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetListBox(sender as ListBox);
        }

        private void typeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetListBox(sender as ListBox);
        }

        private void dateListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetListBox(sender as ListBox);
        }

        private void payListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetListBox(sender as ListBox);
        }
        private void ResetListBox(ListBox list)
        {
            var itemsPanel = GetListBoxItemPanel(list);
            if(null == itemsPanel)
            {
                return;
            }
            int count = VisualTreeHelper.GetChildrenCount(itemsPanel as UIElement); 
            for(int i=0; i< count; i++)
            {
                var listBoxItem = VisualTreeHelper.GetChild(itemsPanel as UIElement, i);
                var grid = VisualTreeHelper.GetChild(listBoxItem, 0);               
                var contentPresenter = VisualTreeHelper.GetChild(grid, 1);              
                var childOfContentPresenter = VisualTreeHelper.GetChild(contentPresenter, 0);
                var borderInner = VisualTreeHelper.GetChild(childOfContentPresenter, 0);
                var text = VisualTreeHelper.GetChild(childOfContentPresenter, 1);
                if ((text as TextBlock).Text.Equals((list.SelectedItem as VideoFilter).filterValue))
                {
                    (text as TextBlock).Foreground = new SolidColorBrush(Colors.White);
                    (borderInner as Border).Background = new SolidColorBrush(Color.FromArgb(255, 19, 86, 113));
                }
                else
                {
                    (text as TextBlock).Foreground = new SolidColorBrush(Color.FromArgb(255, 230, 230, 230));
                    (borderInner as Border).Background = new SolidColorBrush(Color.FromArgb(255, 31, 32, 36));
                }
            }                  
        }
        private object GetListBoxItemPanel(ListBox list)
        {
            var border = VisualTreeHelper.GetChild(list, 0);//only one child type border
            var scrollViwer = VisualTreeHelper.GetChild(border, 0);//only one child type ScrollViwer
            var childOfScrollViewr = VisualTreeHelper.GetChild(scrollViwer, 0);//only one child type border
            var grid = VisualTreeHelper.GetChild(childOfScrollViewr, 0);//only one child Grid
            int childCount = VisualTreeHelper.GetChildrenCount(grid);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(grid, i);
                if ((typeof(ScrollContentPresenter)).ToString().Equals(child.ToString()))
                {
                    var childOfPresent = VisualTreeHelper.GetChild(child, 0);
                    int count1 = VisualTreeHelper.GetChildrenCount(childOfPresent);
                    for (int k = 0; k < count1; k++)
                    {
                        var contentControl = VisualTreeHelper.GetChild(childOfPresent, k);
                        if (contentControl.ToString().Equals((typeof(StackPanel)).ToString()))
                        {
                            return contentControl;
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            return null;
        }
        private void InitSitcoms()
        {
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Mobile"))//Windows.Desktop
            {
                videoInfoPicWidth = (Window.Current.Bounds.Width - 50) / 2;
                videoInfoPicHeight = videoInfoPicWidth * 102 / 180;
            }
            ObservableCollection<SitcomsOverview> sitcoms = new ObservableCollection<SitcomsOverview>();
            for (int i = 0; i < 10; i++)
            {
                SitcomsOverview info = new SitcomsOverview();
                double count = 2.5;
                Random indexRandomizer = new Random(i);
                count += indexRandomizer.Next(0, 20) * 0.1;
                info.playCount = Convert.ToString(count) + "万";
                info.sitcomsId = indexRandomizer.Next(0, 2);
                info.picHeight = videoInfoPicHeight;
                info.picWidth = videoInfoPicWidth;
                info.sitcomsName = string.Format("电视剧名称{0}", i + 1);
                info.picURL = string.Format("/Assets/SitcomsImages/sitcoms{0}.jpg", i + 1);//"/Assets/FilmImages/sitcom.png";
                sitcoms.Add(info);
            }
            sitcomsGrid.ItemsSource = sitcoms;
        }
        private void SitcomsButton_Click(object sender, RoutedEventArgs e)
        {
            string id = (sender as Button).Name;
            sitcomsId = Convert.ToInt32(id);
            videoPlayerPage.Visibility = Visibility.Visible;
            filterPage.Visibility = Visibility.Collapsed;
            InitPlayer();
            InitPageData();
        }
        #endregion
    }
}
