﻿using MusicPlayerOnline.Model.Enum;

namespace MusicPlayerOnline.Model.Model
{
    public class Config
    {
        /// <summary>
        /// 常规设置
        /// </summary>
        public GeneralConfig General { get; set; }
        /// <summary>
        /// 播放设置
        /// </summary>
        public PlayConfig Play { get; set; }
        /// <summary>
        /// 播放器设置
        /// </summary>
        public PlayerConfig Player { get; set; }
    }

    public class GeneralConfig
    {
        /// <summary>
        /// 是否自动检查更新
        /// </summary>
        public bool IsAutoCheckUpdate { get; set; } = false;

        /// <summary>
        /// 关闭时最小化到托盘
        /// </summary>
        public bool IsHideWindowWhenMinimize { get; set; } = true;

        /// <summary>
        /// 仅WIFI下可播放
        /// </summary>
        public bool IsWifiPlayOnly { get; set; } = true;
    }


    public class PlayConfig
    {
        /// <summary>
        /// 是否保存播放记录到本地
        /// </summary>
        public bool IsSavePlaylistToLocal { get; set; } = true;

        /// <summary>
        /// 歌曲无法播放时自动跳到下一首
        /// </summary>
        public bool IsAutoNextWhenFailed { get; set; } = true;
    }

    public class PlayerConfig
    {
        /// <summary>
        /// 音量
        /// </summary>
        public double Voice { get; set; } = 50;

        /// <summary>
        /// 是否静音
        /// </summary>
        public bool IsSoundOff { get; set; } = false;

        /// <summary>
        /// 播放模式
        /// </summary>
        public PlayModeEnum PlayMode { get; set; } = PlayModeEnum.RepeatList;
    }
}
