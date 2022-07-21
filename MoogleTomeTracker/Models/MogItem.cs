using MoogleTomeTracker.Library;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

#nullable disable
namespace MoogleTomeTracker.Models
{
    public partial class MogItem : ObservableObject
    {
        [ObservableProperty]
        private int _id;
        [ObservableProperty]
        private string _name;
        [ObservableProperty]
        private int _cost;
        [ObservableProperty]
        private string _base64Image;
        [ObservableProperty]
        private bool _isTracked;
        [ObservableProperty]
        private bool _isAcquired;

        [JsonIgnore]
        public BitmapSource Image { get => ImageHelper.BitmapFromBase64(Base64Image); }
    }
}
