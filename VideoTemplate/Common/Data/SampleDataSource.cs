using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace VideoTemplate.Common.Data
{
    /// <summary>
    /// Implementation of <see cref="INotifyPropertyChanged"/> to simplify models.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Multicast event for property change notifications.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Checks if a property already matches a desired value.  Sets the property and
        /// notifies listeners only when necessary.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="storage">Reference to a property with both getter and setter.</param>
        /// <param name="value">Desired value for the property.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers that
        /// support CallerMemberName.</param>
        /// <returns>True if the value was changed, false if the existing value matched the
        /// desired value.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String title, String type, String picture)
        {
            this._title = title;
            this._type = type;
            this._picture = picture;           
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _type = string.Empty;
        public string Type
        {
            get { return this._type; }
            set { this.SetProperty(ref this._type, value); }
        }

        private Uri _image = null;
        private String _picture = null;
        public Uri Image
        {
            get
            {
                return new Uri(SampleDataCommon._baseUri, this._picture);
            }

            set
            {
                this._picture = null;
                this.SetProperty(ref this._image, value);
            }
        }      
    }

    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String title, String type, String picture)
            : base(title, type, picture)
        {
        }

    }
    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private ObservableCollection<object> _items = new ObservableCollection<object>();
        public ObservableCollection<object> Items
        {
            get { return this._items; }
        }

        public SampleDataSource()
        {
            //Items.Add(new SampleDataItem("Cliff",
            //        "item",
            //        "Assets/Cliff.jpg"
            //        ));
            //Items.Add(new SampleDataItem("Grapes",
            //        "item",
            //        "Assets/Grapes.jpg"
            //        ));
            //Items.Add(new SampleDataItem("Rainier",
            //        "item",
            //        "Assets/Rainier.jpg"
            //        ));
            //Items.Add(new SampleDataItem("Sunset",
            //        "item",
            //        "Assets/Sunset.jpg"
            //        ));
            //Items.Add(new SampleDataItem("Valley",
            //        "item",
            //        "Assets/Valley.jpg"
            //        ));
            Items.Add(new SampleDataItem("",
        "item",
        "Assets/FilmImages/film1.jpg"
        ));
            Items.Add(new SampleDataItem("",
                    "item",
                    "Assets/FilmImages/film2.jpg"
                    ));
            Items.Add(new SampleDataItem("",
                    "item",
                    "Assets/FilmImages/film3.jpg"
                    ));
            Items.Add(new SampleDataItem("",
                    "item",
                    "Assets/FilmImages/film4.jpg"
                    ));
            Items.Add(new SampleDataItem("",
                    "item",
                    "Assets/FilmImages/film5.jpg"
                    ));
            Items.Insert(0,Items[Items.Count-1]);
            Items.Add(Items[1]);
        }
    }
}
