using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace map2agbgui.Extensions
{

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class ChildPropertyDependency : Attribute
    {

        public string PropertyName { get; private set; }
        public string[] TriggerChildDependency { get; private set; }
        public string[] Dependency { get; private set; }

        public ChildPropertyDependency(string triggerChildDependency, string[] dependency, [CallerMemberName] string propertyName = null)
         : this(new string[] { triggerChildDependency }, dependency, propertyName) { }
        public ChildPropertyDependency(string[] triggerChildDependency, string dependency, [CallerMemberName] string propertyName = null)
          : this(triggerChildDependency, new string[] { dependency }, propertyName) { }
        public ChildPropertyDependency(string triggerChildDependency, string dependency, [CallerMemberName] string propertyName = null)
           : this(new string[] { triggerChildDependency }, new string[] { dependency }, propertyName) { }
        public ChildPropertyDependency(string[] triggerChildDependency, string[] dependency, [CallerMemberName] string propertyName = null)
        {
            PropertyName = propertyName;
            TriggerChildDependency = triggerChildDependency;
            Dependency = dependency;
        }

    }

}
