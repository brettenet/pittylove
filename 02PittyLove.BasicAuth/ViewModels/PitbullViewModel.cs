using System;
using System.Linq;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using _02PittyLove.WinRT2.Model;

namespace _02PittyLove.WinRT2.ViewModels
{
    public class PitbullViewModel : ViewModel
    {
        #region Fields

        private readonly Uri _baseUri;
        private int _uniqueId;
        private string _name = string.Empty;
        private string _description = string.Empty;
        private ImageSource _image = null;
        private String _imagePath = null;

        public PitbullViewModel(Pitbull pitbull)
        {
            _baseUri = new Uri("ms-appx:///");
            UniqueId = pitbull.Id;
            Name = pitbull.Name;
            ImageUrl = pitbull.ImageUrl;
            Description = pitbull.Description;
            SetImage(pitbull.ImageUrl.Split(new[] { '/' }).ToList().Last().TrimStart('/'));
        }

        #endregion

        #region Properties

        public int UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        public string Name
        {
            get { return this._name; }
            set { this.SetProperty(ref this._name, value); }
        }

        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(_baseUri, string.Concat("Assets/", _imagePath)));
                }
                return this._image;
            }
            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public string ImageUrl { get; set; }

        #endregion

        #region Methods

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        #endregion
    }
}
