using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace map2agbgui.Extensions
{

    [AttributeUsage(AttributeTargets.Property,  AllowMultiple = true, Inherited = false)]
    public class CollectionItemPropertyDependency : Attribute
    {

        public string PropertyName { get; private set; }
        public string[] TriggerItemDependency { get; private set; }
        public string[] Dependency { get; private set; }

        public CollectionItemPropertyDependency(string triggerItemDependency, string[] dependency, [CallerMemberName] string propertyName = null)
         : this(new string[] { triggerItemDependency }, dependency, propertyName) { }
        public CollectionItemPropertyDependency(string[] triggerItemDependency, string dependency, [CallerMemberName] string propertyName = null)
          : this(triggerItemDependency, new string[] { dependency }, propertyName) { }
        public CollectionItemPropertyDependency(string triggerItemDependency, string dependency, [CallerMemberName] string propertyName = null)
           : this(new string[] { triggerItemDependency }, new string[] { dependency }, propertyName) { }
        public CollectionItemPropertyDependency(string[] triggerItemDependency, string[] dependency, [CallerMemberName] string propertyName = null)
        {
            PropertyName = propertyName;
            TriggerItemDependency = triggerItemDependency;
            Dependency = dependency;
        }

    }

}
