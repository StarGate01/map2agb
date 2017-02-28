using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace map2agbgui.Models.Main.Maps
{

    public enum MapEntryType { Map, Nullpointer }

    public interface IMapModel : ITupleFormattable
    {

        bool IsSelected { get; set; }

        MapEntryType EntryMode { get; }

        BankModel Bank { get; }

    }

}