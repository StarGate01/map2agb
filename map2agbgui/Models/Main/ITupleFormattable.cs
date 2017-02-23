using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbgui.Models.Main
{

    public interface ITupleFormattable : INotifyPropertyChanged
    {
        string FormatString { get; }
    
    }

}
