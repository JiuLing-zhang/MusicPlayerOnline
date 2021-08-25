using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Model.ViewModel
{
    public class SettingWindowViewModel : ViewModelBase
    {
        private string _updateMessage;
        /// <summary>
        /// 更新消息
        /// </summary>
        public string UpdateMessage
        {
            get => _updateMessage;
            set
            {
                _updateMessage = value;
                OnPropertyChanged();
            }
        }

        private string _version;
        /// <summary>
        /// 版本
        /// </summary>
        public string Version
        {
            get => _version;
            set
            {
                _version = value;
                OnPropertyChanged();
            }
        }
    }
}
