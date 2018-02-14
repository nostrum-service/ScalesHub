using System;
using System.Collections.Generic;
using System.Linq;
using Nostrum.ScalesComponent.Serial;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;
using ScalesHubConsole;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using NLog;

namespace Nostrum.ScalesComponent
{

    public delegate void ScalesEventWeightChanged(object sender, double weight);

    public delegate void ScalesEventStatusChanged(object sender, NxDeviceState status);

    public delegate void ScalesEventDataReceived(object sender, double weight);

    public delegate void ScalesEventRawDataReceived(object sender, string data);

    public delegate void ScalesEventConnectionStatusChanged(object sender, ConnectionStatus connectionStatus);

    public enum ConnectionStatus
    {
        CS_ONLINE,
        CS_OFFLINE
    }

    public interface IScales
    {
        event ScalesEventWeightChanged OnScalesEventWeightChanged;
        event ScalesEventStatusChanged OnScalesEventStatusChanged;
        event ScalesEventDataReceived OnScalesEventDataReceived;
        event ScalesEventRawDataReceived OnScalesEventRawDataReceived;
        event ScalesEventConnectionStatusChanged OnScalesConnectionStatusChanged;

        String Id { get; set; }

        void Start();

        void Stop();

        double Weight { get; }

        NxDeviceState TerminalStatus { get; }

        BaseChannelSettings Settings { get; set; }

        ConnectionStatus ConnectionStatus { get; }

        DateTime LastPacketTimestamp { get; }

        TimeSpan Freeze { get; }

        TimeSpan Timeout { get; set; }

        TimeSpan Exposure { get; set; }
    }

    public class Scales : IScales
    {
        private SerialPortManager portManager;
        private NxDataFrame nxFrame;

        protected System.Threading.Timer checker = null;
        TimeSpan checkerSpan = TimeSpan.FromMilliseconds(500);

        public event ScalesEventWeightChanged OnScalesEventWeightChanged;
        public event ScalesEventStatusChanged OnScalesEventStatusChanged;
        public event ScalesEventDataReceived OnScalesEventDataReceived;
        public event ScalesEventRawDataReceived OnScalesEventRawDataReceived;
        public event ScalesEventConnectionStatusChanged OnScalesConnectionStatusChanged;

        public Scales()
        {
            nxFrame = new NxDataFrame();
            portManager = new SerialPortManager();
            checker = new System.Threading.Timer((unused) => checkOnlineStatus(), null, System.Threading.Timeout.Infinite, (int)checkerSpan.TotalMilliseconds);
            portManager.NewSerialDataRecieved += portManager_NewSerialDataRecieved;
        }

        ~Scales()
        {
            Stop();

            if (checker != null)
            {
                checker.Dispose();
                checker = null;
            }

            portManager.NewSerialDataRecieved -= portManager_NewSerialDataRecieved;
            portManager.Dispose(); 
        }

        protected void checkOnlineStatus()
        {
            if ((DateTime.Now - hLastPacketTimestamp).TotalMilliseconds > hTimeout.TotalMilliseconds)
            {
                ConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;
            }
            else
            {
                ConnectionStatus = ScalesComponent.ConnectionStatus.CS_ONLINE;
            }
        }

        void portManager_NewSerialDataRecieved(object sender, SerialDataEventArgs e)
        {
            hLastPacketTimestamp = DateTime.Now;

            ConnectionStatus = ScalesComponent.ConnectionStatus.CS_ONLINE;

            bool decoded = false;
            try
            {
                decoded = nxFrame.Decode(e.Data);
            }
            catch
            {
                decoded = false;
            }

            if (decoded)
            {
                bool bWeightChanged = hWeight != nxFrame.Weight;
                hWeight = nxFrame.Weight;

                if (OnScalesEventDataReceived != null)
                {
                    try
                    {
                        OnScalesEventDataReceived(this, Weight);
                    }
                    catch
                    {
                    }
                }

                if (bWeightChanged)
                {
                    if (OnScalesEventWeightChanged != null)
                    {
                        try
                        {
                            OnScalesEventWeightChanged(this, Weight);
                        }
                        catch
                        {
                        }
                    }
                }

                if (hTerminalStatus != nxFrame.State)
                {
                    hTerminalStatus = nxFrame.State;

                    if (OnScalesEventWeightChanged != null)
                    {
                        try
                        {
                            OnScalesEventStatusChanged(this, hTerminalStatus);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if (OnScalesEventRawDataReceived != null)
            {
                try
                {
                    String rawData = Encoding.ASCII.GetString(e.Data);
                    OnScalesEventRawDataReceived(this, rawData);
                }
                catch
                {
                }
            }
        }

        private double hWeight = 0.0;
        public double Weight
        {
            get { return hWeight; }
        }

        public void Start()
        {
            hConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;
            checker.Change(0, (int)checkerSpan.TotalMilliseconds);
            portManager.StartListening();
        }

        public void Stop()
        {
            try
            {
                if (checker != null)
                    checker.Change(System.Threading.Timeout.Infinite, (int)checkerSpan.TotalMilliseconds);
            }
            catch
            {
            }

            ConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;
            try
            {
                portManager.StopListening();
            }
            catch
            {
            }
        }

        private String hId;
        public String Id
        {
            get { return hId; }
            set { hId = value; }
        }

        public BaseChannelSettings Settings
        {
            get
            {
                return new SerialPortSettings
                {
                    PortName = portManager.CurrentSerialSettings.PortName,
                    BaudRate = portManager.CurrentSerialSettings.BaudRate,
                    Parity = portManager.CurrentSerialSettings.Parity,
                    StopBits = portManager.CurrentSerialSettings.StopBits,
                    DataBits = portManager.CurrentSerialSettings.DataBits
                };
            }

            set
            {
                if (value is SerialPortSettings)
                {
                    SerialPortSettings tmp = value as SerialPortSettings;
                    portManager.CurrentSerialSettings.PortName = tmp.PortName;
                    portManager.CurrentSerialSettings.BaudRate = tmp.BaudRate;
                    portManager.CurrentSerialSettings.Parity = tmp.Parity;
                    portManager.CurrentSerialSettings.StopBits = tmp.StopBits;
                    portManager.CurrentSerialSettings.DataBits = tmp.DataBits;
                }
            }
        }

        private NxDeviceState hTerminalStatus = NxDeviceState.DS_NONE;
        public NxDeviceState TerminalStatus
        {
            get { return hTerminalStatus; }
        }

        private ConnectionStatus hConnectionStatus = ConnectionStatus.CS_OFFLINE;
        public ConnectionStatus ConnectionStatus 
        {
            get 
            { 
                return hConnectionStatus; 
            }

            private set
            {
                if (hConnectionStatus != value)
                {
                    hConnectionStatus = value;
                    if (OnScalesConnectionStatusChanged != null)
                    {
                        try
                        {
                            OnScalesConnectionStatusChanged(this, ConnectionStatus);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private DateTime hLastPacketTimestamp = DateTime.MinValue;
        public DateTime LastPacketTimestamp 
        {
            get { return hLastPacketTimestamp; }
        }

        private TimeSpan hFreeze = TimeSpan.MinValue;
        public TimeSpan Freeze
        {
            get { return hFreeze; }
        }

        private TimeSpan hTimeout = TimeSpan.FromMilliseconds(500);
        public TimeSpan Timeout 
        {
            get { return hTimeout; }
            set { hTimeout = value; }
        }

        private TimeSpan hExposure = TimeSpan.FromMilliseconds(15000);
        public TimeSpan Exposure
        {
            get { return hExposure; }
            set { hExposure = value; }
        }
    }

    public class TcpScales : IScales
    {
        private TcpClient client;
        private NetworkStream netStream;

        private NxDataFrame nxFrame;

        protected System.Threading.Timer checker = null;
        TimeSpan checkerSpan = TimeSpan.FromMilliseconds(500);

        public event ScalesEventWeightChanged OnScalesEventWeightChanged;
        public event ScalesEventStatusChanged OnScalesEventStatusChanged;
        public event ScalesEventDataReceived OnScalesEventDataReceived;
        public event ScalesEventRawDataReceived OnScalesEventRawDataReceived;
        public event ScalesEventConnectionStatusChanged OnScalesConnectionStatusChanged;

        public TcpScales()
        {
            nxFrame = new NxDataFrame();
            checker = new System.Threading.Timer((unused) => checkOnlineStatus(), null, System.Threading.Timeout.Infinite, (int)checkerSpan.TotalMilliseconds);
        }

        ~TcpScales()
        {
            Stop();

            if (checker != null)
            {
                checker.Dispose();
                checker = null;
            }
        }

        protected void checkOnlineStatus()
        {
            double interval = (DateTime.Now - hLastPacketTimestamp).TotalMilliseconds;
            if (interval > hTimeout.TotalMilliseconds)
            {
                ConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;
                hTerminalStatus = NxDeviceState.DS_ERROR;
                hFreeze = DateTime.Now - hLastPacketTimestamp;
                System.Diagnostics.Trace.WriteLine(string.Format("get offline. timeout {0:n0}ms", interval));

                if (hFreeze > TimeSpan.FromMinutes(1))
                {
                    LogManager.GetCurrentClassLogger().Info(string.Format("Канал {0} время ожидания достигло {1} секунд. Перезапуск.", hSettings.Address, hFreeze.TotalSeconds));
                    Start();
                }
            }
            else
            {
                hFreeze = TimeSpan.MinValue;
                ConnectionStatus = ScalesComponent.ConnectionStatus.CS_ONLINE;
            }
        }

        void NewDataRecieved(byte[] data)
        {
            hLastPacketTimestamp = DateTime.Now;
            hFreeze = TimeSpan.MinValue;

            if (ConnectionStatus == ScalesComponent.ConnectionStatus.CS_OFFLINE)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Warming up, Terminal STATE was {0}", TerminalStatus));
            }

            ConnectionStatus = ScalesComponent.ConnectionStatus.CS_ONLINE;

            bool decoded = false;
            try
            {
                decoded = nxFrame.Decode(data);
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Debug("Ошибка декодирования данных", ex);
                System.Diagnostics.Trace.TraceError("Ошибка декодирования данных {0}", ex.Message);
                decoded = false;
            }

            if (decoded)
            {
                bool bWeightChanged = hWeight != nxFrame.Weight;
                hWeight = nxFrame.Weight;

                if (OnScalesEventDataReceived != null)
                {
                    try
                    {
                        OnScalesEventDataReceived(this, Weight);
                    }
                    catch
                    {
                    }
                }

                if (bWeightChanged)
                {
                    if (OnScalesEventWeightChanged != null)
                    {
                        try
                        {
                            OnScalesEventWeightChanged(this, Weight);
                        }
                        catch
                        {
                        }
                    }
                }

                if (hTerminalStatus != nxFrame.State)
                {
                    hTerminalStatus = nxFrame.State;

                    if (OnScalesEventStatusChanged != null)
                    {
                        try
                        {
                            OnScalesEventStatusChanged(this, hTerminalStatus);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if (OnScalesEventRawDataReceived != null)
            {
                try
                {
                    String rawData = Encoding.ASCII.GetString(data);
                    OnScalesEventRawDataReceived(this, rawData);
                }
                catch
                {
                }
            }
        }

        private double hWeight = 0.0;
        public double Weight
        {
            get { return hWeight; }
        }

        CancellationTokenSource token = null;

        public void Reader(CancellationToken cancellationToken)
        {
            var addr = new Regex(@"(?<ip>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}):(?<port>\d{1,6})").Match(hSettings.Address);
            String host = addr.Success ? addr.Groups["ip"].Value : String.Empty;
            int port = addr.Success ? Convert.ToInt16(addr.Groups["port"].Value) : 0;

            while (!cancellationToken.IsCancellationRequested)
            {
                //while (client == null && !cancellationToken.IsCancellationRequested)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("time: {0:HH:mm:ss}, trying to connect...", DateTime.Now));
                    LogManager.GetCurrentClassLogger().Info(string.Format("Попытка установить соединение {0}", hSettings.Address));

                    try
                    {
                        client = new TcpClient(host, port);
                        if (client.Connected)
                        {
                            netStream = client.GetStream();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(string.Format("time: {0:HH:mm:ss}, connection error: {1}", DateTime.Now, ex.Message));
                        
                        LogManager.GetCurrentClassLogger().Debug(ex, string.Format("Ошибка установки соединения {0}", hSettings.Address));

                        if (netStream != null)
                        {
                            try
                            {
                                netStream.Dispose();
                                netStream = null;
                            }
                            catch
                            {
                            }
                        }

                        if (client != null)
                        {
                            try
                            {
                                client.Close();
                                client = null;
                            }
                            catch
                            {
                            }
                        }

                        //Thread.Sleep(1000);
                    }
                }

                if (client == null)
                {
                    System.Diagnostics.Trace.WriteLine("cleint == null, exit from reader");
                    return;
                }

                if (client.Connected)
                {
                    LogManager.GetCurrentClassLogger().Info(string.Format("Соединение {0} установлено", hSettings.Address));

                    byte[] readBuffer = new byte[client.ReceiveBufferSize];

                    while (netStream.CanRead && !cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            do
                            {
                                int numberOfBytesRead = -1;

                                numberOfBytesRead = netStream.Read(readBuffer, 0, readBuffer.Length);
                                if (numberOfBytesRead > 0)
                                {
                                    //LogManager.GetCurrentClassLogger().Info(string.Format("Данные: |{0}|", Encoding.ASCII.GetString(readBuffer, 0, numberOfBytesRead)));
                                    
                                    System.Diagnostics.Trace.WriteLine(string.Format("time: {0:HH:mm:ss}, bytes in buffer: {1}, data: |{2}|", DateTime.Now, numberOfBytesRead, Encoding.ASCII.GetString(readBuffer, 0, numberOfBytesRead)));
                                    System.Diagnostics.Trace.WriteLine("");

                                    var buffer = new byte[numberOfBytesRead];
                                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, numberOfBytesRead);
                                    NewDataRecieved(buffer);
                                }

                            } while (netStream.DataAvailable && !cancellationToken.IsCancellationRequested);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine(string.Format("time: {0:HH:mm:ss}, socket error: {1}", DateTime.Now, ex.Message));
                            LogManager.GetCurrentClassLogger().Debug(ex, string.Format("Ошибка чтения данных {0}", hSettings.Address));
                            break;
                        }
                    }

                    LogManager.GetCurrentClassLogger().Info(string.Format("Закрытие соединения {0}", hSettings.Address));
                    if (netStream != null)
                    {
                        try
                        {
                            netStream.Dispose();
                            netStream = null;
                        }
                        catch
                        {
                            netStream = null;
                        }
                    }

                    if (client != null)
                    {
                        try
                        {
                            client.Close();
                            client = null;
                        }
                        catch
                        {
                            client = null;
                        }
                    }
                }
            }
        }

        public void Start()
        {
            Stop();

            hConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;

            token = new CancellationTokenSource();
            Task.Run(() => Reader(token.Token));

            checker.Change(0, (int)checkerSpan.TotalMilliseconds);
        }

        public void Stop()
        {
            try
            {
                hLastPacketTimestamp = DateTime.Now;
                if (checker != null)
                    checker.Change(System.Threading.Timeout.Infinite, (int)checkerSpan.TotalMilliseconds);
            }
            catch
            {
            }

            try
            {
                if (token != null)
                {
                    token.Cancel();
                    token = null;
                }
            }
            catch
            {
            }

            ConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;
        }

        private String hId;
        public String Id
        {
            get { return hId; }
            set { hId = value; }
        }

        protected TcpChannelSettings hSettings;
        public BaseChannelSettings Settings
        {
            get
            {
                return hSettings;
            }

            set
            {
                hSettings = value as TcpChannelSettings;  
            }
        }

        private NxDeviceState hTerminalStatus = NxDeviceState.DS_ERROR;
        public NxDeviceState TerminalStatus
        {
            get { return hTerminalStatus; }
        }

        private ConnectionStatus hConnectionStatus = ConnectionStatus.CS_OFFLINE;
        public ConnectionStatus ConnectionStatus
        {
            get
            {
                return hConnectionStatus;
            }

            private set
            {
                if (hConnectionStatus != value)
                {
                    hConnectionStatus = value;
                    if (OnScalesConnectionStatusChanged != null)
                    {
                        try
                        {
                            OnScalesConnectionStatusChanged(this, ConnectionStatus);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private TimeSpan hFreeze = TimeSpan.MinValue;
        public TimeSpan Freeze
        {
            get { return hFreeze; }
        }

        private DateTime hLastPacketTimestamp = DateTime.Now;
        public DateTime LastPacketTimestamp
        {
            get { return hLastPacketTimestamp; }
        }

        private TimeSpan hTimeout = TimeSpan.FromMilliseconds(500);
        public TimeSpan Timeout
        {
            get { return hTimeout; }
            set { hTimeout = value; }
        }

        private TimeSpan hExposure = TimeSpan.FromMilliseconds(15000);
        public TimeSpan Exposure
        {
            get { return hExposure; }
            set { hExposure = value; }
        }
    }

    public class ManualScales : IScales
    {
        protected System.Threading.Timer checker = null;
        TimeSpan checkerSpan = TimeSpan.FromMilliseconds(500);

        public event ScalesEventWeightChanged OnScalesEventWeightChanged;
        public event ScalesEventStatusChanged OnScalesEventStatusChanged;
        public event ScalesEventDataReceived OnScalesEventDataReceived;
        public event ScalesEventRawDataReceived OnScalesEventRawDataReceived;
        public event ScalesEventConnectionStatusChanged OnScalesConnectionStatusChanged;

        public ManualScales()
        {
            checker = new System.Threading.Timer((unused) => checkOnlineStatus(), null, System.Threading.Timeout.Infinite, (int)checkerSpan.TotalMilliseconds);
            ConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;
        }

        ~ManualScales()
        {
            Stop();

            if (checker != null)
            {
                checker.Dispose();
                checker = null;
            }
        }

        protected void checkOnlineStatus()
        {
            DateTime now = DateTime.Now;
            DateTime last = hLastPacketTimestamp;
            if ((now - last) > hTimeout)
            {
                System.Diagnostics.Debug.WriteLine("OFFLINE channel timeout TotalMilliseconds " + (now - hLastPacketTimestamp).TotalMilliseconds);
                ConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;
            }
            else
            {
                ConnectionStatus = ScalesComponent.ConnectionStatus.CS_ONLINE;
            }
        }

        protected void OnDataReceived(Double newWeight, NxDeviceState newStatus)
        {
            hLastPacketTimestamp = DateTime.Now;
            bool bWeightChanged = hWeight != newWeight;
            hWeight = newWeight;

            if (OnScalesEventDataReceived != null)
            {
                try
                {
                    OnScalesEventDataReceived(this, Weight);
                }
                catch
                {
                }
            }

            if (bWeightChanged)
            {
                if (OnScalesEventWeightChanged != null)
                {
                    try
                    {
                        OnScalesEventWeightChanged(this, Weight);
                    }
                    catch
                    {
                    }
                }
            }

            if (hTerminalStatus != newStatus)
            {
                hTerminalStatus = newStatus;
                if (OnScalesEventStatusChanged != null)
                {
                    try
                    {
                        OnScalesEventStatusChanged(this, newStatus);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private double hWeight = 0.0;
        public double Weight
        {
            get { return hWeight; }
        }

        public void Start()
        {
            hConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;
            checker.Change(0, (int)checkerSpan.TotalMilliseconds);
            Program.subscribers.Add(OnMessage);
        }

        protected void OnMessage(String msid, String data)
        {
            if (msid == Id)
            {
                OnDataReceived(StringUtils.GetParameter<double>("weight", data), StringUtils.GetParameter<NxDeviceState>("state", data));
            }
        }

        public void Stop()
        {
            try
            {
                Program.subscribers.Remove(OnMessage);

                if (checker != null)
                    checker.Change(System.Threading.Timeout.Infinite, (int)checkerSpan.TotalMilliseconds);

                ConnectionStatus = ScalesComponent.ConnectionStatus.CS_OFFLINE;
            }
            catch
            {

            }
        }

        private String hId;
        public String Id
        {
            get { return hId; }
            set { hId = value; }
        }

        private SerialPortSettings hSettings = new SerialPortSettings();
        public BaseChannelSettings Settings
        {
            get { return hSettings; }
            set { hSettings = value as SerialPortSettings; }
        }

        private NxDeviceState hTerminalStatus = NxDeviceState.DS_NONE;
        public NxDeviceState TerminalStatus
        {
            get { return hTerminalStatus; }
        }

        private ConnectionStatus hConnectionStatus = ConnectionStatus.CS_OFFLINE;
        public ConnectionStatus ConnectionStatus
        {
            get
            {
                return hConnectionStatus;
            }

            private set
            {
                if (hConnectionStatus != value)
                {
                    hConnectionStatus = value;
                    if (OnScalesConnectionStatusChanged != null)
                    {
                        try
                        {
                            OnScalesConnectionStatusChanged(this, ConnectionStatus);
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private TimeSpan hFreeze = TimeSpan.MinValue;
        public TimeSpan Freeze
        {
            get { return hFreeze; }
        }

        private DateTime hLastPacketTimestamp = DateTime.MinValue;
        public DateTime LastPacketTimestamp
        {
            get { return hLastPacketTimestamp; }
        }

        private TimeSpan hTimeout = TimeSpan.FromMilliseconds(500);
        public TimeSpan Timeout
        {
            get { return hTimeout; }
            set { hTimeout = value; }
        }

        private TimeSpan hExposure = TimeSpan.FromMilliseconds(15000);
        public TimeSpan Exposure
        {
            get { return hExposure; }
            set { hExposure = value; }
        }
    }

    public class StringUtils
    {
        /// <summary>
        /// Разбирает строку вида: data source=sss;user id=uuuu;password=;initial catalog=vvv;Persist Security Info=true;
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<String, String>> EnumParameters(String settings)
        {
            Regex template = new Regex(@"(?<name>(\w+|\s+)+)\s*=\s*(?<value>[^;]+?)\s*(;|$)");
            foreach (Match item in template.Matches(settings))
            {
                yield return new KeyValuePair<String, String>(item.Groups["name"].Value, item.Groups["value"].Value);
            }

            yield break;
        }

        public static bool ParameterExist(String parameter, String source)
        {
            Regex template = new Regex(@"(?<name>(\w+|\s+)+)\s*=\s*(?<value>[^;]+?)\s*(;|$)");
            foreach (Match item in template.Matches(source))
            {
                if (item.Groups["name"].Value == parameter)
                    return true;
            }

            return false;
        }

        public static T GetParameter<T>(String parameter, String source)
        {
            Regex template = new Regex(@"(?<name>(\w+|\s+)+)\s*=\s*(?<value>[^;]+?)\s*(;|$)");

            return (T)RestoreValue(typeof(T), (from Match tmp in template.Matches(source) where tmp.Groups["name"].Value == parameter select tmp.Groups["value"].Value).FirstOrDefault());
        }

        public static Object GetParameter(Type targetType, String parameter, String source)
        {
            Regex template = new Regex(@"(?<name>(\w+|\s+)+)\s*=\s*(?<value>[^;]+?)\s*(;|$)");

            return RestoreValue(targetType, (from Match tmp in template.Matches(source) where tmp.Groups["name"].Value == parameter select tmp.Groups["value"].Value).FirstOrDefault());
        }

        public static Object RestoreValue(Type target_type, String text_value)
        {
            Object targetValue = null;

            if (text_value != null)
            {
                if (target_type.IsEnum)
                {
                    targetValue = Enum.Parse(target_type, text_value);
                }
                else if (target_type == typeof(Type))
                {
                    targetValue = (from assembly in AppDomain.CurrentDomain.GetAssemblies() from type in assembly.GetTypes() where type.FullName == text_value select type).FirstOrDefault();
                }
                else if (target_type == typeof(decimal) || target_type == typeof(double))
                {
                    string v = text_value;
                    string sep = System.Threading.Thread.CurrentThread.CurrentUICulture.NumberFormat.NumberDecimalSeparator;
                    v = v.Replace(",", sep);
                    v = v.Replace(".", sep);
                    targetValue = Convert.ChangeType(v, target_type);
                }
                else if (target_type == typeof(Guid))
                {
                    targetValue = new Guid(text_value);
                }
                else
                {
                    targetValue = Convert.ChangeType(text_value, target_type);
                }
            }

            return targetValue;
        }
    }

}
