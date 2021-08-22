using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Model.ViewModel
{
    public class SearchResultViewModel : MusicInfoViewModel
    {
        private int _fee;
        /// <summary>
        /// 费用类型
        /// </summary>
        public int Fee
        {
            get => _fee;
            set
            {
                _fee = value;
                OnPropertyChanged();
            }
        }
    }
}
