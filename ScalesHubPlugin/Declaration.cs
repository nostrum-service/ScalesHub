using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace ScalesHubPlugin
{
    /// <summary>
    /// Для экспорта декодера
    /// </summary>
    public interface IDataFrame
    {
        bool Decode(byte[] data);
        NxDeviceState State { get; }
        double Weight { get; }
    }
    /// <summary>
    /// Метаданные декодера
    /// </summary>
    public interface IDataFrameMetadata
    {
        string Name { get; }
        string Description { get; }
    }    
}