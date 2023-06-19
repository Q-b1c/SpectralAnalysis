using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Project
{
    class CalculationModel
    {
        /// <summary>
        /// Функция для расчета NDSI
        /// </summary>
        /// <param name="raster3"></param>
        /// <param name="raster6"></param>
        /// <returns></returns> 
        public double[] GetNDSI(float[] raster3, float[] raster6, int width, int height, double thresholdNDSI)
        {
            double[] NDSI = new double[width * height];
            for (int i = 0; i < width; i++)
            {
                NDSI[i] = (raster3[i] - raster6[i]) / (raster3[i] + raster6[i]);
                if (NDSI[i] > thresholdNDSI)
                {
                    NDSI[i] = 255;
                }
            }
            return NDSI;
        }
    }
}
