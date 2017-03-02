using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.IO;

namespace map2agblib.Data
{

    [DataContract]
    public class LazyReference<T> where T : IImportExport<T>, new()
    {

        #region Private variables

        private bool dataLoaded = false;

        #endregion

        #region Properties

        [DataMember]
        public string AbsolutePath { get; set; } = null;

        private T _data;
        [IgnoreDataMember]
        public T Data
        {
            get
            {
                if(!dataLoaded)
                {
                    _data = (new T()).ImportFromFile(AbsolutePath);
                    dataLoaded = true;
                }
                return _data;
            }
        }

        #endregion

        #region Constructors

        public LazyReference()
        {
        }

        public LazyReference(string absolutePath)
        {
            AbsolutePath = absolutePath;
        }

        public LazyReference(string absolutePath, T data)
        {
            AbsolutePath = absolutePath;
            _data = data;
            dataLoaded = true;
        }

        public LazyReference(T data)
        {
            _data = data;
            dataLoaded = true;
        }

        #endregion

        #region Methods

        public void ExportToFile(string otherDirName)
        {
            if (AbsolutePath == null) throw new FileNotFoundException();
            if (!dataLoaded) File.Copy(AbsolutePath, otherDirName);
            else Data.ExportToFile(Data, otherDirName);
        }

        #endregion

    }

}
