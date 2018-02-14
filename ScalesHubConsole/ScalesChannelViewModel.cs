using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LiteDB;
using Newtonsoft.Json;
using Nostrum.ScalesComponent.Serial;

namespace ScalesHubConsole
{
    public class ScalesChannelViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int hId;
        private String hCode;
        private String hName;
        private string hDecoder;
        private Boolean hActive;
        private String hDescription;
        private String hSettingsType;

        private Decimal hTriggerLoading;
        private Decimal hTriggerEmpty;

        private TimeSpan hExposure;
        private TimeSpan hTimeout;

        private String hPortName;
        private String hAddress;
        private int hBaudRate;
        private Parity hParity;
        private int hDataBits;
        private StopBits hStopBits;

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

        public String Decoder
        {
            get { return hDecoder; }
            set { SetField(ref hDecoder, value); }
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

        public String SettingsType
        {
            get { return hSettingsType; }
            set { SetField(ref hSettingsType, value); }
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
            set 
            { 
                if (SetField(ref hExposure, value))
                    OnPropertyChanged("ExposureMilliseconds"); 
            }
        }

        public double ExposureMilliseconds
        {
            get { return hExposure.TotalMilliseconds; }
            set { SetField(ref hExposure, TimeSpan.FromMilliseconds(value)); }
        }

        public TimeSpan Timeout
        {
            get { return hTimeout; }
            set 
            {
                if (SetField(ref hTimeout, value))
                    OnPropertyChanged("TimeoutMilliseconds");
            }
        }

        public double TimeoutMilliseconds
        {
            get { return hTimeout.TotalMilliseconds; }
            set { SetField(ref hTimeout, TimeSpan.FromMilliseconds(value)); }
        }

        public String PortName
        {
            get { return hPortName; }
            set { SetField(ref hPortName, value); }
        }

        public String Address
        {
            get { return hAddress; }
            set { SetField(ref hAddress, value); }
        }

        public int BaudRate
        {
            get { return hBaudRate; }
            set { SetField(ref hBaudRate, value); }
        }

        public Parity Parity
        {
            get { return hParity; }
            set { SetField(ref hParity, value); }
        }

        public int DataBits
        {
            get { return hDataBits; }
            set { SetField(ref hDataBits, value); }
        }

        public StopBits StopBits
        {
            get { return hStopBits; }
            set { SetField(ref hStopBits, value); }
        }

        public static ScalesChannelViewModel LoadFromDb(int id)
        {
            ScalesChannelViewModel result = null;

            using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
            {
                ScalesDescriptor sd = dba.GetCollection<ScalesDescriptor>("scales").FindById(id);
                if (sd != null)
                {
                    ScalesChannelViewModel tmp = new ScalesChannelViewModel();
                    tmp.Load(sd);
                    result = tmp;
                }
            }

            return result;
        }

        public void Load(ScalesDescriptor sd)
        {
            if (sd.StreamType == "serial")
            {
                SerialPortSettings settings = JsonConvert.DeserializeObject<SerialPortSettings>(sd.Settings);
                Id = sd.Id;
                Code = sd.Code;
                Name = sd.Name;
                Decoder = sd.Decoder;
                Active = sd.Active;
                Description = sd.Description;
                Exposure = TimeSpan.FromMilliseconds(sd.Exposure);
                Timeout = TimeSpan.FromMilliseconds(sd.Timeout);
                TriggerLoading = sd.TriggerLoading;
                TriggerEmpty = sd.TriggerEmpty;

                SettingsType = settings.SettingsType;
                PortName = settings.PortName;
                Address = "";
                BaudRate = settings.BaudRate;
                DataBits = settings.DataBits;
                Parity = settings.Parity;
                StopBits = settings.StopBits;
            }

            if (sd.StreamType == "tcp")
            {
                TcpChannelSettings settings = JsonConvert.DeserializeObject<TcpChannelSettings>(sd.Settings);
                Id = sd.Id;
                Code = sd.Code;
                Name = sd.Name;
                Decoder = sd.Decoder;
                Active = sd.Active;
                Description = sd.Description;
                Exposure = TimeSpan.FromMilliseconds(sd.Exposure);
                Timeout = TimeSpan.FromMilliseconds(sd.Timeout);
                TriggerLoading = sd.TriggerLoading;
                TriggerEmpty = sd.TriggerEmpty;

                SettingsType = settings.SettingsType;
                PortName = "";
                Address = settings.Address;
                BaudRate = 0;
                DataBits = 0;
                Parity = 0;
                StopBits = 0;
            }

        }

        public IEnumerable<ValidationResult> Validate(LiteDatabase dba = null)
        {
            if (dba == null)
                dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath));

            List<ValidationResult> result = new List<ValidationResult>();
            if (String.IsNullOrWhiteSpace(Code))
                result.Add(new ValidationResult { PropertyName = "Code", Message = "Код должен быть заполнен!" });
            else
            {
                if (dba.GetCollection<ScalesDescriptor>("scales").Find(x => x.Code == Code && x.Id != Id).Count() != 0)
                    result.Add(new ValidationResult { PropertyName = "Code", Message = "Код должен быть уникальным!" });
            }

            if (String.IsNullOrWhiteSpace(Name))
                result.Add(new ValidationResult { PropertyName = "Name", Message = "Наименование должно быть заполнено!" });
            else
            {
                if (dba.GetCollection<ScalesDescriptor>("scales").Find(x => x.Name == Name && x.Id != Id).Count() != 0)
                    result.Add(new ValidationResult { PropertyName = "Name", Message = "Наименование должно быть уникальным!" });
            }

            return result;
        }

        public bool Update()
        {
            using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
            {
                var vr = Validate(dba);
                if (vr.Count() != 0)
                    return false;

                ScalesDescriptor sd = dba.GetCollection<ScalesDescriptor>("scales").FindById(Id);
                if (sd != null)
                {
                    sd.Code = this.Code;
                    sd.Name = this.Name;
                    sd.Decoder = this.Decoder;
                    sd.Active = this.Active;
                    sd.Description = this.Description;
                    sd.Exposure = Exposure.TotalMilliseconds;
                    sd.Timeout = Timeout.TotalMilliseconds;
                    sd.TriggerLoading = this.TriggerLoading;
                    sd.TriggerEmpty = this.TriggerEmpty;
                    sd.StreamType = this.SettingsType;
                    sd.Settings = this.SettingsType == "serial" ?
                        JsonConvert.SerializeObject(new SerialPortSettings
                        {
                            SettingsType = this.SettingsType,
                            PortName = this.PortName,
                            BaudRate = this.BaudRate,
                            DataBits = this.DataBits,
                            Parity = this.Parity,
                            StopBits = this.StopBits
                        }) :
                        JsonConvert.SerializeObject(new TcpChannelSettings
                        {
                            SettingsType = this.SettingsType,
                            Address = this.Address,
                        });

                    return dba.GetCollection<ScalesDescriptor>("scales").Update(sd);
                }

                return false;
            }
        }

        public bool Register()
        {
            using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
            {
                var vr = Validate(dba);
                if (vr.Count() != 0)
                    return false;

                var newId = dba.GetCollection<ScalesDescriptor>("scales").Insert(new ScalesDescriptor
                {
                    Code = this.Code,
                    Name = this.Name,
                    Decoder = this.Decoder,
                    Active = this.Active,
                    Description = this.Description,
                    Exposure = this.Exposure.TotalMilliseconds,
                    Timeout = this.Timeout.TotalMilliseconds,
                    TriggerLoading = this.TriggerLoading,
                    TriggerEmpty = this.TriggerEmpty,
                    StreamType = this.SettingsType,
                    Settings = this.SettingsType == "serial" ? 
                    JsonConvert.SerializeObject(new SerialPortSettings { 
                        SettingsType = this.SettingsType,
                        PortName = this.PortName, BaudRate = this.BaudRate, DataBits = this.DataBits, Parity = this.Parity, StopBits = this.StopBits
                    }) :
                    JsonConvert.SerializeObject(new TcpChannelSettings {
                        SettingsType = this.SettingsType,
                        Address = this.Address,
                    })
                });

                Id = newId.AsInt32;
            }

            return true;
        }
    }

    public class ValidationResult
    {
        public String PropertyName { get; set; }
        public String Message { get; set; }
    }

}
