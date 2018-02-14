using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Common;
using NLog.Targets;

namespace ScalesHubConsole
{

    [Target("NxLog")]
    public sealed class NxLogTarget : TargetWithLayout
    {
        public NxLogTarget()
        {
        }

        private LinkedList<LogEventInfo> queuedEvents = new LinkedList<LogEventInfo>();

        private Object locker = new object();
        private Action<DateTime, String> hLoggerAction;
        public Action<DateTime, String> LoggerAction
        {
            get
            {
                return hLoggerAction;
            }

            set
            {
                if (value != hLoggerAction)
                {
                    hLoggerAction = value;

                    if (hLoggerAction != null && queuedEvents.Count != 0)
                    {
                        try
                        {
                            foreach (LogEventInfo evt in queuedEvents)
                            {
                                hLoggerAction(evt.TimeStamp, evt.Message);
                            }

                            queuedEvents.Clear();
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        protected override void Write(AsyncLogEventInfo[] logEvents)
        {
            foreach (var logEvent in logEvents)
            {
                Write(logEvent);
            }
        }

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            //string logMessage = this.Layout.Render(logEvent);

            if (hLoggerAction != null)
            {
                try
                {
                    foreach (LogEventInfo evt in queuedEvents)
                    {
                        hLoggerAction(evt.TimeStamp, evt.Message);
                    }

                    queuedEvents.Clear();

                    hLoggerAction(logEvent.LogEvent.TimeStamp, logEvent.LogEvent.Message);
                }
                catch
                {

                }
            }
            else
            {
                queuedEvents.AddLast(logEvent.LogEvent);
            }
        }
    } 

}
