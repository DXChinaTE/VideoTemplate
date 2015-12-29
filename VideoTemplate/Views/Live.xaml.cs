using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Casting;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VideoTemplate.Common;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace VideoTemplate.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Live : Page
    {
        private CastingDevicePicker picker = null;
        private VideoMetaData video = null;
        private CastingConnection connection;
        public Live()
        {
            this.InitializeComponent();
            player.MediaOpened += Player_MediaOpened;
            player.MediaFailed += Player_MediaFailed;
            player.CurrentStateChanged += Player_CurrentStateChanged;

            // Get an Azure hosted video
            VideoDataProvider dataProvider = new VideoDataProvider();
            video = dataProvider.GetRandomVideo();

            //Set the source on the player           
            player.Source = video.VideoLink;

            //Subscribe for the clicked event on the custom cast button
            ((MediaTransportControlsWithCustomCastButton)this.player.TransportControls).CastButtonClicked += TransportControls_CastButtonClicked;

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

        private void TransportControls_CastButtonClicked(object sender, EventArgs e)
        {
            //rootPage.NotifyUser("Custom Cast Button Clicked", NotifyType.StatusMessage);

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
                    //rootPage.NotifyUser(string.Format("Picker DeviceSelected event fired for device '{0}'", args.SelectedCastingDevice.FriendlyName), NotifyType.StatusMessage);

                    DateTime t1 = DateTime.Now;
                    DeviceInformation mydevice = await DeviceInformation.CreateFromIdAsync(args.SelectedCastingDevice.Id);
                    DateTime t2 = DateTime.Now;

                    TimeSpan ts = new TimeSpan(t2.Ticks - t1.Ticks);

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
                        //rootPage.NotifyUser(string.Format("Starting casting to '{0}'", args.SelectedCastingDevice.FriendlyName), NotifyType.StatusMessage);
                        CastingConnectionErrorStatus status = await connection.RequestStartCastingAsync(source);

                        if (status == CastingConnectionErrorStatus.Succeeded)
                        {
                            player.Play();
                            //rootPage.NotifyUser(string.Format("Starting casting to '{0}'", args.SelectedCastingDevice.FriendlyName), NotifyType.StatusMessage);
                        }

                    }
                    catch
                    {
                        //rootPage.NotifyUser(string.Format("Failed to get casting source for video '{0}'", video.Title), NotifyType.ErrorMessage);
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
                //rootPage.NotifyUser("Casting Connection State Changed: " + sender.State, NotifyType.StatusMessage);
            });
        }
        private async void Connection_ErrorOccurred(CastingConnection sender, CastingConnectionErrorOccurredEventArgs args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //rootPage.NotifyUser("Connection Error Occured: " + args.Message, NotifyType.ErrorMessage);
            });
        }

        #endregion
        private void Player_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            //rootPage.NotifyUser(string.Format("{0} '{1}'", this.player.CurrentState, video.Title), NotifyType.StatusMessage);
        }
        private void Player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //rootPage.NotifyUser(string.Format("Failed to load '{0}'", video.Title), NotifyType.ErrorMessage);
        }
        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            //rootPage.NotifyUser(string.Format("Openend '{0}'", video.Title), NotifyType.StatusMessage);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {          
            foreach(ListViewItem item in tvShows.Items)
            {
                if(Window.Current.Bounds.Width > 640)
                {
                    (item.Content as Grid).Width = tvShows.ActualWidth;
                }
                else
                {
                    (item.Content as Grid).Width = Window.Current.Bounds.Width;
                }                
            }
            if (Window.Current.Bounds.Width > 640)
            {
                tvShows.Height = Window.Current.Bounds.Height;
            }
        }
    }
}
