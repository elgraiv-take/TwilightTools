using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Elgraiv.TwilightTools.Mvvm
{
    public abstract class BindableBase : INotifyPropertyChanged
    {

        protected bool SetProperty<T>(ref T storage,T value, [CallerMemberName] string name = "")
        {
            if (!Equals(storage, value))
            {
                storage = value;
                RaisePropertyChanged(name);
                return true;
            }
            return false;
        }

        protected void RaisePropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
