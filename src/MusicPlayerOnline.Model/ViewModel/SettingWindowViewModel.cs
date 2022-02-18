using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Model.ViewModel
{
    public class SettingWindowViewModel : ViewModelBase
    {
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
