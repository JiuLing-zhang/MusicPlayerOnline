namespace MusicPlayerOnline.App.AppInterface
{
    public interface IAppVersionInfo
    {
        /// <summary>
        /// 获取用于更新的程序版本号
        /// </summary>
        /// <returns></returns>
        int GetVersionCode();
        /// <summary>
        /// 获取程序版本号
        /// </summary>
        /// <returns></returns>
        string GetVersionName();
        /// <summary>
        /// 更新
        /// </summary>
        void Update(string downloadUrl);
    }
}
