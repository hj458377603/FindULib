using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace FindULib.Common
{
    public class BaseCommand : ICommand, INotifyPropertyChanged
    {
        private bool canExecute = false;
        private Action handler;

        public BaseCommand(Action handler)
        {
            this.handler = handler;
        }

        public bool CanExecute(object parameter)
        {
            //throw new NotImplementedException();
            canExecute = true;
            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (canExecute)
            {
                handler();
            }
            //throw new NotImplementedException();
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
