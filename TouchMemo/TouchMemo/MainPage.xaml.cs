using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TouchMemo.Resources;
using System.Windows.Media;
using System.Windows.Ink;
using System.Windows.Media.Imaging;
using Microsoft.Xna.Framework.Media;
using System.IO;
using Microsoft.Phone.Tasks;

namespace TouchMemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static Color penColor = Colors.White;
        public static Double thickness = 5;
        Stroke currentStroke;
        private Stack<Stroke> _removedStrokes = new Stack<Stroke>();

        // コンストラクター
        public MainPage()
        {
            InitializeComponent();

            // ApplicationBar をローカライズするためのサンプル コード
            //BuildLocalizedApplicationBar();
        }

        private void inkPresenter1_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            currentStroke = new Stroke();
            currentStroke.DrawingAttributes.Color = penColor;
            currentStroke.DrawingAttributes.Width = thickness;
            currentStroke.DrawingAttributes.Height = thickness;

            currentStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(inkPresenter1));
            inkPresenter1.Strokes.Add(currentStroke);
        }

        private void inkPresenter1_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            currentStroke.StylusPoints.Add(e.StylusDevice.GetStylusPoints(inkPresenter1));
        }

        private void btnSettimg_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/PenSetting.xaml", UriKind.Relative));
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (inkPresenter1.Strokes != null && inkPresenter1.Strokes.Count > 0)
            {
                _removedStrokes.Push(inkPresenter1.Strokes.Last());
                inkPresenter1.Strokes.RemoveAt(inkPresenter1.Strokes.Count - 1);
            }
        }

        private void btnRedo_Click(object sender, EventArgs e)
        {
            if (_removedStrokes != null && _removedStrokes.Count > 0)
            {
                inkPresenter1.Strokes.Add(_removedStrokes.Pop());
            }
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("画面を消去します。よろしいですか？", "画面消去", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                inkPresenter1.Strokes.Clear();
                inkPresenter1.Background = new SolidColorBrush(Colors.Green);
            }
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            WriteableBitmap bmp = new WriteableBitmap(inkPresenter1, null);

            MemoryStream stream = new MemoryStream();
            bmp.SaveJpeg(stream, bmp.PixelWidth, bmp.PixelHeight, 0, 100);

            using (MediaLibrary medialib = new MediaLibrary())
            {
                medialib.SavePicture("TouchMemo", stream.ToArray());
            }

            MessageBox.Show("保存しました");
        }

        private void mnuLoading_Click(object sender, EventArgs e)
        {
            PhotoChooserTask photo = new PhotoChooserTask();
            photo.Completed += photo_Completed;
            photo.Show();
        }

        void photo_Completed(object sender, PhotoResult e)
        {
            //throw new NotImplementedException();
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);

                ImageBrush imgBrush = new ImageBrush()
                {
                    Stretch = Stretch.UniformToFill,
                    ImageSource = bmp
                };
                inkPresenter1.Background = imgBrush;
            }
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        // ローカライズされた ApplicationBar を作成するためのサンプル コード
        //private void BuildLocalizedApplicationBar()
        //{
        //    // ページの ApplicationBar を ApplicationBar の新しいインスタンスに設定します。
        //    ApplicationBar = new ApplicationBar();

        //    // 新しいボタンを作成し、テキスト値を AppResources のローカライズされた文字列に設定します。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // AppResources のローカライズされた文字列で、新しいメニュー項目を作成します。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}