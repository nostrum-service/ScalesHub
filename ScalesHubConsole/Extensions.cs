using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScalesHubConsole
{

    internal static class ControlExtensions
    {
        public static void Invoke(this Control Control, Action Action)
        {
            if (Control != null ? !Control.IsDisposed : false)
            {
                if (Control.InvokeRequired)
                {
                    Control.Invoke(Action);
                }
                else
                {
                    Action();
                }
            }
        }

        public static void SafeInvoke(this Control Control, Action Action, bool forceSynchronous)
        {
            if (Control != null ? !Control.IsDisposed : false)
            {
                if (Control.InvokeRequired)
                {
                    if (forceSynchronous)
                        Control.BeginInvoke(Action);
                    else
                        Control.Invoke(Action);
                }
                else
                {
                    Action();
                }
            }
        }
    }

}
