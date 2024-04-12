using ProductMonitorPt.Model;
using ProductMonitorPt.OpCommand;
using ProductMonitorPt.UserControls;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace ProductMonitorPt.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public MainWindowVM()
        {
            #region 初始化定时器
            DispatcherTimer dispatcherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            dispatcherTimer.Tick += (object? sender, EventArgs e) =>
            {
                this.TimeStr = DateTime.Now.ToString("HH:mm");
                this.DateStr = DateTime.Now.ToString("yyyy-MM-dd");
            };
            dispatcherTimer.Start();
            #endregion

            #region 初始化环境监控数据
            EnviromentList = new List<EnvironmentModel>();

            EnviromentList.Add(new EnvironmentModel { EnItemName = "光照(Lux)", EnItemValue = 123 });
            EnviromentList.Add(new EnvironmentModel { EnItemName = "噪音(db)", EnItemValue = 55 });
            EnviromentList.Add(new EnvironmentModel { EnItemName = "温度(℃)", EnItemValue = 80 });
            EnviromentList.Add(new EnvironmentModel { EnItemName = "湿度(%)", EnItemValue = 43 });
            EnviromentList.Add(new EnvironmentModel { EnItemName = "PM2.5(m³)", EnItemValue = 20 });
            EnviromentList.Add(new EnvironmentModel { EnItemName = "硫化氢(PPM)", EnItemValue = 15 });
            EnviromentList.Add(new EnvironmentModel { EnItemName = "氮气(PPM)", EnItemValue = 18 });

            // 从设备读取数据(异步) 如果您没有学习到上位机，该区域代码报错，直接注释该区域代码
            #region 从设备读取环境数据
            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        using (SerialPort serialPort = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One))
            //        {
            //            serialPort.Open();
            //            Modbus.Device.IModbusSerialMaster master = Modbus.Device.ModbusSerialMaster.CreateRtu(serialPort);

            //            //功能码03
            //            ushort[] value = master.ReadHoldingRegisters(1, 0, 7);//从设备地址，寄存器起始地址，寄存器个数

            //            for (int i = 0; i < 7; i++)
            //            {
            //                EnviromentList[i].EnItemValue = value[i];
            //            }
            //        }
            //    }
            //});
            #endregion
            #endregion

            #region 初始化报警列表

            AlarmList = new List<AlarmModel>();
            AlarmList.Add(new AlarmModel { Num = "01", Msg = "设备温度过高", Time = "2023-11-23 18:34:56", Duration = 7 });
            AlarmList.Add(new AlarmModel { Num = "02", Msg = "车间温度过高", Time = "2023-12-08 20:40:59", Duration = 10 });
            AlarmList.Add(new AlarmModel { Num = "03", Msg = "设备转速过快", Time = "2024-01-05 12:24:34", Duration = 12 });
            AlarmList.Add(new AlarmModel { Num = "04", Msg = "设备气压偏低", Time = "2024-02-04 19:58:00", Duration = 90 });
            #endregion

            #region 初始化设备监控
            DeviceList = new List<DeviceModel>();
            DeviceList.Add(new DeviceModel { DeviceItem = "电能(Kw.h)", Value = 60.8 });
            DeviceList.Add(new DeviceModel { DeviceItem = "电压(V)", Value = 390 });
            DeviceList.Add(new DeviceModel { DeviceItem = "电流(A)", Value = 5 });
            DeviceList.Add(new DeviceModel { DeviceItem = "压差(kpa)", Value = 13 });
            DeviceList.Add(new DeviceModel { DeviceItem = "温度(℃)", Value = 36 });
            DeviceList.Add(new DeviceModel { DeviceItem = "振动(mm/s)", Value = 4.1 });
            DeviceList.Add(new DeviceModel { DeviceItem = "转速(r/min)", Value = 2600 });
            DeviceList.Add(new DeviceModel { DeviceItem = "气压(kpa)", Value = 0.5 });
            #endregion

            #region 初始化人员缺岗信息
            StuffOutWorkList = new List<StuffOutWorkModel>();
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "张晓婷", Position = "技术员", OutWorkCount = 123 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "李晓", Position = "操作员", OutWorkCount = 23 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "王克俭", Position = "技术员", OutWorkCount = 134 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "陈家栋", Position = "统计员", OutWorkCount = 143 });
            StuffOutWorkList.Add(new StuffOutWorkModel { StuffName = "杨过", Position = "技术员", OutWorkCount = 12 });
            #endregion

            #region 初始化车间列表 
            WorkShopList = new List<WorkShopModel>();
            WorkShopList.Add(new WorkShopModel { WorkShopName = "贴片车间", WorkingCount = 32, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            WorkShopList.Add(new WorkShopModel { WorkShopName = "封装车间", WorkingCount = 20, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            WorkShopList.Add(new WorkShopModel { WorkShopName = "焊接车间", WorkingCount = 68, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            WorkShopList.Add(new WorkShopModel { WorkShopName = "贴片车间", WorkingCount = 68, WaitCount = 8, WrongCount = 4, StopCount = 0 });
            #endregion

            #region 初始化雷达数据 
            RaderList = new List<RaderModel>
            {
                new RaderModel { ItemName = "排烟风机", Value = 90 },
                new RaderModel { ItemName = "客梯", Value = 30.00 },
                new RaderModel { ItemName = "供水机", Value = 34.89 },
                new RaderModel { ItemName = "喷淋水泵", Value = 69.59 },
                new RaderModel { ItemName = "稳压设备", Value = 20 }
            };
            #endregion

            #region 初始化机台列表
            MachineList = new List<MachineModel>();
            Random random = new Random();
            for (int i = 0; i < 20; i++)
            {
                int plan = random.Next(100, 1000);//计划量 随机数
                int finished = random.Next(0, plan);//已完成量
                MachineList.Add(new MachineModel
                {
                    MachineName = "焊接机-" + (i + 1),
                    FinishedCount = finished,
                    PlanCount = plan,
                    Status = "作业中",
                    OrderNo = "H202212345678"
                });
            }
            #endregion
        }

        private UserControl _mycontrol;
        /// <summary>
        /// 监控
        /// </summary>
        public UserControl Mycontrol
        {
            get
            {
                if (_mycontrol == null)
                {
                    _mycontrol = new MonitorUC();
                }
                return _mycontrol;
            }
            set
            {
                _mycontrol = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Mycontrol"));
            }
        }

        #region 时间和日期
        private string _timeStr;
        public string TimeStr
        {
            get
            {
                _timeStr = DateTime.Now.ToString("HH:mm");
                return _timeStr;
            }
            set { _timeStr = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeStr")); }
        }

        private string _dateStr;
        public string DateStr
        {
            get
            {
                _dateStr = DateTime.Now.ToString("yyyy-MM-dd");
                return _dateStr;
            }
            set { _dateStr = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DateStr")); }
        }
        #endregion

        #region 指标数据

        /// <summary>
        /// 机台总数
        /// </summary>
        private string _machineCount = "22454";
        public string MachineCount
        {
            get { return _machineCount; }
            set { _machineCount = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MachineCount")); }
        }

        /// <summary>
        /// 生产的总数
        /// </summary>
        private string _productCount = "1111";
        public string ProductCount
        {
            get { return _productCount; }
            set { _productCount = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductCount")); }
        }


        /// <summary>
        /// 不良计数
        /// </summary>
        private string _badCount = "66666";
        public string BadCount
        {
            get { return _badCount; }
            set { _badCount = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BadCount")); }
        }
        #endregion

        #region 环境监控数据
        /// <summary>
        /// 环境监控数据
        /// </summary>
        private List<EnvironmentModel> _EnviromentList;

        /// <summary>
        /// 环境监控数据
        /// </summary>
        public List<EnvironmentModel> EnviromentList
        {
            get { return _EnviromentList; }
            set
            {
                _EnviromentList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("EnviromentList"));
                }
            }
        }

        #endregion

        #region 报警属性

        /// <summary>
        /// 报警集合
        /// </summary>
        private List<AlarmModel> _AlarmList;

        /// <summary>
        /// 报警集合
        /// </summary>
        public List<AlarmModel> AlarmList
        {
            get { return _AlarmList; }
            set
            {
                _AlarmList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("AlarmList"));
                }
            }
        }
        #endregion

        #region 设备集合属性
        /// <summary>
        /// 设备集合
        /// </summary>
        private List<DeviceModel> _DeviceList;

        /// <summary>
        /// 设备集合
        /// </summary>
        public List<DeviceModel> DeviceList
        {
            get { return _DeviceList; }
            set
            {
                _DeviceList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("DeviceList"));
                }
            }
        }
        #endregion

        #region 雷达数据属性
        /// <summary>
        /// 雷达
        /// </summary>
        private List<RaderModel> _RaderList;

        /// <summary>
        /// 雷达
        /// </summary>
        public List<RaderModel> RaderList
        {
            get
            {
                return _RaderList;
            }
            set
            {
                _RaderList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RaderList"));
                }
            }
        }

        #endregion

        #region 缺岗员工属性

        /// <summary>
        /// 缺岗员工
        /// </summary>
        private List<StuffOutWorkModel> _StuffOutWorkList;

        /// <summary>
        /// 缺岗员工
        /// </summary>
        public List<StuffOutWorkModel> StuffOutWorkList
        {
            get { return _StuffOutWorkList; }
            set
            {
                _StuffOutWorkList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StuffOutWorkList"));
                }
            }
        }
        #endregion

        #region 车间属性
        /// <summary>
        /// 车间
        /// </summary>
        private List<WorkShopModel> _WorkShopList;

        /// <summary>
        /// 车间
        /// </summary>
        public List<WorkShopModel> WorkShopList
        {
            get { return _WorkShopList; }
            set { _WorkShopList = value; }
        }

        #endregion

        #region 机台集合属性

        /// <summary>
        /// 机台集合属性
        /// </summary>
        private List<MachineModel> _MachineList;

        /// <summary>
        /// 机台集合属性
        /// </summary>
        public List<MachineModel> MachineList
        {
            get { return _MachineList; }
            set
            {
                _MachineList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("MachineList"));
                }
            }
        }
        #endregion

    }
}