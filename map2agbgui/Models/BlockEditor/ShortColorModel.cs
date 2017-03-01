using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using map2agblib.Imaging;
using System.Windows.Media;

namespace map2agbgui.Models.BlockEditor
{

    public class ShortColorModel: IRomSerializable<ShortColorModel, ShortColor>, INotifyPropertyChanged
    {

        #region Properties

        private byte _red, _green, _blue;
        public byte Red
        {
            get
            {
                return _red;
            }
            set
            {
                _red = value;
                RaisePropertyChanged("Red");
                RaisePropertyChanged("Brush");
            }
        }
        public byte Green
        {
            get
            {
                return _green;
            }
            set
            {
                _green = value;
                RaisePropertyChanged("Green");
                RaisePropertyChanged("Brush");
            }
        }
        public byte Blue
        {
            get
            {
                return _blue;
            }
            set
            {
                _blue = value;
                RaisePropertyChanged("Blue");
                RaisePropertyChanged("Brush");
            }
        }

        public SolidColorBrush Brush
        {
            get
            {
                return new SolidColorBrush(Color.FromArgb(255, (byte)(_red << 3), (byte)(_green << 3), (byte)(_blue << 3)));
            }
        }

        #endregion

        #region Constructor

        public ShortColorModel(ShortColor color) : base(color)
        {
            _red = color.Red;
            _green = color.Green;
            _blue = color.Blue;
        }

        #endregion

        #region Methods

        public override ShortColor ToRomData()
        {
            return new ShortColor(_red, _green, _blue);
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }

}
