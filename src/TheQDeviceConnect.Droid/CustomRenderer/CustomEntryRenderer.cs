using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text;
using Android.Widget;
using TheQDeviceConnect.Droid.CustomRenderer;
using TheQDeviceConnect.UI.CustomUI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AndroidGraphicsColor = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace TheQDeviceConnect.Droid.CustomRenderer
{
    public class CustomEntryRenderer : EntryRenderer
    {

        public CustomEntryRenderer(Context context) : base(context)
        {

        }

        [Obsolete]
        #pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        #pragma warning restore CS0809 // Obsolete member overrides non-obsolete member
        {
            base.OnElementChanged(e);
           
            if (Control != null) {
                
                Control.Elevation = 10;
                Control.TextSize = 30;
                Control.SetPadding(100, 0, 100, 0);
                Control.SetOutlineSpotShadowColor(AndroidGraphicsColor.Black);
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                    Control.BackgroundTintList = ColorStateList.ValueOf(AndroidGraphicsColor.White);
                else
                    Control.Background.SetColorFilter(AndroidGraphicsColor.White, PorterDuff.Mode.SrcAtop);

            }
        }
    }
}
