using Android.Graphics;
using Android.Service.Controls;
using Android.Widget;
using MusicPlayerOnlineApp.AppExtends;
using MusicPlayerOnlineApp.Droid.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CustomSwitchRenderer))]
namespace MusicPlayerOnlineApp.Droid.Renderer
{
    public class CustomSwitchRenderer : SwitchRenderer
    {
        private CustomSwitch view;
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || e.NewElement == null)
                return;
            view = (CustomSwitch)Element;
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
            {
                if (this.Control != null)
                {
                    if (this.Control.Checked)
                    {
                        this.Control.TrackDrawable.SetColorFilter(view.OnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                        this.Control.ThumbDrawable.SetColorFilter(view.OnThumbColor.ToAndroid(), PorterDuff.Mode.Multiply);
                    }
                    else
                    {
                        this.Control.TrackDrawable.SetColorFilter(view.OffColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                        this.Control.ThumbDrawable.SetColorFilter(view.OffThumbColor.ToAndroid(), PorterDuff.Mode.Multiply);
                    }
                    this.Control.CheckedChange += this.OnCheckedChange;
                }
            }
        }
        private void OnCheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (this.Control.Checked)
            {
                this.Control.TrackDrawable.SetColorFilter(view.OnColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                this.Control.ThumbDrawable.SetColorFilter(view.OnThumbColor.ToAndroid(), PorterDuff.Mode.Multiply);
            }
            else
            {
                this.Control.TrackDrawable.SetColorFilter(view.OffColor.ToAndroid(), PorterDuff.Mode.SrcAtop);
                this.Control.ThumbDrawable.SetColorFilter(view.OffThumbColor.ToAndroid(), PorterDuff.Mode.Multiply);
            }
        }
        protected override void Dispose(bool disposing)
        {
            this.Control.CheckedChange -= this.OnCheckedChange;
            base.Dispose(disposing);
        }
    }
}