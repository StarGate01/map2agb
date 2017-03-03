using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbgui.Extensions
{

    public interface IRaiseItemPropertyChanged : INotifyPropertyChanged
    {

        void RaiseItemPropertyChanged(object sender, PropertyChangedEventArgs e);

    }

}
