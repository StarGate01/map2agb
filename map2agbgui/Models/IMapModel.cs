using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbgui.Models
{

    public enum MapEntryType { Map, Nullpointer }

    public interface IMapModel : ITupelFormattable
    {

        MapEntryType Mode { get; }

        Uri IconPath { get; }

    }

}
