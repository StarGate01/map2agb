using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace map2agbgui.Models.Main
{

    public enum MapEntryType { Map, Nullpointer }

    public interface IMapModel : ITupleFormattable
    {

        bool IsSelected { get; set; }

        MapEntryType EntryMode { get; }

        Uri IconPath { get; }

        BankModel Bank { get; }

    }

}