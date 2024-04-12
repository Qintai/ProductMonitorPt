using ProductMonitorPt.OpCommand;
using ProductMonitorPt.UserControls;
using ProductMonitorPt.View;
using ProductMonitorPt.ViewModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace ProductMonitorPt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowVM vm = new MainWindowVM();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;

            /*  
                第P18节课-雷达那里。MonitorUC.xaml 中 需要绑定值传给 RaderUC.xaml。<rader:RaderUC ItemSource="{Binding RaderList}"></rader:RaderUC>。
                排查了很久，找到原因是顺序问题。
                InitializeComponent();
                DataContext = new MainWindowVM();
                 // 设置DataContext 一定要在 InitializeComponent() 方法之后，才可以成功
            */
        }

        /// <summary>
        /// 展示详情命令
        /// </summary>
        public Command ShowDetailCmm
        {
            get
            {
                return new Command(ShowDetailUC);
            }
        }

        /// <summary>
        /// 显示车间详情页
        /// </summary>
        private void ShowDetailUC()
        {
            WorkShopDetailUC workShopDetailUC = new WorkShopDetailUC();

            vm.Mycontrol = workShopDetailUC;

            // start 动画效果(由下而上)
            //位移 移动时间
            ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0, 50, 0, -50),
                new Thickness(0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 2000));

            //透明度, 在顶部没有东西的东西，需要有个东西进行填充。
            DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1,
                new TimeSpan(0, 0, 0, 0, 2000));

            // 目标是 workShopDetailUC 这个控件
            Storyboard.SetTarget(thicknessAnimation, workShopDetailUC);
            Storyboard.SetTarget(doubleAnimation, workShopDetailUC);

            // 这个是设置的属性名称
            Storyboard.SetTargetProperty(thicknessAnimation, new PropertyPath("Margin"));
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Opacity"));

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(thicknessAnimation);
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
            // End
        }


        /// <summary>
        /// 返回监控界面命令
        /// </summary>
        public Command GoBackCmm
        {
            get
            {
                return new Command(GoBackMonitor);
            }
        }

        /// <summary>
        /// 返回到监控
        /// </summary>
        private void GoBackMonitor()
        {
            MonitorUC monitorUC = new MonitorUC();
            vm.Mycontrol = monitorUC;
        }

        /// <summary>
        /// 最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMin(object sender, RoutedEventArgs e)
        {
            //最小化
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMax(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;//最大化
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClose(object sender, RoutedEventArgs e)
        {
            //this.Close();
            Environment.Exit(0);
        }

        #region 弹出配置窗口
        /// <summary>
        /// 弹出配置窗口
        /// </summary>
        private void ShowSettingWin()
        {
            //父子关系
            SettingsWin settingsWin = new SettingsWin() { Owner = this };
            settingsWin.ShowDialog();
        }

        /// <summary>
        /// 创建 弹出配置窗口命令。绑定的命令写在这里的原因是这里才可以绑定到当前的窗体。涉及窗体相关的操作都写在窗体后台代码里面。
        /// </summary>
        public Command ShowSettingaCmm
        {
            get
            {
                return new Command(ShowSettingWin);
            }
        }
        #endregion
    }
}