using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Model.Enum;

namespace MusicPlayerOnline.Win
{
    public class PlatformFormatValueConverter : IValueConverter
    {
        private const string AllName = "全部";
        private readonly PlatformEnum _allPlatform;
        public PlatformFormatValueConverter()
        {
            _allPlatform = 0;
            foreach (PlatformEnum item in Enum.GetValues(typeof(PlatformEnum)))
            {
                _allPlatform = _allPlatform | item;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PlatformEnum platform)
            {
                if (platform == _allPlatform)
                {
                    return AllName;
                }

                return GetString(platform);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (s == AllName)
                {
                    return _allPlatform;
                }
                return Enum.Parse(typeof(PlatformEnum), s.Substring(s.IndexOf(':') + 1));
            }
            return null;
        }

        public string[] Strings => GetStrings();

        private static string GetString(PlatformEnum platform)
        {
            return $"{GetDescription(platform)}:{platform}";
        }
        private static string GetDescription(PlatformEnum platform)
        {
            return platform.GetDescription();

        }

        public static string[] GetStrings()
        {
            List<string> list = new List<string>();
            list.Add(AllName);
            foreach (PlatformEnum platform in Enum.GetValues(typeof(PlatformEnum)))
            {
                list.Add(GetString(platform));
            }

            return list.ToArray();
        }
    }
}
