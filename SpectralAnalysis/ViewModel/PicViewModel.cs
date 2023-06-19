using Microsoft.Win32;
using OSGeo.GDAL;
using OSGeo.OGR;
using Project;
using SpectralAnalysis.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using ViewModelBase.Commands.QuickCommands;

using static System.Net.Mime.MediaTypeNames;
using Color = System.Drawing.Color;

namespace SpectralAnalysis
{
    internal class PicViewModel : INotifyPropertyChanged
    {

        public PicViewModel()
        {
            showMainWindowCommand = new Command(ExecuteShowMainWindow);
            ValidTextCommand = new Command(ValidateInputText);
            getImagesCommand = new Command(ExecuteOpenFileDialog);
            showScaleWinCommand = new Command(ExecuteShowScaleWindow);
            openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Image files (*.TIF)|*.tif",
                Title = "Выберите изображение"
            };
        }

        private CalculationModel calculationModel;

        public CalculationModel CalculationModel
        {
            get { return calculationModel; }
            set
            {
                calculationModel = value;
                OnPropertyChanged(nameof(CalculationModel));
            }
        }

        readonly ICommand showMainWindowCommand;

        public ICommand ShowMainWindowCommand { get { return showMainWindowCommand; } }

        public ICommand ValidTextCommand { get; private set; }

        readonly OpenFileDialog openFileDialog;

        readonly ICommand getImagesCommand;
        public ICommand GetImagesCommand { get { return getImagesCommand; } }

        readonly ICommand showScaleWinCommand;

        public ICommand ShowScaleWinCommand { get { return showScaleWinCommand; } }

      
        private ObservableCollection<ImageSource> imageCollection;
        public ObservableCollection<ImageSource> ImageCollection
        {
            get { return imageCollection; }
            set
            {
                imageCollection = value;
                OnPropertyChanged(nameof(ImageCollection));
            }
        }

        private double thresholdNDSI;

        public double ThresholdNDSI
        {
            get { return thresholdNDSI; }
            set
            {
                thresholdNDSI = value;
                OnPropertyChanged(nameof(thresholdNDSI));
            }
        }

        void ExecuteOpenFileDialog()
        {
            string path1="", path2="", path3="";
            MessageBox.Show("Выберите изображение в зеленом канале");
            if (openFileDialog.ShowDialog() == true)
            {
                 path1 = openFileDialog.FileName;
            }
            if (path1 == "")
            {
                throw new ArgumentException("Не выбран файл.");
            }
            MessageBox.Show("Выберите изображение в инфракрасном канале");
            if (openFileDialog.ShowDialog() == true)
            {
                 path2 = openFileDialog.FileName;           
            }
            if (path2 == "")
            {
                throw new ArgumentException("Не выбран файл.");
            }
            MessageBox.Show("Выберите изображение в панхроматическом канале");
            if (openFileDialog.ShowDialog() == true)
            {
                 path3 = openFileDialog.FileName;
            }
            if (path3 == "")
            {
                throw new ArgumentException("Не выбран файл.");
            }
            //GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureGdal();
            Gdal.AllRegister();
//            Ogr.RegisterAll();
            //Console.WriteLine(path1);
            //Console.WriteLine(path2);
            //Console.WriteLine(path3);
            Dataset raster3 = Gdal.Open(path1, Access.GA_ReadOnly); // зеленый канал
            Band raster3band = raster3.GetRasterBand(1);
            Dataset raster6 = Gdal.Open(path2, Access.GA_ReadOnly); // инфракрасный
            Band raster6Band = raster6.GetRasterBand(1);
            int width = raster3band.XSize;
            int height = raster3band.YSize;
            if (width != raster6Band.XSize || height != raster6Band.YSize)
            {
                throw new ArgumentException("Размеры образцов не совпадают.");
            }
            if (width > 5000 || height > 5000)
            {
                throw new ArgumentException("Размер образцов слишком велик, это может привести к проблемам с производительностью.");
            }
            float[] r3 = new float[width * height];
            float[] r6 = new float[width * height];
            raster3band.ReadRaster(0, 0, width, height, r3, width, height, 0, 0);
            raster6Band.ReadRaster(0, 0, width, height, r6, width, height, 0, 0);
            double[] ndsi = CalculationModel.GetNDSI(r3, r6, width, height, thresholdNDSI);
            raster3.Dispose();
            raster3band.Dispose();
            raster6.Dispose();
            raster6Band.Dispose();
            Bitmap bmp = new Bitmap(path1);
            Color blcolor = Color.Blue;
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    if (ndsi[x + y * bmp.Width] == 255)
                        bmp.SetPixel(x, y, blcolor);
                }
            }
            
        }
        private string errText;
        public string ErrText
        {
            get { return errText; }
            set
            {
                errText = value;
                OnPropertyChanged(nameof(errText));
            }
        }

        private string inputText;
        public string InputText
        {
            get { return inputText; }
            set
            {
                inputText = value;
                ValidateInputText();
                OnPropertyChanged(nameof(InputText));
            }
        }

        private bool isInputTextValid;
        public bool IsInputTextValid
        {
            get { return isInputTextValid; }
            set
            {
                isInputTextValid = value;
                OnPropertyChanged(nameof(IsInputTextValid));
            }
        }
        private void ValidateInputText()
        {
            bool isErrValue = false;
            if (!string.IsNullOrEmpty(InputText))
            {
                if (!double.TryParse(InputText, out thresholdNDSI))
                {
                    isErrValue = true;
                    ErrText = "Ошибка! Введенное значение должно быть числом";
                }
                else
                {
                    if (thresholdNDSI < 0.1 || thresholdNDSI > 0.6)
                    {
                        isErrValue = true;
                        ErrText = "Ошибка! Значение должно быть в пределах от 0.1 до 0.6";
                    }
                    else
                    {
                        isErrValue = false;
                    }
                }
            }

            // Обновите значение свойства IsInputTextValid
            IsInputTextValid = isErrValue;
        }
        void ExecuteShowScaleWindow()
        {
            ScaleWindow sclwin = new();
            sclwin.Show();
        }
        void ExecuteShowMainWindow()
        {
            var window = System.Windows.Application.Current.Windows.OfType<PicWindow>().FirstOrDefault();
            if (window != null) { window.Close(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(
           [CallerMemberName] string? propertyName = null) =>
           PropertyChanged?.Invoke(
               this,
               new PropertyChangedEventArgs(propertyName));
    }
}
