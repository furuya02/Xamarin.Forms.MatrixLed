using Xamarin.Forms;

namespace MatrixLed
{
    public class Led:BoxView{

        bool _sw;

        public bool Sw {
            set {
                _sw = value;
                Color = _sw ? Color.FromRgba(224, 33, 0, 255) : Color.FromRgba(255, 100, 100, 100);
                OnPropertyChanged(propertyName: "Color");
            }
            get { return _sw; }
        }

        public Led() {
            WidthRequest = 45;
            HeightRequest = 45;
            Sw = false;
        }

    }
}
