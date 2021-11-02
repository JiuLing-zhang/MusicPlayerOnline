using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayerOnline.Log;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnlineApp.Common;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class ClearCachePageViewModel : ViewModelBase
    {
        public ClearCachePageViewModel()
        {
            Caches = new ObservableCollection<MusicFile>();
            Task.Run(() =>
            {
                ScanCacheFiles();
            });
        }

        private ObservableCollection<MusicFile> _caches;
        /// <summary>
        /// 缓存的文件
        /// </summary>
        public ObservableCollection<MusicFile> Caches
        {
            get => _caches;
            set
            {
                _caches = value;
                OnPropertyChanged();
            }
        }

        private void ScanCacheFiles()
        {
            EachDirectory(GlobalArgs.AppMusicCachePath, paths =>
            {
                CalcFilesInfo(paths);
            });
        }

        private void EachDirectory(string folderPath, Action<List<string>> callbackFilePaths)
        {
            try
            {
                Directory.GetDirectories(folderPath).ToList().ForEach(path =>
                     {
                         //继续遍历文件夹内容
                         EachDirectory(path, callbackFilePaths);
                     });

                callbackFilePaths.Invoke(Directory.GetFiles(folderPath).ToList());
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.Write(LogTypeEnum.错误, $"遍历文件夹{folderPath}失败，没有权限：{ex.Message}.{ex.StackTrace}");
            }
        }

        private void CalcFilesInfo(List<string> paths)
        {

            //根据路径加载文件信息
            var files = paths.Select(path => new FileInfo(path)).ToList();
            files.ForEach(file =>
            {
                //符合条件的文件写入队列
                var musicFile = new MusicFile()
                {
                    Name = file.Name,
                    Size = file.Length,
                };
                Caches.Add(musicFile);
            });
        }
    }
}
