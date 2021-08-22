using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiuLing.CommonLibs.ExtensionAttributes;

namespace MusicPlayerOnline.Model.Enum
{
    public enum PlayModeEnum
    {
        [Description("单曲循环")]
        RepeatOne,
        [Description("列表循环")]
        RepeatList,
        [Description("随机播放")]
        Shuffle
    }
}
