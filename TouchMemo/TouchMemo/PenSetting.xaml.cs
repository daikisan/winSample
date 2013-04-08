using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TouchMemo
{
    public partial class PenSetting : PhoneApplicationPage
    {
        public PenSetting()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Ellipse buttonEllipse = (sender as Button).Content as Ellipse;
            MainPage.thickness = buttonEllipse.Width / 2;
            NavigationService.GoBack();
        }

        private void Rectangle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SolidColorBrush rectBrush = (sender as Rectangle).Fill as SolidColorBrush;
            MainPage.penColor = rectBrush.Color;

            small.Fill = rectBrush;
            medium.Fill = rectBrush;
            large.Fill = rectBrush;
        }

    }
}