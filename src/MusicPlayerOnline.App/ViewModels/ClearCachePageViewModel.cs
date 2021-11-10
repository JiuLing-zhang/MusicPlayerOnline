using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MusicPlayerOnline.Log;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Common;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class ClearCachePageViewModel : ViewModelBase
    {
        public ICommand SelectAllCommand => new Command(SelectAll);
        public ICommand ClearCommand => new Command(Clear);
        public ClearCachePageViewModel()
        {
            Caches = new ObservableCollection<MusicFileViewModel>();
        }
        public string Title => "缓存清理";

        private Int64 _allSize;
        public Int64 AllSize
        {
            get => _allSize;
            set
            {
                _allSize = value;
                OnPropertyChanged();

                AllSizeString = SizeToString(value);
            }
        }

        private string _allSizeString;
        public string AllSizeString
        {
            get => _allSizeString;
            set
            {
                _allSizeString = value;
                OnPropertyChanged();
            }
        }

        private Int64 _selectedSize;
        public Int64 SelectedSize
        {
            get => _selectedSize;
            set
            {
                _selectedSize = value;
                OnPropertyChanged();

                SelectedSizeString = SizeToString(value);
            }
        }

        private string _selectedSizeString;
        public string SelectedSizeString
        {
            get => _selectedSizeString;
            set
            {
                _selectedSizeString = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<MusicFileViewModel> _caches;
        /// <summary>
        /// 缓存的文件
        /// </summary>
        public ObservableCollection<MusicFileViewModel> Caches
        {
            get => _caches;
            set
            {
                _caches = value;
                OnPropertyChanged();
            }
        }

        public void OnAppearing()
        {
            SelectedSize = 0;
            GetCacheFiles();
        }

        public void CalcSelectedSize()
        {
            SelectedSize = 0;
            foreach (var cache in Caches)
            {
                if (cache.IsChecked == true)
                {
                    SelectedSize = SelectedSize + cache.Size;
                }
            }
        }

        private void GetCacheFiles()
        {
            Caches.Clear();
            AllSize = 0;
            Task.Run(() =>
            {
                EachDirectory(GlobalArgs.AppMusicCachePath, CalcFilesInfo);
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
                var musicFile = new MusicFileViewModel()
                {
                    Name = file.Name,
                    FullName = file.FullName,
                    Size = file.Length,
                    IsChecked = false
                };
                Caches.Add(musicFile);
                AllSize = AllSize + musicFile.Size;
            });
        }

        private void SelectAll()
        {
            if (Caches.Count == Caches.Count(x => x.IsChecked == true))
            {
                foreach (var cache in Caches)
                {
                    cache.IsChecked = false;
                }
            }
            else
            {
                foreach (var cache in Caches)
                {
                    cache.IsChecked = true;
                }
            }
        }
        private async void Clear()
        {
            var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要删除吗？删除后不可恢复。", "确定", "取消");
            if (isOk == false)
            {
                return;
            }

            foreach (var cache in Caches)
            {
                if (cache.IsChecked == true)
                {
                    try
                    {
                        System.IO.File.Delete(cache.FullName);
                    }
                    catch (Exception ex)
                    {
                        await Logger.WriteAsync(LogTypeEnum.错误, $"删除本地缓存失败：{ex.Message}.{ex.StackTrace}");
                    }
                }
            }
            GetCacheFiles();
            DependencyService.Get<IToast>().Show("删除完成");
        }

        private string SizeToString(Int64 size)
        {
            return $"{size / 1024 / 1024:N2}";
        }
    }
}
