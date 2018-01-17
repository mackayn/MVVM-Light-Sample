
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Text;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using XamFormsTestApp.CustomRenderers;
using XamFormsTestApp.Droid.Renderers;
using Color = Xamarin.Forms.Color;

[assembly: ExportRenderer(typeof(ReadonlyEntry), typeof(ReadonlyEntryRenderer))]

namespace XamFormsTestApp.Droid.Renderers
{
    public class ReadonlyEntryRenderer : EntryRenderer
    {
        // Override the OnElementChanged method so we can tweak this renderer post-initial setup
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // Diasble user input
                Control.InputType = InputTypes.Null;

                // Remove border
                var shape = new ShapeDrawable(new RectShape());
                shape.Paint.Color = Color.Transparent.ToAndroid();
                shape.Paint.StrokeWidth = 0;
                shape.Paint.SetStyle(Paint.Style.Stroke);

                Control.SetBackgroundDrawable(shape);

            }
        }
    }
}