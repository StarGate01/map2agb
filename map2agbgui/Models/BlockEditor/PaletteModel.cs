﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using map2agblib.Imaging;
using System.Collections.ObjectModel;
using map2agbgui.Models.Main;
using System.Windows;

namespace map2agbgui.Models.BlockEditor
{

    public class PaletteModel : INotifyPropertyChanged, ITupleFormattable
    {

        #region Properties
        public string FormatString
        {
            get
            {
                return "{1} {0}";
            }
        }

        private ObservableCollection<ShortColorModel> _colors;
        public ObservableCollection<ShortColorModel> Colors
        {
            get
            {
                return _colors;
            }
            set
            {
                _colors = value;
                RaisePropertyChanged("Colors");
            }
        }

        #endregion

        #region Constructor

        public PaletteModel(Palette palette)
        {
            _colors = new ObservableCollection<ShortColorModel>(palette.Colors.Select(p => new ShortColorModel(p)));
        }

        public PaletteModel() : this((MockData.MockRomData()).Tilesets.First().Value.Data.Palettes.First())
        {
            if (!(bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue))
                throw new InvalidOperationException("NSEditorModel can only be constructed without parameters by the designer");
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return "Palette";
        }

        public Palette ToRomData()
        {
            return new Palette(_colors.Select(p => p.ToRomData()).ToArray());
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public PaletteModel GetCopy()
        {
            PaletteModel copy = (PaletteModel)this.MemberwiseClone();
            return copy;
        }

        #endregion

    }

}