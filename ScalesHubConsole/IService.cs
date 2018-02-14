using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;

namespace ScalesHubConsole
{

    //[DataContract(Namespace = "http://schemas.nostrum.ru/scales")]
    [DataContract]
    public class ScalesMeasureData
    {
        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public DateTime TimestampUTC { get; set; }

        [DataMember]
        public String Code { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String State { get; set; }

        [DataMember]
        public String ScalesState { get; set; }

        [DataMember]
        public DateTime? MeasureTime { get; set; }

        [DataMember]
        public String MeasureTimeStr { get; set; }

        [DataMember]
        public Decimal MeasureValue { get; set; }

        [DataMember]
        public DateTime? LastStableTime { get; set; }

        [DataMember]
        public String LastStableTimeStr { get; set; }

        [DataMember]
        public Decimal LastStableValue { get; set; }
    }

    //[ServiceContract(Namespace = "http://schemas.nostrum.ru/scales")]
    [ServiceContract]
    public interface IScalesHubService
    {
        [OperationContract]
        //[FaultContract(typeof(ServiceException))]
        IEnumerable<ScalesMeasureData> EnumScales();

        [OperationContract]
        //[FaultContract(typeof(ServiceException))]
        ScalesMeasureData GetScalesMeasureData(String code);
    }

}
