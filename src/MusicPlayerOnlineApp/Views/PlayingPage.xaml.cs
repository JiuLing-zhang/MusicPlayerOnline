using System;
using MusicPlayerOnline.Model.ViewModelApp;
using MusicPlayerOnline.Player;
using MusicPlayerOnlineApp.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicPlayerOnlineApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayingPage : ContentPage
    {
        private readonly PlayingPageViewModel _myModel = new PlayingPageViewModel();
        public PlayingPage()
        {
            InitializeComponent();
            BindingContext = _myModel;

            Common.GlobalArgs.Audio = DependencyService.Get<IAudio>();
            Common.GlobalArgs.Audio.MediaBegin += Audio_MediaBegin;
            Common.GlobalArgs.Audio.MediaEnded += Audio_MediaEnded;
            Common.GlobalArgs.Audio.MediaFailed += Audio_MediaFailed;
            
            this.Appearing += (sender, args) =>
            {
                RefreshPage();
            };
        }

        private void RefreshPage()
        {
            _myModel.CurrentMusic = GlobalArgs.CurrentMusic;
        }

        private void Audio_MediaBegin()
        {
            _myModel.CurrentMusic = GlobalArgs.CurrentMusic;
            _myModel.IsPlaying = true;
        }

        private void Audio_MediaEnded()
        {
            _myModel.IsPlaying = false;
        }
        private void Audio_MediaFailed()
        {
            DependencyService.Get<IToast>().Show("播放失败");
        }

        private void PlayerStateChange_Clicked(object sender, EventArgs e)
        {
            if (_myModel.IsPlaying == true)
            {
                Common.GlobalArgs.Audio.Pause();
            }
            else
            {
                Common.GlobalArgs.Audio.Start();
            }

            _myModel.IsPlaying = !_myModel.IsPlaying;
        }
    }
}