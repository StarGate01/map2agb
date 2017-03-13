using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections.Specialized;

namespace map2agbgui.Extensions
{

    public class PropertyDependencyHandler
    {

        #region Private fields

        private IRaisePropertyChanged _caller;
        private Dictionary<string, IEnumerable<string>> propDepAttrs = new Dictionary<string, IEnumerable<string>>();
        private Dictionary<string, Pair<IEnumerable<string>, CollectionChangedClass>> colPropDepAttrs = new Dictionary<string, Pair<IEnumerable<string>, CollectionChangedClass>>();
        private Dictionary<string, Pair<IEnumerable<Pair<IEnumerable<string>, IEnumerable<string>>>, ItemPropertyChangedClass>> colItemPropDepAttrs = new Dictionary<string, Pair<IEnumerable<Pair<IEnumerable<string>, IEnumerable<string>>>, ItemPropertyChangedClass>>();
        private Dictionary<string, Pair<IEnumerable<Pair<IEnumerable<string>, IEnumerable<string>>>, ChildPropertyChangedClass>> colChildPropDepAttrs = new Dictionary<string, Pair<IEnumerable<Pair<IEnumerable<string>, IEnumerable<string>>>, ChildPropertyChangedClass>>();

        #endregion

        #region Event handling classes

        private class CollectionChangedClass
        {

            private PropertyDependencyHandler _parent;
            private string _propertyName;

            public CollectionChangedClass(PropertyDependencyHandler parent, string propertyName)
            {
                _parent = parent;
                _propertyName = propertyName;
            }

            public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                IEnumerable<string> dependency = _parent.colPropDepAttrs[_propertyName].First;
                for (int i=0; i< dependency.Count(); i++)
                    _parent._caller.RaisePropertyChanged(dependency.ElementAt(i));
#if DEBUG
                Debug.WriteLine("PropertyDependencyHandler.CollectionChangedClass: Success: " + _parent._caller.GetType().Name + ", Target: " + string.Join(" ", dependency) + ", Invoker: " + _propertyName);
#endif
            }

        }

        private class ItemPropertyChangedClass
        {

            private PropertyDependencyHandler _parent;
            private string _propertyName;

            public ItemPropertyChangedClass(PropertyDependencyHandler parent, string propertyName)
            {
                _parent = parent;
                _propertyName = propertyName;
            }

            public void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                IEnumerable<Pair<IEnumerable<string>, IEnumerable<string>>> dependency = _parent.colItemPropDepAttrs[_propertyName].First;
                for (int i = 0; i < dependency.Count(); i++)
                    if (dependency.ElementAt(i).First.Contains(e.PropertyName))
                    {
                        for (int k = 0; k < dependency.ElementAt(i).Second.Count(); k++)
                            _parent._caller.RaisePropertyChanged(dependency.ElementAt(i).Second.ElementAt(k));
#if DEBUG
                        Debug.WriteLine("PropertyDependencyHandler.ItemPropertyChangedClass: Success: " + _parent._caller.GetType().Name + ", Target: " + string.Join(" ", dependency.ElementAt(i).Second) + ", Invoker: " + _propertyName);
#endif
                    }
            }

        }

        private class ChildPropertyChangedClass
        {

            private PropertyDependencyHandler _parent;
            private string _propertyName;

            public ChildPropertyChangedClass(PropertyDependencyHandler parent, string propertyName)
            {
                _parent = parent;
                _propertyName = propertyName;
            }

            public void PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                IEnumerable<Pair<IEnumerable<string>, IEnumerable<string>>> dependency = _parent.colChildPropDepAttrs[_propertyName].First;
                for (int i = 0; i < dependency.Count(); i++)
                    if (dependency.ElementAt(i).First.Contains(e.PropertyName))
                    {
                        for (int k = 0; k < dependency.ElementAt(i).Second.Count(); k++)
                            _parent._caller.RaisePropertyChanged(dependency.ElementAt(i).Second.ElementAt(k));
#if DEBUG
                        Debug.WriteLine("PropertyDependencyHandler.ChildPropertyChangedClass: Success: " + _parent._caller.GetType().Name + ", Target: " + string.Join(" ", dependency.ElementAt(i).Second) + ", Invoker: " + _propertyName);
#endif
                    }
            }

        }

        #endregion

        #region Constructor

        public PropertyDependencyHandler(IRaisePropertyChanged caller)
        {
            _caller = caller;
            _caller.PropertyChanged += Caller_PropertyChanged;
            foreach(PropertyInfo property in caller.GetType().GetProperties())
            {
                PropertyDependency[] pDep = (PropertyDependency[])property.GetCustomAttributes(typeof(PropertyDependency));
                if (pDep.Length != 0) propDepAttrs[property.Name] = 
                        pDep.SelectMany(p => p.Dependency);
                CollectionPropertyDependency[] cPDep = (CollectionPropertyDependency[])property.GetCustomAttributes(typeof(CollectionPropertyDependency));
                if (cPDep.Length != 0) colPropDepAttrs[property.Name] = 
                        new Pair<IEnumerable<string>, CollectionChangedClass> (cPDep.SelectMany(p => p.Dependency), null);
                CollectionItemPropertyDependency[] cIPDep = (CollectionItemPropertyDependency[])property.GetCustomAttributes(typeof(CollectionItemPropertyDependency));
                if (cIPDep.Length != 0) colItemPropDepAttrs[property.Name] = 
                        new Pair<IEnumerable<Pair<IEnumerable<string>, IEnumerable<string>>>, ItemPropertyChangedClass>
                        (cIPDep.Select(p => new Pair<IEnumerable<string>, IEnumerable<string>>(p.TriggerItemDependency, p.Dependency)), null);
                ChildPropertyDependency[] cCPDep = (ChildPropertyDependency[])property.GetCustomAttributes(typeof(ChildPropertyDependency));
                if (cCPDep.Length != 0) colChildPropDepAttrs[property.Name] = 
                        new Pair<IEnumerable<Pair<IEnumerable<string>, IEnumerable<string>>>, ChildPropertyChangedClass>
                        (cCPDep.Select(p => new Pair<IEnumerable<string>, IEnumerable<string>>(p.TriggerChildDependency, p.Dependency)), null);
                Caller_PropertyChangedInner(this, new PropertyChangedEventArgs(property.Name), true);
            }
        }

        #endregion

        #region Methods

        private void Caller_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Caller_PropertyChangedInner(sender, e, false);
        }

        #endregion

        #region Events

        private void Caller_PropertyChangedInner(object sender, PropertyChangedEventArgs e, bool bindOnly)
        {
            //Get property
            PropertyInfo property = null;
            object root = _caller;
            string[] propPath = e.PropertyName.Split('.');
            for (int i = 0; i < propPath.Length; i++)
            {
                if (property == null) property = _caller.GetType().GetProperty(propPath[i]);
                else
                {
                    root = property.GetValue(root);
                    property = root.GetType().GetProperty(propPath[i]);
                }
            }
            if (property == null) throw new EntryPointNotFoundException(e.PropertyName);

            //Handle attribute bindings
            if (!bindOnly && propDepAttrs.ContainsKey(e.PropertyName))
            {
                IEnumerable<string> dependency = propDepAttrs[e.PropertyName];
                for (int j = 0; j < dependency.Count(); j++)
                    _caller.RaisePropertyChanged(dependency.ElementAt(j));
#if DEBUG
                Debug.WriteLine("PropertyDependencyHandler: Success: " + _caller.GetType().Name + ", Target: " + string.Join(" ", dependency) + ", Invoker: " + e.PropertyName);
#endif
            }
            if (colPropDepAttrs.ContainsKey(e.PropertyName))
            {
                object value = property.GetValue(_caller);
                if (colPropDepAttrs[e.PropertyName].Second != null) ((INotifyCollectionChanged)value).CollectionChanged -= colPropDepAttrs[e.PropertyName].Second.CollectionChanged;
                colPropDepAttrs[e.PropertyName].Second = new CollectionChangedClass(this, e.PropertyName);
                ((INotifyCollectionChanged)value).CollectionChanged += colPropDepAttrs[e.PropertyName].Second.CollectionChanged;
            }
            if (colItemPropDepAttrs.ContainsKey(e.PropertyName))
            {
                object value = property.GetValue(_caller);
                if (colItemPropDepAttrs[e.PropertyName].Second != null) ((IItemPropertyChanged)value).ItemPropertyChanged -= colItemPropDepAttrs[e.PropertyName].Second.ItemPropertyChanged;
                colItemPropDepAttrs[e.PropertyName].Second = new ItemPropertyChangedClass(this, e.PropertyName);
                ((IItemPropertyChanged)value).ItemPropertyChanged += colItemPropDepAttrs[e.PropertyName].Second.ItemPropertyChanged;
            }
            if (colChildPropDepAttrs.ContainsKey(e.PropertyName))
            {
                object value = property.GetValue(_caller);
                if (colChildPropDepAttrs[e.PropertyName].Second != null) ((INotifyPropertyChanged)value).PropertyChanged -= colChildPropDepAttrs[e.PropertyName].Second.PropertyChanged;
                colChildPropDepAttrs[e.PropertyName].Second = new ChildPropertyChangedClass(this, e.PropertyName);
                ((INotifyPropertyChanged)value).PropertyChanged += colChildPropDepAttrs[e.PropertyName].Second.PropertyChanged;
            }
        }

        #endregion

    }

}
