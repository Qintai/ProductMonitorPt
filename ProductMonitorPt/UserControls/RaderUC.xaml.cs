using ProductMonitorPt.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProductMonitorPt.UserControls
{
    /// <summary>
    /// RaderUC.xaml 的交互逻辑
    /// </summary>
    public partial class RaderUC : UserControl
    {
        public RaderUC()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;//Alt+Enter
        }

        /// <summary>
        /// 数据源。支持数据绑定 依赖属性
        /// </summary>
        public List<RaderModel> ItemSource
        {
            get { return (List<RaderModel>)GetValue(ItemSourceProperty); }
            set { SetValue(ItemSourceProperty, value); }
        }

        // 使用DependencyProperty作为MyProperty的后备存储。这将启用动画、样式、绑定等。。。
        public static readonly DependencyProperty ItemSourceProperty =
            DependencyProperty.Register("ItemSource", typeof(List<RaderModel>), typeof(RaderUC));

        /// <summary>
        /// 窗体大小发生变化 重新画图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            Drug();
        }

        private void Drug()
        {
            //判断是否有数据
            if (ItemSource == null || ItemSource.Count == 0)
            {
                ItemSource = new List<RaderModel>
                {
                    new RaderModel { ItemName = "WWW", Value = 90 },
                    new RaderModel { ItemName = "SS", Value = 30.00 },
                    new RaderModel { ItemName = "DDD", Value = 34.89 },
                    new RaderModel { ItemName = "FFF", Value = 69.59 },
                    new RaderModel { ItemName = "TTT", Value = 20 }
                };
                //return;
            }

            // 清空之前画的
            mainCanvas.Children.Clear();
            P1.Points.Clear();
            P2.Points.Clear();
            P3.Points.Clear();
            P4.Points.Clear();
            P5.Points.Clear();

            // 调整大小
            //调整大小(正方形) RenderSize 就是父级控件的宽 高
            double size = Math.Min(RenderSize.Width, RenderSize.Height);
            LayGrid.Height = size;
            LayGrid.Width = size;
            // 雷达图的 半径
            double raduis = size / 2;

            //步子跨度
            double step = 360.0 / ItemSource.Count;

            for (int i = 0; i < ItemSource.Count; i++)
            {
                double x = (raduis - 20) * Math.Cos((step * i - 90) * Math.PI / 180);//x偏移量  余弦函数  raduis * 余弦函数（x * 3.141 / 180）结果
                double y = (raduis - 20) * Math.Sin((step * i - 90) * Math.PI / 180);//y偏移量  正铉函数

                //X Y坐标
                P1.Points.Add(new Point(raduis + x, raduis + y));

                P2.Points.Add(new Point(raduis + x * 0.75, raduis + y * 0.75));

                P3.Points.Add(new Point(raduis + x * 0.5, raduis + y * 0.5));

                P4.Points.Add(new Point(raduis + x * 0.25, raduis + y * 0.25));

                //数据多边形
                P5.Points.Add(new Point(raduis + x * ItemSource[i].Value * 0.01, raduis + y * ItemSource[i].Value * 0.01));

                //文字处理
                TextBlock txt = new TextBlock();
                txt.Width = 60;
                txt.FontSize = 10;
                txt.TextAlignment = TextAlignment.Center;
                txt.Text = ItemSource[i].ItemName;
                txt.Foreground = new SolidColorBrush(Color.FromArgb(100, 255, 255, 255));
                txt.SetValue(Canvas.LeftProperty, raduis + (raduis - 10) * Math.Cos((step * i - 90) * Math.PI / 180) - 30);//设置左边间距
                txt.SetValue(Canvas.TopProperty, raduis + (raduis - 10) * Math.Sin((step * i - 90) * Math.PI / 180) - 7);//设置上边间距

                mainCanvas.Children.Add(txt);
            }
        }
    }
}
