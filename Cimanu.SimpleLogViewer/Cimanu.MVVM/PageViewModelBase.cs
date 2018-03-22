using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cimanu.MVVM
{
    /// <summary>
    /// ViewModels base class
    /// </summary>
    public abstract class PageViewModelBase : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Notifies about property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public PageViewModelBase(params object[] args)
        {

        }

        /// <summary>
        /// Notifies about specified property changed
        /// </summary>
        /// <param name="property">Property name</param>
        protected void RaisePropertyChanged(String property)
        {
            var x = PropertyChanged;
            x?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Notifies about caller property changed - no need to specify property name
        /// </summary>
        /// <param name="property">Caller property name</param>
        protected void RaiseSelfChanged([CallerMemberName]String property = null)
        {
            var x = PropertyChanged;
            x?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Raises PropertyChanged for all properties in viewmodel's type
        /// </summary>
        protected void RaisePropertyChangedForAll()
        {
            var x = PropertyChanged;
            if (x != null)
                foreach (var prop in this.GetType().GetProperties(
                    BindingFlags.Public | BindingFlags.Instance))
                    x(this, new PropertyChangedEventArgs(prop.Name));
        }

        /// <summary>
        /// Disposes Viewmodel
        /// </summary>
        public virtual void Dispose() { }
        
    }
}
