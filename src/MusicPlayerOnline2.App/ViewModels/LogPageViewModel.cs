using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JiuLing.CommonLibs.ExtensionMethods;
using MusicPlayerOnline.Log;
using MusicPlayerOnline.Model.Model;
using MusicPlayerOnline.Service;
using MusicPlayerOnlineApp.AppInterface;
using MusicPlayerOnlineApp.Common;
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

        private List<LogDetail> _dbLogs;
        public void OnAppearing()
        {
            GetLogs();
        }

        private async void GetLogs()
        {
            if (Logs.Count > 0)
            {
                Logs.Clear();
                _dbLogs.Clear();
            }
            _dbLogs = await _logService.GetLogs();
            foreach (var log in _dbLogs)
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
            if (_dbLogs == null || _dbLogs.Count == 0)
            {
                DependencyService.Get<IToast>().Show("没有可上传的日志");
                return;
            }

            var isOk = await App.Current.MainPage.DisplayAlert("提示", "确定要上传日志记录吗？", "确定", "取消");
            if (isOk == false)
            {
                return;
            }

            var logs = new UpdateLog()
            {
                SessionId = $"{DependencyService.Get<IAppVersionInfo>().GetVersionName()} {JiuLing.CommonLibs.GuidUtils.GetFormatN()}",
                Logs = new List<UpdateLogDetail>()
            };
            foreach (var log in _dbLogs)
            {
                logs.Logs.Add(new UpdateLogDetail()
                {
                    Timestamp = log.Timestamp,
                    LogType = (int)log.LogType,
                    Message = log.Message,
                });
            }

            try
            {
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(logs);
                var httpClient = new JiuLing.CommonLibs.Net.HttpClientHelper();
                string httpResult = await httpClient.PostJsonReadString(GlobalArgs.UrlLog, json);
                if (httpResult.IsEmpty())
                {
                    DependencyService.Get<IToast>().Show("上传失败，服务器状态异常");
                    await Logger.WriteAsync(LogTypeEnum.错误, "上传失败，服务器状态异常");
                    return;
                }
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JiuLing.CommonLibs.Model.JsonResult>(httpResult);
                if (obj == null)
                {
                    DependencyService.Get<IToast>().Show("上传失败，服务器返回数据格式异常");
                    await Logger.WriteAsync(LogTypeEnum.错误, "上传失败，服务器返回数据格式异常");
                    return;
                }
                DependencyService.Get<IToast>().Show(obj.Message);
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("上传失败");
                await Logger.WriteAsync(LogTypeEnum.错误, $"日志上传失败：{ex.Message}.{ex.StackTrace}");
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
            _dbLogs.Clear();
        }
    }
}
