using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Model.Netease
{
    public class ResultBase<T>
    {
        public List<T> data { get; set; }
        public T result { get; set; }
        public int code { get; set; }
        public string msg { get; set; }
    }
}
