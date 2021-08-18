using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Enum;

namespace MusicPlayerOnline
{
    public class PlatformFormatValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlatformEnum platform)
            {
                return GetString(platform);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                return Enum.Parse(typeof(PlatformEnum), s.Substring(s.IndexOf(':') + 1));
            }
            return null;
        }

        public string[] Strings => GetStrings();

        public static string GetString(PlatformEnum platform)
        {
            return $"{GetDescription(platform)}:{platform}";
        }
        public static string GetDescription(PlatformEnum platform)
        {
            return platform.GetDescription();

        }

        public static string[] GetStrings()
        {
            List<string> list = new List<string>();
            foreach (PlatformEnum platform in Enum.GetValues(typeof(PlatformEnum)))
            {
                list.Add(GetString(platform));
            }

            return list.ToArray();
        }
    }
}
