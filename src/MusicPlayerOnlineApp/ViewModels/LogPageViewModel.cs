using System.Collections.ObjectModel;
using System.Windows.Input;
using MusicPlayerOnline.Service;
using Xamarin.Forms;

namespace MusicPlayerOnlineApp.ViewModels
{
    public class LogPageViewModel : ViewModelBase
    {
        private readonly ILogService _logService;
        public ICommand UpdateLogsCommand => new Command(UpdateLogs);
        public ICommand ClearLogsCommand => new Command(ClearLogs);
        public LogPageViewModel()
        {
            _logService = new LogService();
            Logs = new ObservableCollection<LogDetailViewModel>();
            GetLogs();
        }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title => "日志记录";

        private ObservableCollection<LogDetailViewModel> _logs;
        public ObservableCollection<LogDetailViewModel> Logs
        {
            get => _logs;
            set
            {
                _logs = value;
                OnPropertyChanged();
            }
        }

        public void OnAppearing()
        {
            GetLogs();
        }

        private async void GetLogs()
        {
            if (Logs.Count > 0)
            {
                Logs.Clear();
            }
            var logs = await _logService.GetLogs();
            foreach (var log in logs)
            {
                Logs.Add(new LogDetailViewModel()
                {
                    Time = JiuLing.CommonLibs.Text.TimestampUtils.ConvertToDateTime(log.Timestamp).ToString("yyyy-MM-dd HH:mm:ss"),
                    Message = log.Message
                });
            }
        }

        private async void UpdateLogs()
        {
            var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要上传日志记录吗？", "确定", "取消");
            if (isOk == false)
            {
                return;
            }
        }

        private async void ClearLogs()
        {
            var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要清空日志吗？", "确定", "取消");
            if (isOk == false)
            {
                return;
            }

            await _logService.ClearLogs();
            Logs.Clear();
        }
    }
}
