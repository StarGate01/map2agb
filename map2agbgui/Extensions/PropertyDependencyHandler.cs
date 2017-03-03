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

    public static class PropertyDependencyHandler
    {

        public static void Handle(IRaisePropertyChanged caller)
        {
            caller.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                try
                {
                    //Subhandlers
                    Dictionary<string, NotifyCollectionChangedEventHandler> notifyCollectionChangedEventHandlers = new Dictionary<string, NotifyCollectionChangedEventHandler>();
                    Dictionary<string, PropertyChangedEventHandler> itemPropertyChangedEventHandlers = new Dictionary<string, PropertyChangedEventHandler>();

                    //Get property
                    PropertyInfo property = caller.GetType().GetProperty(e.PropertyName);
                    if (property == null) throw new EntryPointNotFoundException(e.PropertyName);

                    //Get attributes of property
                    PropertyDependency[] propDepAttr = (PropertyDependency[])Attribute.GetCustomAttributes(property, typeof(PropertyDependency));
                    CollectionPropertyDependency[] colPropDepAttr = (CollectionPropertyDependency[])Attribute.GetCustomAttributes(property, typeof(CollectionPropertyDependency));
                    CollectionItemPropertyDependency[] colItemPropDepAttr = (CollectionItemPropertyDependency[])Attribute.GetCustomAttributes(property, typeof(CollectionItemPropertyDependency));

                    //Handle attribute bindings
                    for (int i = 0; i < propDepAttr.Length; i++)
                    {
                        if (propDepAttr[i].PropertyName == e.PropertyName)
                        {
                            for(int j=0; j<propDepAttr[i].Dependency.Length; j++) caller.RaisePropertyChanged(propDepAttr[i].Dependency[j]);
                        }
                    }
                    for (int i = 0; i < colPropDepAttr.Length; i++)
                    {
                        if (colPropDepAttr[i].PropertyName == e.PropertyName)
                        {
                            ObservableCollection<INotifyPropertyChanged> value = (ObservableCollection<INotifyPropertyChanged>)property.GetValue(caller);
                            if(!notifyCollectionChangedEventHandlers.ContainsKey(e.PropertyName))
                            {
                                notifyCollectionChangedEventHandlers[e.PropertyName] = new NotifyCollectionChangedEventHandler((object listSender, NotifyCollectionChangedEventArgs listE) =>
                                {
                                    for (int j = 0; j < colPropDepAttr[i].Dependency.Length; j++) caller.RaisePropertyChanged(colPropDepAttr[i].Dependency[j]);
                                });
                            }
                            value.CollectionChanged -= notifyCollectionChangedEventHandlers[e.PropertyName];
                            value.CollectionChanged += notifyCollectionChangedEventHandlers[e.PropertyName];
                        }
                    }
                    for (int i = 0; i < colItemPropDepAttr.Length; i++)
                    {
                        if (colItemPropDepAttr[i].PropertyName == e.PropertyName)
                        {
                            ObservableCollectionEx<INotifyPropertyChanged> value = (ObservableCollectionEx<INotifyPropertyChanged>)property.GetValue(caller);
                            if (!itemPropertyChangedEventHandlers.ContainsKey(e.PropertyName))
                            {
                                itemPropertyChangedEventHandlers[e.PropertyName] = new PropertyChangedEventHandler((object listSender, PropertyChangedEventArgs listE) =>
                                {
                                    if (listE.PropertyName == colItemPropDepAttr[i].TriggerItemDependency)
                                    {
                                        for (int j = 0; j < colItemPropDepAttr[i].Dependency.Length; j++) caller.RaisePropertyChanged(colItemPropDepAttr[i].Dependency[j]);
                                    }
                                });
                            }
                            value.ItemPropertyChanged -= itemPropertyChangedEventHandlers[e.PropertyName];
                            value.ItemPropertyChanged += itemPropertyChangedEventHandlers[e.PropertyName];
                        }
                    }
                }
                catch (EntryPointNotFoundException ex)
                {
#if DEBUG
                    Debug.WriteLine("PropertyDependencyHandler: Property not found: " + ex.Message + ", Invoker: " + e.PropertyName);
#endif
                }
            };
        }

    }

}
