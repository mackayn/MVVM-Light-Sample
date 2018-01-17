using UIKit;
using Xamarin.Forms;
using XamFormsTestApp.CustomRenderers;
using XamFormsTestApp.iOS.Renderers;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomImageCell), typeof(CustomImageCellRenderer))]
namespace XamFormsTestApp.iOS.Renderers
{
    public class CustomImageCellRenderer : ImageCellRenderer
    {
        private UIView _bgSelView;

        public override UITableViewCell GetCell(Cell item,UITableViewCell reusableCel, UITableView tv)
        {
            var cell = base.GetCell(item,reusableCel, tv);

            if (_bgSelView == null)
            {
                _bgSelView = new UIView(cell.SelectedBackgroundView.Bounds);
                _bgSelView.Layer.BackgroundColor = UIColor.FromRGB(122, 202, 182).CGColor;
                _bgSelView.Layer.BorderColor = UIColor.FromRGB(255, 255, 255).CGColor;
                _bgSelView.Layer.BorderWidth = 3.0f;
            }
            cell.SelectedBackgroundView = _bgSelView;
            cell.ContentView.BackgroundColor = UIColor.FromRGB(242, 243, 244);
            tv.SeparatorColor = UIColor.FromRGB(255, 255, 255);
            return cell;
        }

    }
}
