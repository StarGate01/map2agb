using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbgui.Models
{

    public abstract class IRomSerializable<T, R> where T : IRomSerializable<T, R>
    {

        protected IRomSerializable(R romData) { }

        public object Clone(params object[] additionalConstructorArgs)
        {
            List<object> newargs = additionalConstructorArgs.ToList();
            newargs.Insert(0, ToRomData());
            return (T)Activator.CreateInstance(typeof(T), newargs.ToArray());
        }

        public abstract R ToRomData();

    }

}
