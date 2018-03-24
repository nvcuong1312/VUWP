using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using HtmlAgilityPack;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using System.Threading.Tasks;
using System.Diagnostics;
using MyToolkit.Multimedia;
using Windows.Media.Core;
using Windows.UI.Xaml.Input;
using Windows.ApplicationModel.DataTransfer;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace vozForums_Universal.CommonControl
{
    public sealed partial class rtbViewHtml : UserControl
    {
        private string sourceImage = "";
        private string baseLink = "https://vozforums.com";
        public rtbViewHtml()
        {
            this.InitializeComponent();
        }
        private string CleanText(string input)
        {
            string clean = Windows.Data.Html.HtmlUtilities.ConvertToText(input);
            if (clean == "\0")
                clean = "\n";
            return clean;
        }
        private Block GenerateBlockForTopNode(HtmlNode node)
        {
            return GenerateParagraph(node);
        }
        public string MyHtml
        {
            get { return (string)GetValue(MyHtmlProperty); }
            set { SetValue(MyHtmlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyHtml.  This enables animation, styling, binding, etc...
        public readonly DependencyProperty MyHtmlProperty =
            DependencyProperty.Register("MyHtml", typeof(string), typeof(rtbViewHtml), new PropertyMetadata(" ", (d, e) =>
            {
                rtbViewHtml tb = d as rtbViewHtml;
                tb.ChangeHtml(e);
            }));

        private async void ChangeHtml(DependencyPropertyChangedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    string xhtml = MyHtml.ToString();
                    MyRtbViewHtml.Children.Clear();
                    //Tao Blocks
                    List<Block> blocks = GenerateBlocksForHtml(xhtml, baseLink);
                    //Them cac blocks vao RichTextBlock
                    foreach (Block b in blocks)
                    {
                        RichTextBlock rtb = new RichTextBlock();
                        rtb.TextWrapping = TextWrapping.Wrap;
                        rtb.HorizontalAlignment = HorizontalAlignment.Stretch;
                        rtb.VerticalAlignment = VerticalAlignment.Top;
                        rtb.Blocks.Add(b);
                        MyRtbViewHtml.Children.Add(rtb);
                    }

                }
                catch (Exception ex)
                {
                }
            });
        }


        private List<Block> GenerateBlocksForHtml(string xhtml, string baseLink)
        {
            List<Block> bc = new List<Block>();
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(xhtml);
                Block b = GenerateParagraph(doc.DocumentNode);
                bc.Add(b);
            }
            catch (Exception ex)
            { }
            return bc;
        }
        private Block GenerateParagraph(HtmlNode node)
        {
            Paragraph p = new Paragraph();
            AddChildren(p, node);
            return p;
        }
        private void AddChildren(Paragraph p, HtmlNode node)
        {
            bool added = false;
            foreach (HtmlNode child in node.ChildNodes)
            {
                Inline i = GenerateBlockForNode(child);
                if (i != null)
                {
                    p.Inlines.Add(i);
                    added = true;
                }
            }
            if (!added)
            {
                p.Inlines.Add(new Run() { Text = CleanText(node.InnerText) });
            }
        }
        private void AddChildren(Span s, HtmlNode node)
        {
            bool added = false;

            foreach (HtmlNode child in node.ChildNodes)
            {
                Inline i = GenerateBlockForNode(child);
                if (i != null)
                {
                    s.Inlines.Add(i);
                    added = true;
                }
            }
            if (!added)
            {
                s.Inlines.Add(new Run() { Text = CleanText(node.InnerText) });
            }
        }

        private Inline GenerateBlockForNode(HtmlNode node)
        {
            switch (node.Name)
            {
                case "img":
                case "IMG":
                    return GenerateImage(node);
                case "div":
                    return GenerateSpan(node);
                case "p":
                case "P":
                    return GenerateInnerParagraph(node);

                case "a":
                case "A":
                    if (node.ChildNodes.Count >= 1 && (node.FirstChild.Name == "img" || node.FirstChild.Name == "IMG"))
                        return GenerateImage(node.FirstChild);
                    else
                        return GenerateHyperLinkAsync(node);
                case "li":
                case "LI":
                    return GenerateLI(node);
                case "b":
                case "B":
                    return GenerateBold(node);
                case "i":
                case "I":
                    return GenerateItalic(node);
                case "u":
                case "U":
                    return GenerateUnderline(node);
                case "br":
                case "BR":
                    return new LineBreak();
                case "span":
                case "Span":
                    return GenerateSpan(node);
                case "iframe":
                case "Iframe":
                    return GenerateIFrame(node);
                case "#text":
                    if (!string.IsNullOrWhiteSpace(node.InnerText))
                        return new Run() { Text = CleanText(node.InnerText) };
                    break;
                case "h1":
                case "H1":

                    return GenerateH1(node);
                case "h2":
                case "H2":
                    return GenerateH2(node);
                case "h3":
                case "H3":
                    return GenerateH3(node);
                case "ul":
                case "UL":
                    return GenerateUL(node);
                case "table":
                    return GenerateTable(node);
                case "strong":
                    return GenerateStrong(node);
                default:
                    return GenerateSpanWNewLine(node);
                    if (!string.IsNullOrWhiteSpace(node.InnerText))
                        return new Run() { Text = CleanText(node.InnerText) };


            }
            return null;
        }
        private Inline GenerateStrong(HtmlNode node)
        {
            Span s = new Span();
            Bold bold = new Bold();
            Run r = new Run() { Text = CleanText(node.InnerText) };
            bold.Inlines.Add(r);
            s.Inlines.Add(bold);
            s.Inlines.Add(new LineBreak());
            return s;
        }
        private Inline GenerateLI(HtmlNode node)
        {
            Span s = new Span();
            InlineUIContainer iui = new InlineUIContainer();
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = new SolidColorBrush(Colors.Black);
            ellipse.Width = 6;
            ellipse.Height = 6;
            ellipse.Margin = new Thickness(5, 0, 0, 1);
            iui.Child = ellipse;
            s.Inlines.Add(iui);
            AddChildren(s, node);
            s.Inlines.Add(new LineBreak());
            return s;
        }

        private Inline GenerateImage(HtmlNode node)
        {
            Span s = new Span();
            try
            {
                InlineUIContainer iui = new InlineUIContainer();
                var sourceUri = System.Net.WebUtility.HtmlDecode(node.Attributes["src"].Value);
                Image img = new Image() { Source = new BitmapImage(new Uri(sourceUri)), Tag = sourceUri };

                if (node.Attributes["src"].Value.ToString().Contains("Assets/iconvoz") || node.Attributes["src"].Value.ToString().Contains("https://vozforums.com/images/smilies/brick.png"))
                {
                    img.Height = 35;
                    img.Width = 35;
                }


                img.Stretch = Stretch.UniformToFill;
                img.VerticalAlignment = VerticalAlignment.Top;
                img.HorizontalAlignment = HorizontalAlignment.Center;
                img.ImageOpened += img_ImageOpened;
                img.ImageFailed += img_ImageFailed;
                img.RightTapped += new RightTappedEventHandler(img_RightTapped);
                //img.Tapped += ScrollingBlogPostDetailPage.img_Tapped;   
                iui.Child = img;
                s.Inlines.Add(iui);
                //s.Inlines.Add(new LineBreak());
            }
            catch (Exception ex)
            {

            }
            return s;
        }

        private void img_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Image img = sender as Image;
            BitmapImage bitMap = img.Source as BitmapImage;
            //Uri uri = bitMap?.UriSource;
            sourceImage = ((Image)sender).Tag.ToString();
            MenuFlyout myFlyout = new MenuFlyout();
            MenuFlyoutItem copyLink = new MenuFlyoutItem { Text = "Copy Image", Height = 30, Width = 120, Padding = new Thickness(0, 0, 0, 0) };
            copyLink.Click += fly_Click;
            myFlyout.Items.Add(copyLink);
            myFlyout.ShowAt(sender as UIElement, e.GetPosition(sender as UIElement));
        }

        private void fly_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(sourceImage);
            Clipboard.SetContent(dataPackage);
            sourceImage = string.Empty;
        }

        private void img_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var i = 5;
        }


        private async void img_ImageOpened(object sender, RoutedEventArgs e)
        {
            Image img = sender as Image;
            BitmapImage bimg = img.Source as BitmapImage;

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {

                if (bimg.PixelWidth <= 50 && bimg.PixelWidth >= 40 && bimg.PixelHeight <= 50 && bimg.PixelHeight >= 40)
                {
                    img.Height = 35;
                    img.Width = 35;
                }
                else if (bimg.PixelWidth > 400 || bimg.PixelHeight > 350)
                {
                    if (bimg.PixelHeight > 0 && ActualWidth > 24 && double.IsNaN(img.Width))
                    {
                        var withd = (double)ActualWidth - 24;
                        if (bimg.PixelWidth < Width)
                            Width = bimg.PixelWidth;
                        if (img.Width < Width && img.Width != 0)
                            Width = img.Width;

                        img.Width = Width;
                        img.Height = bimg.PixelHeight * Width / bimg.PixelWidth;
                    }
                }

                else
                {
                    img.Height = bimg.PixelHeight;
                    img.Width = bimg.PixelWidth;
                }
            });



        }

        private Inline GenerateHyperLinkAsync(HtmlNode node)
        {
            Span s = new Span();
            Paragraph p = new Paragraph();
            InlineUIContainer iui = new InlineUIContainer();
            Debug.WriteLine(node.Attributes["href"].Value);
            string lk = node.Attributes["href"].Value;
            string urlMedia = null;


            if (!lk.Contains("https://") && lk.Contains("m.youtube.com"))
            {
                urlMedia = lk.Remove(0, 25);
                urlMedia = urlMedia.Replace("%3A", ":").Replace("%2F", "/").Replace("%3D", "=").Replace("%3F", "?");
                urlMedia = urlMedia.Remove(0, 30);
                var a = Task.Run(async () =>
                {
                    Uri uri = await GetYoutubeUri(urlMedia);
                    return uri;
                }
                );
                try
                {
                    Grid mediaGrid = new Grid();
                    mediaGrid.Children.Add(new MediaPlayerElement() { Source = MediaSource.CreateFromUri(new Uri(a.Result.ToString())), AutoPlay = false, AreTransportControlsEnabled = true, Width = 300, Height = 250 });
                    s.Inlines.Add(new InlineUIContainer() { Child = mediaGrid });
                    //s.Inlines.Add(new InlineUIContainer() { Child = new MediaPlayerElement() { Source = MediaSource.CreateFromUri(new Uri(a.Result.ToString())), AutoPlay = false, AreTransportControlsEnabled = true, Width = 300, Height = 250 } });
                }
                catch { }

            }

            if (!lk.Contains("https://") && lk.Contains("youtube.com"))
            {
                urlMedia = lk.Remove(0, 25);
                urlMedia = urlMedia.Replace("%3A", ":").Replace("%2F", "/").Replace("%3D", "=").Replace("%3F", "?");
                urlMedia = urlMedia.Remove(0, 32);
                var a = Task.Run(async () =>
                {
                    Uri uri = await GetYoutubeUri(urlMedia);
                    return uri;
                }
                );
                try
                {
                    Grid mediaGrid = new Grid();
                    mediaGrid.Children.Add(new MediaPlayerElement() { Source = MediaSource.CreateFromUri(new Uri(a.Result.ToString())), AutoPlay = false, AreTransportControlsEnabled = true, Width = 300, Height = 250 });
                    s.Inlines.Add(new InlineUIContainer() { Child = mediaGrid });
                }
                catch { }
            }

            if (lk.Contains("https://") == false)
            {
                lk = string.Format("https://vozforums.com/{0}", lk);

                iui.Child = new HyperlinkButton() { NavigateUri = new Uri(lk, UriKind.RelativeOrAbsolute), Content = CleanText(node.InnerText), Margin = new Thickness(3, 3, 0, 0) };
                s.Inlines.Add(iui);
            }
            return s;
        }

        internal async Task<Uri> GetYoutubeUri(string VideoID)
        {
            YouTubeUri uri = await YouTube.GetVideoUriAsync(VideoID, YouTubeQuality.NotAvailable);
            return uri.Uri;
        }
        private Inline GenerateIFrame(HtmlNode node)
        {
            try
            {
                Span s = new Span();
                s.Inlines.Add(new LineBreak());
                InlineUIContainer iui = new InlineUIContainer();
                WebView ww = new WebView() { Source = new Uri(node.Attributes["src"].Value, UriKind.Absolute), Width = Int32.Parse(node.Attributes["width"].Value), Height = Int32.Parse(node.Attributes["height"].Value) };

                iui.Child = ww;
                s.Inlines.Add(iui);
                s.Inlines.Add(new LineBreak());
                return s;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private Inline GenerateBold(HtmlNode node)
        {
            Bold b = new Bold();
            AddChildren(b, node);
            return b;
        }
       private Inline GenerateUnderline(HtmlNode node)
        {
            Underline u = new Underline();
            AddChildren(u, node);
            return u;
        }
        private Inline GenerateItalic(HtmlNode node)
        {
            Italic i = new Italic();
            AddChildren(i, node);
            return i;
        }
        private Inline GenerateUL(HtmlNode node)
        {
            Span s = new Span();
            s.Inlines.Add(new LineBreak());
            AddChildren(s, node);
            return s;
        }
        private Inline GenerateInnerParagraph(HtmlNode node)
        {
            Span s = new Span();
            s.Inlines.Add(new LineBreak());
            AddChildren(s, node);
            s.Inlines.Add(new LineBreak());
            return s;
        }
        private Inline GenerateSpan(HtmlNode node)
        {
            Span s = new Span();
            AddChildren(s, node);
            return s;
        }
        private Inline GenerateSpanWNewLine(HtmlNode node)
        {
            Span s = new Span();
            AddChildren(s, node);
            if (s.Inlines.Count > 0)
                s.Inlines.Add(new LineBreak());
            return s;
        }
        private Span GenerateH3(HtmlNode node)
        {
            Span s = new Span();
            s.Inlines.Add(new LineBreak());
            Bold bold = new Bold();
            Run r = new Run() { Text = CleanText(node.InnerText) };
            bold.Inlines.Add(r);
            s.Inlines.Add(bold);
            s.Inlines.Add(new LineBreak());
            return s;
        }
        private Inline GenerateH2(HtmlNode node)
        {
            Span s = new Span() { FontSize = 24 };
            s.Inlines.Add(new LineBreak());
            Run r = new Run() { Text = CleanText(node.InnerText) };
            s.Inlines.Add(r);
            s.Inlines.Add(new LineBreak());
            return s;
        }
        private Inline GenerateH1(HtmlNode node)
        {
            Span s = new Span() { FontSize = 30 };
            s.Inlines.Add(new LineBreak());
            Run r = new Run() { Text = CleanText(node.InnerText) };
            s.Inlines.Add(r);
            s.Inlines.Add(new LineBreak());
            return s;
        }
        private Inline GenerateTable(HtmlNode node)
        {
            Span s = new Span() { FontStyle = Windows.UI.Text.FontStyle.Italic };
            s.Foreground = new SolidColorBrush(Colors.LightCoral);

            InlineUIContainer iuiTop = new InlineUIContainer();
            Line lineTop = new Line();
            lineTop.Stroke = new SolidColorBrush(Windows.UI.Colors.DarkGray);
            lineTop.X1 = 0;
            lineTop.X2 = ActualWidth;
            lineTop.VerticalAlignment = VerticalAlignment.Top;
            lineTop.Margin = new Thickness(0, 0, 0, 0);
            lineTop.StrokeThickness = 1;
            lineTop.StrokeDashArray = new DoubleCollection() { 4 };
            iuiTop.Child = lineTop;
            s.Inlines.Add(iuiTop);
            AddChildren(s, node);
            InlineUIContainer iui = new InlineUIContainer();
            Line line = new Line();
            line.Stroke = new SolidColorBrush(Windows.UI.Colors.DarkGray);
            line.X1 = 0;
            line.X2 = ActualWidth;
            line.VerticalAlignment = VerticalAlignment.Top;
            line.Margin = new Thickness(0, 0, 0, 0);
            line.StrokeThickness = 1;
            line.StrokeDashArray = new DoubleCollection() { 4 };
            iui.Child = line;
            s.Inlines.Add(iui);
            s.Inlines.Add(new LineBreak());
            return s;
        }
    }
}
