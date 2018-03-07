using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiteDB;
using NLog;
using Nostrum.ScalesComponent;
using Nostrum.ScalesComponent.Serial;
using ScalesHubPlugin;

namespace ScalesHubConsole
{

    public class ScalesChannel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Control owner = null;

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] String propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                Action action = () => handler(this, new PropertyChangedEventArgs(propertyName));
                if (owner != null)
                    owner.Invoke(action);
                else
                    action();
            }
        }

        public enum ChState
        {
            CH_ERROR,
            CH_WAITING,
            CH_WAITING_ZERO,
            CH_MEASURING,
            CH_EXPOSURING,
            CH_WEIGHT_ACCEPTED
        }

        protected IScales hEventSource = null;

        public IScales EventSource
        {
            get
            {
                return hEventSource;
            }

            set
            {
                if (hEventSource != value)
                {
                    if (hEventSource != null)
                    {
                        hEventSource.OnScalesEventDataReceived -= OnScalesEventDataReceived;
                        hEventSource.OnScalesConnectionStatusChanged -= OnScalesConnectionStatusChanged;
                        hEventSource.OnScalesEventStatusChanged -= OnScalesEventStatusChanged;
                    }

                    State = ChState.CH_ERROR;

                    hEventSource = value;

                    if (hEventSource != null)
                    {
                        State = hEventSource.ConnectionStatus == ConnectionStatus.CS_OFFLINE ? ChState.CH_ERROR : ChState.CH_WAITING;

                        hEventSource.OnScalesEventDataReceived += OnScalesEventDataReceived;
                        hEventSource.OnScalesConnectionStatusChanged += OnScalesConnectionStatusChanged;
                        hEventSource.OnScalesEventStatusChanged += OnScalesEventStatusChanged;
                    }
                }
            }
        }

        protected void ExposureData(decimal weight, DateTime time)
        {
            State = ChState.CH_WEIGHT_ACCEPTED;
            AcceptedValue = weight;
            AcceptedTime = time;

            LogManager.GetCurrentClassLogger().Info(string.Format("Канал {0} вес {1}. Получено подтверждение. Показание {2}", Name, AcceptedValue, MeasureValue));

            Task.Run(async delegate
            {
                await Task.Delay(TimeSpan.FromSeconds(1.5));
                if (State == ChState.CH_WEIGHT_ACCEPTED)
                    State = ChState.CH_WAITING_ZERO;
            });

            Task.Run(delegate
            {
                using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
                {
                    dba.GetCollection<DbScalesMeasureData>("measure").Insert(new DbScalesMeasureData
                    {
                        ScalesId = Id,
                        Time = (DateTime)AcceptedTime,
                        Value = AcceptedValue
                    });
                }
            });
        }

        CancellationTokenSource tokenSource = null;
        decimal exposuredWeight = 0;
        protected void OnScalesEventStatusChanged(object sender, NxDeviceState state)
        {
            DeviceState = state;

            /*
            if ((state & NxDataFrame.NxDeviceState.DS_STEADY) == NxDataFrame.NxDeviceState.DS_STEADY)
            {
                if (State == ChState.CH_MEASURING && MeasureValue > TriggerEmpty)
                {
                    if (tokenSource == null)
                    {
                        tokenSource = new CancellationTokenSource();
                        Task.Run(async delegate
                        {
                            bool exposured = true;
                            exposuredWeight = MeasureValue;

                            DateTime expStart = DateTime.Now;
                            State = ChState.CH_EXPOSURING;
                            Program.Log.Info(string.Format("Канал {0} вес {1}. Ожидание подтверждения. Интервал [{2}]", Name, MeasureValue, Exposure.ToString(@"ss\.fff")));
                            try
                            {
                                await Task.Delay(Exposure, tokenSource.Token);
                            }
                            catch
                            {
                                exposured = false;
                                if (State == ChState.CH_EXPOSURING)
                                    State = ChState.CH_MEASURING;
                            }

                            if (exposured && State == ChState.CH_EXPOSURING)
                                ExposureData(exposuredWeight, DateTime.Now);
                            else
                                Program.Log.Info(string.Format("Канал {0} вес {1}. Подтверждение НЕ получено. Интервал [{2}]. Выдержан [{3}]", Name, exposuredWeight, Exposure.ToString(@"ss\.fff"), (DateTime.Now - expStart).ToString(@"ss\.fff")));

                            exposuredWeight = 0;
                        });
                    }

                    return;
                }

                if (State == ChState.CH_WAITING_ZERO && MeasureValue <= TriggerEmpty)
                {
                    State = ChState.CH_WAITING;
                }
            }
            else
            {
                if (tokenSource != null)
                {
                    tokenSource.Cancel();
                    tokenSource.Dispose();
                    tokenSource = null;
                    exposuredWeight = 0;
                }
            }*/
        }

        protected void OnScalesConnectionStatusChanged(object sender, ConnectionStatus connectionStatus)
        {
            if (connectionStatus == ConnectionStatus.CS_ONLINE)
            {
                State = ChState.CH_WAITING;
                LogManager.GetLogger(Application.ProductName).Info(string.Format("Канал {0}. Статус ONLINE", Name));
            }

            if (connectionStatus == ConnectionStatus.CS_OFFLINE)
            {
                State = ChState.CH_ERROR;
                LogManager.GetLogger(Application.ProductName).Info(string.Format("Канал {0}. Статус OFFLINE", Name));

            }
        }

        protected void OnScalesEventDataReceived(object sender, double weight)
        {
            MeasureValue = (decimal)weight;
            MeasureTime = DateTime.Now;

            State = ChState.CH_MEASURING;

            /*
            if (MeasureValue > TriggerEmpty && State == ChState.CH_WAITING)
                State = ChState.CH_MEASURING;

            if (State == ChState.CH_EXPOSURING && MeasureValue != exposuredWeight)
                OnScalesEventStatusChanged(sender, NxDataFrame.NxDeviceState.DS_NONE);

            if (MeasureValue > TriggerEmpty && State == ChState.CH_MEASURING && (EventSource.TerminalStatus & NxDataFrame.NxDeviceState.DS_STEADY) == NxDataFrame.NxDeviceState.DS_STEADY)
                OnScalesEventStatusChanged(sender, NxDataFrame.NxDeviceState.DS_STEADY);

            if (MeasureValue <= TriggerEmpty && State == ChState.CH_MEASURING)
                State = ChState.CH_WAITING;

            if (MeasureValue <= TriggerEmpty && State == ChState.CH_WAITING_ZERO)
                State = ChState.CH_WAITING;*/
        }

        public ScalesChannel()
        {
            State = ChState.CH_ERROR;
        }

        private int hId;
        private String hCode;
        private String hName;
        private Boolean hActive;
        private String hDescription;

        private Decimal hTriggerLoading;
        private Decimal hTriggerEmpty;
        private TimeSpan hExposure;
        private TimeSpan hTimeout;

        protected DateTime? hAcceptedTime;
        protected decimal hAcceptedValue;

        protected DateTime hMeasureTime;
        protected decimal hMeasureValue;

        protected ChState hState;
        protected NxDeviceState hDeviceState;


        public int Id
        {
            get { return hId; }
            set { SetField(ref hId, value); }
        }

        public String Code
        {
            get { return hCode; }
            set { SetField(ref hCode, value); }
        }

        public String Name
        {
            get { return hName; }
            set { SetField(ref hName, value); }
        }

        public Boolean Active
        {
            get { return hActive; }
            set { SetField(ref hActive, value); }
        }

        public String Description
        {
            get { return hDescription; }
            set { SetField(ref hDescription, value); }
        }

        public Decimal TriggerLoading
        {
            get { return hTriggerLoading; }
            set { SetField(ref hTriggerLoading, value); }
        }

        public Decimal TriggerEmpty
        {
            get { return hTriggerEmpty; }
            set { SetField(ref hTriggerEmpty, value); }
        }

        public TimeSpan Exposure
        {
            get { return hExposure; }
            set { SetField(ref hExposure, value); }
        }

        public TimeSpan Timeout
        {
            get { return hTimeout; }
            set { SetField(ref hTimeout, value); }
        }

        public BaseChannelSettings ChannelSettings { get; set; }


        public DateTime? AcceptedTime
        {
            get { return hAcceptedTime; }
            set { SetField(ref hAcceptedTime, value); }
        }

        public decimal AcceptedValue
        {
            get { return hAcceptedValue; }
            set { SetField(ref hAcceptedValue, value); }
        }

        public DateTime MeasureTime
        {
            get { return hMeasureTime; }
            set { SetField(ref hMeasureTime, value); }
        }

        public decimal MeasureValue
        {
            get { return hMeasureValue; }
            set { SetField(ref hMeasureValue, value); }
        }

        public ChState State
        {
            get { return hState; }
            set 
            { 
                if (SetField(ref hState, value))
                {
                    //LogManager.GetLogger(Application.ProductName).Info(string.Format("ИЗМЕНЕНИЕ СОСТОЯНИЯ -> Канал {0}, показание {1}, состояние {2}", Name, MeasureValue, State));
                }
            }
        }

        public NxDeviceState DeviceState
        {
            get { return hDeviceState; }
            private set 
            { 
                if (SetField(ref hDeviceState, value))
                {
                    OnPropertyChanged("DeviceStateString");
                }

            }
        }

        public String DeviceStateString
        {
            get { return Convert.ToString(hDeviceState); }
        }
    }

}
