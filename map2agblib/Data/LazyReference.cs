﻿using System;
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

        public LazyReference(string absolutePath, T data) : this(data)
        {
            AbsolutePath = absolutePath;
        }

        public LazyReference(T data)
        {
            _data = data;
            dataLoaded = true;
        }

        #endregion

        #region Methods

        public void ExportToFile(string otherDirName = null)
        {
            if (AbsolutePath == null) throw new FileNotFoundException();
            Data.ExportToFile(Data, (otherDirName == null)? AbsolutePath : otherDirName);
        }

        #endregion

    }

}
