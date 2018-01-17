using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using XamFormsTestApp.CustomRenderers;
using XamFormsTestApp.iOS.Renderers;

[assembly: ExportRenderer(typeof(ReadonlyEntry), typeof(ReadonlyEntryRenderer))]

namespace XamFormsTestApp.iOS.Renderers
{
    public class ReadonlyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                // Disable user input
                Control.ShouldChangeCharacters = (a, b, c) => false;
            }
        }

    }
}