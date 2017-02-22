using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using map2agblib;
using System.ComponentModel;
using System.Windows;
using map2agblib.Data;
using map2agblib.Map;

namespace map2agbgui.Models
{
    public class MainModel : INotifyPropertyChanged
    {

        private RomData _data;
        public RomData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                RaisePropertyChanged("Data");
                RaisePropertyChanged("Banks");
            }
        }


        private List<NumericDisplayTuple<BankModel>> _banks;
        public List<NumericDisplayTuple<BankModel>> Banks
        {
            get
            {
                return _data.Banks.Select((p, pi) => 
                    new NumericDisplayTuple<BankModel>(pi, new BankModel(p.Select((k, ki) =>
                        new NumericDisplayTuple<IMapModel>(ki, ((k != null)? (IMapModel)(new MapModel(k, _data)): (new NullpointerMapModel())))).ToList()))).ToList();
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
            _data = new RomData();
#if DEBUG
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
            {
                PopulateDesignerData();
            }
#endif
        }

        public MainModel(RomData romData)
        {
            _data = romData;
        }
        
        public void PopulateDesignerData()
        {
            _data.Banks.Add(new List<MapHeader>() { new MapHeader(), null, new MapHeader(), new MapHeader() });
            _data.Banks.Add(new List<MapHeader>() { new MapHeader(), new MapHeader(), new MapHeader(), null });
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
