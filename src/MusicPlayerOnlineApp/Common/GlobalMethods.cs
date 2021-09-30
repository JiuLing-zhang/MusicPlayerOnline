using System;
using System.Collections.Generic;
using System.Text;
using MusicPlayerOnline.Model.Model;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.Common
{
    public class GlobalMethods
    {
        public static void PlayMusic(string path)
        {
            MessagingCenter.Send<string>(path, "Play");
        }
    }
}
