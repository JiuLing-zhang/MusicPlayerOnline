using System;
using JiuLing.CommonLibs.ExtensionAttributes;

namespace MusicPlayerOnline.Model.Enum
{
    [Flags]
    public enum PlatformEnum
    {
        [Description("网易")]
        Netease = 1,
        [Description("酷狗")]
        KuGou = 2
    }
}
