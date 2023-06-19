using Microsoft.Win32;
using OSGeo.GDAL;
using OSGeo.OGR;
using OSGeo.OSR;
using SpectralAnalysis;
using SpectralAnalysis.View;
//using Project.View;
//using Project.ViewModel;
//using Project.ViewModel.Commands;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelBase.Commands.QuickCommands;

namespace SpectralAnalysis
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ViewModel()
        {
            
            showPicCommand = new Command(ExecuteShowPicWindow);
            closeAppCommand = new Command(ExecuteCloseApp);
            //openFileDialogCommand = new Command(ExecuteOpenFileDialog);
            //openFileDialog = new OpenFileDialog()
            //{
            //    Multiselect = true,
            //    Filter = "Image files (*.TIF)|*.tif",
            //    Title = "Выберите изображение"
            //};
        }

        readonly ICommand closeAppCommand;

        public ICommand CloseAppCommand { get { return closeAppCommand; } }

        public ImageSource Image { get; private set; }

        readonly ICommand showPicCommand;

        public ICommand ShowPicCommand { get { return showPicCommand; } }

        void ExecuteShowPicWindow()
        {
            PicWindow picwin = new();
            picwin.Show();
            Application.Current.MainWindow.Hide();
        }

        void ExecuteCloseApp()
        {
            Application.Current.MainWindow.Close();
        }

        protected virtual void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
    }
}
