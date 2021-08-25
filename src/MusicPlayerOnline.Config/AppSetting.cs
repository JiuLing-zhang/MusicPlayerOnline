namespace MusicPlayerOnline.Config
{
    public class AppSetting
    {
        private static Config _setting;

        public static Config Setting
        {
            get
            {
                if (_setting == null)
                {
                    Setting = ConfigHandle.Read();
                }
                return _setting;
            }
            set => _setting = value;
        }
    }
}
