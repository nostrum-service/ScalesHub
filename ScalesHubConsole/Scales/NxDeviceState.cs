using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nostrum.ScalesComponent
{
    [Flags]
    public enum NxDeviceState
    {
        DS_NONE = 0x00,
        DS_STEADY = 0x01,
        DS_MAX = 0x02,
        DS_ZERO = 0x04,
        DS_CALIBRATION = 0x10,
        DS_FULL = NxDeviceState.DS_MAX | NxDeviceState.DS_CALIBRATION | NxDeviceState.DS_STEADY | NxDeviceState.DS_ZERO,
        DS_ERROR = 0x20
    }
}
