﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ItVitaeAssenstelsel
{
    public class ColourBox : INotifyPropertyChanged
    {
        #region private properties
        private string _ColourName = "Red";
        private Color _Colour;
        #endregion

        #region public properties
        public string ColourName
        {
            get { return _ColourName; }
            set
            {
                _ColourName = value;
                Colour = (Color)ColorConverter.ConvertFromString(ColourName);
                OnPropertyChanged();
            }
        }
        public Color Colour
        {
            get { return _Colour; }
            set
            {
                _Colour = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
