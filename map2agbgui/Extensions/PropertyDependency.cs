using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace map2agbgui.Extensions
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PropertyDependency : Attribute
    {

        public string PropertyName { get; private set; }
        public string[] Dependency { get; private set; }

        public PropertyDependency(string[] dependency, [CallerMemberName] string propertyName = null)
        {
            PropertyName = propertyName;
            Dependency = dependency;
        }

    }

}
