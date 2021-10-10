using Xamarin.Forms;

namespace MusicPlayerOnlineApp.AppInterface
{
    public interface ILoadingPageService
    {
        void InitLoadingPage(ContentPage loadingIndicatorPage = null);

        void ShowLoadingPage();

        void HideLoadingPage();
    }
}
