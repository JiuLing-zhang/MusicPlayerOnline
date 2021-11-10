using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MusicPlayerOnline.App.AppExtends
{
    public class CustomSwitch : Switch
    {

        public new static readonly BindableProperty OnColorProperty =
            BindableProperty.Create(nameof(OnColor), typeof(Color), typeof(CustomSwitch), Color.Default);

        public new Color OnColor
        {
            get => (Color)GetValue(OnColorProperty);
            set => SetValue(OnColorProperty, value);
        }

        public static readonly BindableProperty OnThumbColorProperty =
            BindableProperty.Create(nameof(OnThumbColor), typeof(Color), typeof(CustomSwitch), Color.Default);

        public Color OnThumbColor
        {
            get => (Color)GetValue(OnThumbColorProperty);
            set => SetValue(OnThumbColorProperty, value);
        }

        public static readonly BindableProperty OffColorProperty =
            BindableProperty.Create(nameof(OffColor), typeof(Color), typeof(CustomSwitch), Color.Default);

        public Color OffColor
        {
            get => (Color)GetValue(OffColorProperty);
            set => SetValue(OffColorProperty, value);
        }

        public static readonly BindableProperty OffThumbColorProperty =
            BindableProperty.Create(nameof(OffThumbColor), typeof(Color), typeof(CustomSwitch), Color.Default);

        public Color OffThumbColor
        {
            get => (Color)GetValue(OffThumbColorProperty);
            set => SetValue(OffThumbColorProperty, value);
        }
    }
}
