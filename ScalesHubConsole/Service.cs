using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace ScalesHubConsole
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public class ScalesDataService : IScalesHubService, IDisposable
    {
        public ScalesDataService()
        {
            Startup();
        }

        public void Startup()
        {
            LogManager.GetCurrentClassLogger().Info("Сервис ScalesDataService запущен");
        }

        public void Shutdown()
        {
            LogManager.GetCurrentClassLogger().Info("Сервис ScalesDataService остановлен");
        }

        public void Dispose()
        {
            Shutdown();
        }

        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = "scales")]
        public IEnumerable<ScalesMeasureData> EnumScales()
        {
            LogManager.GetCurrentClassLogger().Info("Вызов EnumScales");
            return from channel in Program.MeasuringChannels.ToArray()
                   select new ScalesMeasureData
                   {
                       Timestamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local),
                       TimestampUTC = DateTime.UtcNow,
                       Code = channel.Code,
                       Name = channel.Name,
                       MeasureTime = channel.MeasureTime == DateTime.MinValue ? null : (DateTime?)channel.MeasureTime,
                       MeasureTimeStr = channel.MeasureTime == DateTime.MinValue ? String.Empty : String.Format("{0:dd.MM.yyyy HH:mm:ss}", channel.MeasureTime),
                       MeasureValue = channel.MeasureValue,
                       State = Convert.ToString(channel.State),
                       ScalesState = channel.DeviceStateString,
                       LastStableTime = channel.AcceptedTime,
                       LastStableTimeStr = channel.AcceptedTime == null ? String.Empty : String.Format("{0:dd.MM.yyyy HH:mm:ss}", channel.AcceptedTime),
                       LastStableValue = channel.AcceptedValue
                   };
        }

        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "scales/{code}")]
        public ScalesMeasureData GetScalesMeasureData(String code)
        {
            //OperationContext.Current
            var channel = Program.MeasuringChannels.FirstOrDefault(tmp => tmp.Code == code);
            if (channel != null)
            {
                LogManager.GetCurrentClassLogger().Info(string.Format("Вызов GetScalesMeasureData(code='{0}') MeasureTime={1}, MeasureValue={2}", 
                    code,
                    (channel.MeasureTime == DateTime.MinValue ? String.Empty : String.Format("{0:dd.MM.yyyy HH:mm:ss}", channel.MeasureTime)), 
                    channel.MeasureValue));

                return new ScalesMeasureData
                {
                    Timestamp = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local),
                    TimestampUTC = DateTime.UtcNow,
                    Code = channel.Code,
                    Name = channel.Name,
                    MeasureTime = channel.MeasureTime == DateTime.MinValue ? null : (DateTime?)channel.MeasureTime,
                    MeasureTimeStr = channel.MeasureTime == DateTime.MinValue ? String.Empty : String.Format("{0:dd.MM.yyyy HH:mm:ss}", channel.MeasureTime),
                    MeasureValue = channel.MeasureValue,
                    State = Convert.ToString(channel.State),
                    ScalesState = channel.DeviceStateString,
                    LastStableTime = channel.AcceptedTime,
                    LastStableTimeStr = channel.AcceptedTime == null ? String.Empty : String.Format("{0:dd.MM.yyyy HH:mm:ss}", channel.AcceptedTime),
                    LastStableValue = channel.AcceptedValue
                };
            }

            LogManager.GetCurrentClassLogger().Info(string.Format("Вызов GetScalesMeasureData(code='{0}') канал не найден!", code));
            return null;
        }
    }

}
