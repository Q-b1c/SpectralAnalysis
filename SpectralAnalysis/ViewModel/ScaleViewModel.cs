using Microsoft.Win32;
using SpectralAnalysis.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ViewModelBase.Commands.QuickCommands;

namespace SpectralAnalysis
{
    internal class ScaleViewModel : INotifyPropertyChanged
    {
        public ScaleViewModel()
        {
            scaleImageToMinCommand = new Command(ExecuteScalateToMinImage);
            scaleImageToMaxCommand = new Command(ExecuteScalateToMaxImage);
            closeWinCommand = new Command(ExecuteCloseWin);
            openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Image files (*.TIF)|*.tif",
                Title = "Выберите изображение"
            };
        }

        private ICommand closeWinCommand;
        public ICommand CloseWinCommand { get { return closeWinCommand; } }

        private ICommand scaleImageToMinCommand;
        public ICommand ScaleImageToMinCommand { get { return scaleImageToMinCommand; } }

        private ICommand scaleImageToMaxCommand;
        public ICommand ScaleImageToMaxCommand { get { return scaleImageToMaxCommand; } }

        readonly OpenFileDialog openFileDialog;


        void ExecuteCloseWin()
        {
            var window = System.Windows.Application.Current.Windows.OfType<ScaleWindow>().FirstOrDefault();
            if (window != null) { window.Close(); }
        }

        void ExecuteScalateToMinImage()
        {
            string path = "";
            if (openFileDialog.ShowDialog() == true) 
            {
                path = openFileDialog.FileName;
            }
            if (path != "") 
            { 
                Image image = Image.FromFile(path);
                double width = image.Width * 0.5;
                double height = image.Height * 0.5;
                Bitmap newImage = new Bitmap(Convert.ToInt32(width), Convert.ToInt32(height));
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(image, new Rectangle(0, 0, Convert.ToInt32(width), Convert.ToInt32(height)));
                }
                newImage.Save(path.Replace(".tif", string.Empty)+"_test.tif");
                MessageBox.Show("Изображение отмасштабировано");
            }
        }

        void ExecuteScalateToMaxImage()
        {
            string path = "";
            if (openFileDialog.ShowDialog() == true)
            {
                path = openFileDialog.FileName;
            }
            if (path != "")
            {
                Image image = Image.FromFile(path);
                double width = image.Width * 2;
                double height = image.Height * 2;
                Bitmap newImage = new Bitmap(Convert.ToInt32(width), Convert.ToInt32(height));
                using (Graphics graphics = Graphics.FromImage(newImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.DrawImage(image, new Rectangle(0, 0, Convert.ToInt32(width), Convert.ToInt32(height)));
                }
                newImage.Save(path.Replace(".tif", string.Empty) + "_test.tif");
                MessageBox.Show("Изображение отмасштабировано");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
    }
}
