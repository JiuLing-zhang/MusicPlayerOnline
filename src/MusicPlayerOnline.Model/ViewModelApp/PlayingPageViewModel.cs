using System;
using System.Collections.Generic;
using System.Text;
using MusicPlayerOnline.Model.ViewModel;

namespace MusicPlayerOnline.Model.ViewModelApp
{
    public class PlayingPageViewModel : ViewModelBase
    {
        public PlayingPageViewModel()
        {

        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title => "正在播放";
    }
}
