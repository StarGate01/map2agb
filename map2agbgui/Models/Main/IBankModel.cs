using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbgui.Models.Main
{

    public enum BankEntryType { Bank, Nullpointer }

    public interface IBankModel : ITupleFormattable
    {

        bool IsSelected { get; set; }

        BankEntryType EntryMode { get; }

    }

}
