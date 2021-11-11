using System;
using Android.Content;
using TheQDeviceConnect.Droid.CustomRenderer;
using TheQDeviceConnect.UI.CustomUI;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace TheQDeviceConnect.Droid.CustomRenderer
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public bool IsUnderline { private get; set; }
        public CustomButtonRenderer(Context context) : base(context)
        {
            
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
            CustomButton newElement = (CustomButton)e.NewElement;
            if (Control != null)
            {
                if (newElement.IsUnderline)
                {
                    Control.PaintFlags = Android.Graphics.PaintFlags.UnderlineText;
                }
                if (!newElement.IsEnabled)
                {
                    Control.SetTextColor(Android.Graphics.Color.White);
                }
            }
        }

    }
}
