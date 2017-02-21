using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib;
using System.ComponentModel;
using System.Windows;
using map2agblib.Map;
using map2agblib.String;

namespace map2agbgui.Models
{
    public class MainModel : INotifyPropertyChanged
    {

        private List<NumericDisplayTuple<BankModel>> _banks;
        public List<NumericDisplayTuple<BankModel>> Banks
        {
            get
            {
                return _banks;
            }
            set
            {
                _banks = value;
                RaisePropertyChanged("Banks");
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                RaisePropertyChanged("Status");
            }
        }

        public MainModel()
        {
#if DEBUG
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                PopulateDesignerData();
                return;
            }
#endif
            _banks = new List<NumericDisplayTuple<BankModel>>();
        }

        public void PopulateDesignerData()
        {
            Banks = new List<NumericDisplayTuple<BankModel>>()
            {
                new NumericDisplayTuple<BankModel>(0, new BankModel(new List<NumericDisplayTuple<MapModel>>() {
                    new NumericDisplayTuple<MapModel>(0, new MapModel("BLABLA")),
                    new NumericDisplayTuple<MapModel>(1, new MapModel("BLABLA")),
                    new NumericDisplayTuple<MapModel>(2, new MapModel("OKTEST"))
                })),
                new NumericDisplayTuple<BankModel>(1, new BankModel(new List<NumericDisplayTuple<MapModel>>() {
                    new NumericDisplayTuple<MapModel>(0, new MapModel("DEINMUM")),
                    new NumericDisplayTuple< MapModel>(1, new MapModel("NOCHNTEST")),
                    new NumericDisplayTuple<MapModel>(2, new MapModel("ALLESKLAR"))
                }))
            };
            Status = "Data loaded";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public MainModel GetCopy()
        {
            MainModel copy = (MainModel)this.MemberwiseClone();
            return copy;
        }

    }

}
