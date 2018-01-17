using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamFormsTestApp.CustomRenderers
{
    public class CollapsibleContentView : Grid
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(CollapsibleContentView), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(CollapsibleContentView), null);

        public static readonly BindableProperty CollapsedProperty =
            BindableProperty.Create("Collapsed", typeof(bool), typeof(CollapsibleContentView), false,
                BindingMode.TwoWay);

        public static readonly BindableProperty IndicatorImageProperty =
            BindableProperty.Create("IndicatorImage", typeof(ImageSource), typeof(CollapsibleContentView), null);

        public static readonly BindableProperty HeaderTextProperty =
            BindableProperty.Create("HeaderText", typeof(string), typeof(CollapsibleContentView), null);

        public static readonly BindableProperty HeaderTextColorProperty =
            BindableProperty.Create("HeaderTextColor", typeof(Color), typeof(CollapsibleContentView), Color.Black);

        public static readonly BindableProperty HeaderBackgroundColorProperty =
           BindableProperty.Create("HeaderBackgroundColor", typeof(Color), typeof(CollapsibleContentView), Color.Black);

        public static readonly BindableProperty HeaderTextSizeProperty =
            BindableProperty.Create("HeaderTextSize", typeof(double), typeof(CollapsibleContentView),
                Font.Default.FontSize);

        public static readonly BindableProperty HeaderFontAttributesProperty =
            BindableProperty.Create("HeaderFontAttributes", typeof(FontAttributes), typeof(CollapsibleContentView),
                FontAttributes.None);

        public static readonly BindableProperty CollapseContentProperty =
            BindableProperty.Create("CollapseContent", typeof(StackLayout), typeof(CollapsibleContentView), null);

        private Image _buttonImage;
        private StackLayout _collapseContent;
        private Grid _headerGrid;
        private Label _headerText;

        public CollapsibleContentView()
        {
            Initialize();
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public bool Collapsed
        {
            get { return (bool)GetValue(CollapsedProperty); }
            set { SetValue(CollapsedProperty, value); }
        }

        public ImageSource IndicatorImage
        {
            get { return (ImageSource)GetValue(IndicatorImageProperty); }
            set { SetValue(IndicatorImageProperty, value); }
        }

        public StackLayout CollapseContent
        {
            get { return (StackLayout)GetValue(CollapseContentProperty); }
            set { SetValue(CollapseContentProperty, value); }
        }

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        public Color HeaderTextColor
        {
            get { return (Color)GetValue(HeaderTextColorProperty); }
            set { SetValue(HeaderTextColorProperty, value); }
        }

        public double HeaderTextSize
        {
            get { return (double)GetValue(HeaderTextSizeProperty); }
            set { SetValue(HeaderTextSizeProperty, value); }
        }

        public FontAttributes HeaderFontAttributes
        {
            get { return (FontAttributes)GetValue(HeaderFontAttributesProperty); }
            set { SetValue(HeaderFontAttributesProperty, value); }
        }

        public Color HeaderBackgroundColor
        {
            get { return (Color)GetValue(HeaderBackgroundColorProperty); }
            set { SetValue(HeaderBackgroundColorProperty, value); }
        }

        private ICommand TransitionCommand
        {
            get
            {
                return new Command(async () =>
                {
                    AnchorX = 0.48;
                    AnchorY = 0.48;

                    Collapsed = !Collapsed;

                    await _buttonImage.RotateTo(Collapsed ? 0 : 180, 130, Easing.Linear);
                    await Task.Delay(100);
                    await _buttonImage.ScaleTo(1, 50, Easing.Linear);

                    if (Command != null)
                    {
                        Command.Execute(CommandParameter);
                    }

                    ItemTapped(this, EventArgs.Empty);
                });
            }
        }

        public event EventHandler ItemTapped = (e, a) => { };

        public void Initialize()
        {
            _buttonImage = new Image
            {
                Aspect = Aspect.Fill,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                InputTransparent = true
            };

            _headerText = new Label
            {
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                InputTransparent = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            _collapseContent = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            _headerGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                RowDefinitions =
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(40, GridUnitType.Absolute)}
                }
            };
            _headerGrid.Children.Add(_headerText, 0, 0);
            _headerGrid.Children.Add(_buttonImage, 1, 0);
           

            RowDefinitions.Add(new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) });
            RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            RowSpacing = 0;
            ColumnSpacing = 0;
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            InputTransparent = false;

            Children.Add(_headerGrid, 0, 0);
            _headerGrid.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = TransitionCommand
            });

        }

        public async void Reset()
        {
            Collapsed = true;
            await _buttonImage.RotateTo(Collapsed ? 0 : 180, 130, Easing.Linear);
            await Task.Delay(100);
            await _buttonImage.ScaleTo(1, 50, Easing.Linear);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged();
            if (propertyName == null) return;
            if (Equals(propertyName, "IndicatorImage"))
            {
                _buttonImage.Source = IndicatorImage;
            }
            if (Equals(propertyName, "HeaderText"))
            {
                _headerText.Text = HeaderText;
            }
            if (Equals(propertyName, "HeaderTextColor"))
            {
                _headerText.TextColor = HeaderTextColor;
            }
            if (Equals(propertyName, "HeaderTextSize"))
            {
                _headerText.FontSize = HeaderTextSize;
            }
            if (Equals(propertyName, "HeaderFontAttributes"))
            {
                _headerText.FontAttributes = HeaderFontAttributes;
            }
            if (Equals(propertyName, "CollapseContent"))
            {
                _collapseContent = CollapseContent;
                Children.Add(_collapseContent, 0, 1);
            }
            if (Equals(propertyName, "Collapsed"))
            {
                _collapseContent.IsVisible = !Collapsed;
                _collapseContent.ForceLayout();
            }
            if (Equals(propertyName, "HeaderBackgroundColor"))
            {
                _headerGrid.BackgroundColor = HeaderBackgroundColor;
            }
        }
    }
}
