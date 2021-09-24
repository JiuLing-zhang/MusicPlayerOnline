using Android.Widget;
using MusicPlayerOnlineApp.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ToastService))]
namespace MusicPlayerOnlineApp.Droid
{
    public class ToastService : IToast
    {
        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}