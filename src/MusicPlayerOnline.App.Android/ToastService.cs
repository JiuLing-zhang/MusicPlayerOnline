using Android.Widget;
using MusicPlayerOnline.App.AppInterface;
using MusicPlayerOnline.App.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ToastService))]
namespace MusicPlayerOnline.App.Droid
{
    public class ToastService : IToast
    {
        public void Show(string message)
        {
            Android.Widget.Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}