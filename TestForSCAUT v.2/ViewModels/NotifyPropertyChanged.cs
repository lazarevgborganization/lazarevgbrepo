using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForSCAUT_v_2.ViewModels
{
    /// <summary>
    /// Реализует интерфейс INotifyPropertyChanged
    /// </summary>
    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        //Ниже представлена реализация интерфейса с оберткой вызова события в lock (не является потокобезопасным)

        //public event PropertyChangedEventHandler PropertyChanged;
        //PropertyChangedEventHandler handler;

        //protected void OnPropertyChanged(string propertyName)
        //{
        //    lock(this)
        //    {
        //        handler = PropertyChanged;

        //        if (handler != null)
        //        {
        //            handler(this, new PropertyChangedEventArgs(propertyName));
        //        }
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
