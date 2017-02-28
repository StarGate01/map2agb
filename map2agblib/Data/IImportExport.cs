using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agblib.Data
{

    public interface IImportExport<T>
    {

        T ImportFromFile(string filePath);
    
        void ExportToFile(T data, string filePath);

    }

}
