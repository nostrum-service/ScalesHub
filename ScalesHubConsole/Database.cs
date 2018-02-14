using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace ScalesHubConsole
{

    public class ScalesDescriptor
    {
        public int Id { get; set; }

        [BsonIndex(true)]
        public String Code { get; set; }

        public String Name { get; set; }

        public String Decoder { get; set; }
        public Boolean Active { get; set; }
        public String Description { get; set; }

        public Decimal TriggerLoading { get; set; }
        public Decimal TriggerEmpty { get; set; }
        public double Exposure { get; set; }
        public double Timeout { get; set; }

        public String StreamType { get; set; }
        public String Settings { get; set; }
    }


    public class DbScalesMeasureData
    {
        [BsonIndex(false)]
        public int ScalesId { get; set; }

        [BsonIndex(false)]
        public DateTime Time { get; set; }

        public String State { get; set; }

        public Decimal Value { get; set; }

    }

}
