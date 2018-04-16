using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using LiteDB;
using Newtonsoft.Json;
using NLog;
using Nostrum.ScalesComponent;
using Nostrum.ScalesComponent.Serial;
using ScalesHubPlugin;

namespace ScalesHubConsole
{
    public class HostPlatform
    {
        protected List<ServiceHost> hosts = new List<ServiceHost>();
        public List<IScales> eventSource = new List<IScales>();

        protected void StartService(Type serviceType)
        {
            try
            {
                LogManager.GetCurrentClassLogger().Info("Запуск сервиса " + serviceType.FullName);

                Stopwatch sw = new Stopwatch();
                sw.Start();

                ServiceHost tmp = new ServiceHost(serviceType);
                hosts.Add(tmp);
                tmp.Open();

                sw.Stop();

                LogManager.GetCurrentClassLogger().Info(String.Format("Сервис {0} запущен. [{1}ms]", serviceType.FullName, sw.ElapsedMilliseconds));
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(String.Format("Ошибка создания сервиса [{0}]", serviceType.FullName), ex);
            }
        }
        ///// <summary>
        ///// Генерирует декодер для весов 
        ///// </summary>
        ///// <param name="decoderType"></param>
        ///// <returns></returns>
        //NxDataFrame GenerateScaleDecoder(string decoderType)
        //{
        //    NxDataFrame scaleDecoder = null;
        //    switch (decoderType)
        //    {
        //        case "НВТ-1":
        //            scaleDecoder = new DataFrameNVT1();
        //            break;
        //        case "НВТ-3":
        //            scaleDecoder = new DataFrameNVT3();
        //            break;
        //        case "НВТ-9":
        //            scaleDecoder = new DataFrameNVT9();
        //            break;
        //        default:
        //            scaleDecoder = new NxDataFrame();
        //            break;
        //    }
        //    return scaleDecoder;
        //}
        ///// <summary>
        ///// Получение декодера через атрибут
        ///// </summary>
        ///// <param name="scaleType"></param>
        ///// <returns></returns>
        //NxDataFrame GetDecoderByAttribute(Type scaleType)
        //{
        //    //получение атрибута
        //    var decoderAttribute = scaleType.GetCustomAttributes(typeof(DecoderAttribute), true).FirstOrDefault();
        //    if (decoderAttribute != null)
        //    {
        //        var decoderName = (decoderAttribute as DecoderAttribute).DecoderName;
        //        return GenerateScaleDecoder(decoderName);                
        //    }
        //    return null;
        //}
        public IScales SetupSource(ScalesDescriptor sd)
        {
            //определение декодера по настройкам ScalesDescriptor
            IDataFrame decoder = Program.Plugins.DecoderModules.Where(x => x.Metadata.Name == sd.Decoder).FirstOrDefault()?.Value;//GenerateScaleDecoder(sd.Decoder);

            if (sd.StreamType == "serial")
            {
                SerialPortSettings settings = JsonConvert.DeserializeObject<SerialPortSettings>(sd.Settings);

                IScales scales = eventSource.FirstOrDefault(tmp => tmp.Id == settings.PortName);
                if (scales == null && sd.Active)
                {
                    LogManager.GetLogger(Application.ProductName).Info(string.Format("Настройка источника {0}", sd.Settings));

                    if (settings.PortName.StartsWith("MAN"))
                    {                        
                        ManualScales tmp = new ManualScales(); 
                        tmp.Id = settings.PortName;
                        tmp.Settings = settings;
                        tmp.Exposure = TimeSpan.FromMilliseconds(sd.Exposure);
                        tmp.Timeout = TimeSpan.FromMilliseconds(sd.Timeout);
                        //var attributes = tmp.GetType().GetCustomAttributes(typeof(DecoderAttribute), true); 
                        scales = tmp;

                        if (Program.mainForm != null)
                        {
                            ManualSourceControl ctrl = new ManualSourceControl(settings.PortName);
                            ctrl.Dock = DockStyle.Fill;
                            var panel = Program.mainForm.dockManager.AddPanel(DevExpress.XtraBars.Docking.DockingStyle.Float);
                            panel.Text = settings.PortName;
                            panel.Tag = settings.PortName;
                            panel.Controls.Add(ctrl);
                            panel.ClosingPanel += (sender, e) =>
                            {
                                e.Cancel = true;
                                XtraMessageBox.Show("Необходимо деактивировать связанные каналы!", Program.mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            };
                        }
                    }
                    else
                    {
                        try
                        {
                            //var tempDecoder = GetDecoderByAttribute(typeof(Nostrum.ScalesComponent.Scales));
                            //if (decoder?.GetType() != tempDecoder?.GetType())
                            //{
                            //    //throw new Exception();
                            //}    
                            Nostrum.ScalesComponent.Scales tmp = new Nostrum.ScalesComponent.Scales(decoder);
                            tmp.Id = settings.PortName;
                            tmp.Settings = settings;
                            tmp.Exposure = TimeSpan.FromMilliseconds(sd.Exposure);
                            tmp.Timeout = TimeSpan.FromMilliseconds(sd.Timeout);
                            scales = tmp;
                        }
                        catch
                        {
                            scales = null;
                        }
                    }

                    if (scales != null)
                    {
                        try
                        {
                            scales.Start();
                            eventSource.Add(scales);
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetCurrentClassLogger().Error("Ошибка запуска источника\r\n", ex);
                            scales = null;
                        }
                    }
                }

                return scales;
            }

            if (sd.StreamType == "tcp")
            {
                TcpChannelSettings settings = JsonConvert.DeserializeObject<TcpChannelSettings>(sd.Settings);

                IScales scales = eventSource.FirstOrDefault(tmp => tmp.Id == settings.Address);
                if (scales == null && sd.Active)
                {
                    LogManager.GetCurrentClassLogger().Info(string.Format("Настройка источника {0}", sd.Settings));

                    try
                    {
                        //var tempDecoder = GetDecoderByAttribute(typeof(TcpScales));
                        //if (decoder?.GetType() != tempDecoder?.GetType())
                        //{
                        //    //throw new Exception();
                        //}
                        TcpScales tmp = new TcpScales(decoder);
                        tmp.Id = settings.Address;
                        tmp.Settings = settings;
                        tmp.Exposure = TimeSpan.FromMilliseconds(sd.Exposure);
                        tmp.Timeout = TimeSpan.FromMilliseconds(sd.Timeout);
                        scales = tmp;
                    }
                    catch
                    {
                        scales = null;
                    }

                    if (scales != null)
                    {
                        try
                        {
                            scales.Start();
                            eventSource.Add(scales);
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger(Application.ProductName).Error("Ошибка запуска источника\r\n", ex);
                            scales = null;
                        }
                    }
                }

                return scales;
            }

            return null;
        }

        public void RemoveChannel(ScalesChannel channel)
        {
            if (channel != null)
            {
                var source = channel.EventSource;
                channel.EventSource = null;
                channel.Active = false;

                using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
                {
                    dba.GetCollection<ScalesDescriptor>("scales").Delete(x => x.Id == channel.Id);
                    dba.GetCollection<DbScalesMeasureData>("measure").Delete(x => x.ScalesId == channel.Id);
                }

                LogManager.GetLogger(Application.ProductName).Info(string.Format("Удаление канала {0} произошло успешно!", channel.Name));
                Program.MeasuringChannels.Remove(channel);

                if (source != null ? Program.MeasuringChannels.FirstOrDefault(tmp => tmp.EventSource == source) == null : false)
                {
                    if (source.Settings is SerialPortSettings)
                    {
                        if ((source.Settings as SerialPortSettings).PortName.StartsWith("MAN") && Program.mainForm != null)
                        {
                            var panel = Program.mainForm.dockManager.Panels.FirstOrDefault(x => Object.Equals(x.Tag, (source.Settings as SerialPortSettings).PortName));
                            if (panel != null)
                            {
                                Program.mainForm.dockManager.RemovePanel(panel);
                            }
                        }

                        LogManager.GetLogger(Application.ProductName).Info(string.Format("Остановка источника {0}", (source.Settings as SerialPortSettings).PortName));
                        source.Stop();
                        eventSource.Remove(source);
                    }

                    if (source.Settings is TcpChannelSettings)
                    {
                        LogManager.GetLogger(Application.ProductName).Info(string.Format("Остановка источника {0}", (source.Settings as TcpChannelSettings).Address));
                        source.Stop();
                        eventSource.Remove(source);
                    }

                }
            }
        }

        public void AddChannel(ScalesDescriptor sd, DbScalesMeasureData lastMeaure = null)
        {
            IScales scales = SetupSource(sd);
            ScalesChannel channel = null;

            if (sd.StreamType == "serial")
            {

                SerialPortSettings settings = JsonConvert.DeserializeObject<SerialPortSettings>(sd.Settings);

                channel = new ScalesChannel
                {
                    Id = sd.Id,
                    Code = sd.Code,
                    Name = string.Format("{0} ({1})", sd.Name, settings.PortName),
                    Active = scales != null ? sd.Active : false,
                    Description = sd.Description,
                    Exposure = TimeSpan.FromMilliseconds(sd.Exposure),
                    Timeout = TimeSpan.FromMilliseconds(sd.Timeout),
                    TriggerEmpty = sd.TriggerEmpty,
                    TriggerLoading = sd.TriggerLoading,
                    ChannelSettings = settings,
                    owner = Program.mainForm
                };
            }

            if (sd.StreamType == "tcp")
            {

                TcpChannelSettings settings = JsonConvert.DeserializeObject<TcpChannelSettings>(sd.Settings);

                channel = new ScalesChannel
                {
                    Id = sd.Id,
                    Code = sd.Code,
                    Name = string.Format("{0} ({1})", "TCP", settings.Address),
                    Active = scales != null ? sd.Active : false,
                    Description = sd.Description,
                    Exposure = TimeSpan.FromMilliseconds(sd.Exposure),
                    Timeout = TimeSpan.FromMilliseconds(sd.Timeout),
                    TriggerEmpty = sd.TriggerEmpty,
                    TriggerLoading = sd.TriggerLoading,
                    ChannelSettings = settings,
                    owner = Program.mainForm
                };
            }

            if (lastMeaure != null)
            {
                channel.AcceptedTime = lastMeaure.Time;
                channel.AcceptedValue = lastMeaure.Value;
            }

            Program.MeasuringChannels.Add(channel);

            channel.EventSource = scales;
        }

        public void DeactivateChannel(ScalesChannel channel)
        {
            if (channel != null)
            {
                var source = channel.EventSource;
                channel.EventSource = null;
                channel.Active = false;

                using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
                {
                    ScalesDescriptor sd = dba.GetCollection<ScalesDescriptor>("scales").FindById(channel.Id);
                    sd.Active = false;
                    dba.GetCollection<ScalesDescriptor>("scales").Update(sd);
                }

                if (source != null ? Program.MeasuringChannels.FirstOrDefault(tmp => tmp.EventSource == source) == null : false)
                {
                    if (source.Settings is SerialPortSettings)
                    {
                        if ((source.Settings as SerialPortSettings).PortName.StartsWith("MAN") && Program.mainForm != null)
                        {
                            var panel = Program.mainForm.dockManager.Panels.FirstOrDefault(x => Object.Equals(x.Tag, (source.Settings as SerialPortSettings).PortName));
                            if (panel != null)
                            {
                                Program.mainForm.dockManager.RemovePanel(panel);
                            }
                        }

                        LogManager.GetLogger(Application.ProductName).Info(string.Format("Остановка источника {0}", (source.Settings as SerialPortSettings).PortName));
                        source.Stop();
                        eventSource.Remove(source);
                    }

                    if (source.Settings is TcpChannelSettings)
                    {
                        LogManager.GetLogger(Application.ProductName).Info(string.Format("Остановка источника {0}", (source.Settings as TcpChannelSettings).Address));
                        source.Stop();
                        eventSource.Remove(source);
                    }


                    LogManager.GetLogger(Application.ProductName).Info(string.Format("Декативация канала {0} выполнена успешно!", channel.Name));
                }
            }
        }

        public void ActivateChannel(ScalesChannel channel)
        {
            if (channel != null)
            {
                using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
                {
                    ScalesDescriptor sd = dba.GetCollection<ScalesDescriptor>("scales").FindById(channel.Id);
                    sd.Active = true;
                    dba.GetCollection<ScalesDescriptor>("scales").Update(sd);

                    channel.EventSource = SetupSource(sd);
                    channel.Active = true;
                }

                LogManager.GetLogger(Application.ProductName).Info(string.Format("Активация канала {0} выполнена успешно!", channel.Name));
            }
        }

        public void LoadMetadata()
        {
            try
            {
                LogManager.GetLogger(Application.ProductName).Info("Загрузка метаданных каналов");
                using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
                {
                    foreach (var tmp in dba.GetCollection<ScalesDescriptor>("scales").FindAll())
                    {
                        LogManager.GetLogger(Application.ProductName).Info(string.Format("Загрузка настроек для {0} {1}", tmp.Name, tmp.Settings));

                        DbScalesMeasureData lastMeaure = dba.GetCollection<DbScalesMeasureData>("measure").Find(x => x.ScalesId == tmp.Id).OrderByDescending(x => x.Time).FirstOrDefault();
                        AddChannel(tmp, lastMeaure);
                    }

                }

                LogManager.GetLogger(Application.ProductName).Info("Загрузка метаданных каналов завершена");
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(Application.ProductName).Error("Ошибка загрузки метаданных", ex);
            }

        }

        public void Startup()
        {
            LogManager.GetLogger(Application.ProductName).Info("Инициализация службы");

            StartService(typeof(ScalesDataService));

            LogManager.GetLogger(Application.ProductName).Info("Инициализация службы завершена");
        }

        public void Shutdown()
        {
            LogManager.GetLogger(Application.ProductName).Info("Завершение работы службы...");

            foreach (var tmp in hosts)
            {
                try
                {
                    if (tmp.State == CommunicationState.Opened)
                    {
                        tmp.Close();
                    }
                }
                catch
                {
                }
            }

            hosts.Clear();

            LogManager.GetLogger(Application.ProductName).Info("Деактивация каналов...");
            foreach (var tmp in eventSource)
            {
                tmp.Stop();
            }

            eventSource.Clear();

            LogManager.GetLogger(Application.ProductName).Info("Работа службы завершена.");
        }

    }
}
