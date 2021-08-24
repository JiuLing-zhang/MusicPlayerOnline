using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerOnline.Model.KuGou
{
    public class HttpResultBase<T>
    {
        public int status { get; set; }
        public int error_code { get; set; }
        public string error_msg { get; set; }
        public T data { get; set; }
    }
}
