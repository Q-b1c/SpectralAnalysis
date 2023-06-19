using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.ViewModelBase.Commands
{
    public interface IWindowService
    {
        public void ShowWindow<T>(object dataContext) where T : Window, new();
    }
}
