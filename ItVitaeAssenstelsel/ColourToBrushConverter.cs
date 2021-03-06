﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ItVitaeAssenstelsel
{
    [ValueConversion(typeof(Color), typeof(Brush))]
    public class ColourToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
               object parameter, System.Globalization.CultureInfo culture)
        {
            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType,
               object parameter, System.Globalization.CultureInfo culture)
        {
             throw new NotImplementedException();
        }
    }
}
