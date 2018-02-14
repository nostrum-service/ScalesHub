using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nostrum.ScalesComponent;
using System.Runtime.CompilerServices;

namespace ScalesHubConsole
{
    public partial class ManualSourceControl : UserControl
    {
        public ScalesData source = new ScalesData();
        System.Threading.Timer timer = null;
        bool started = false;

        public void Stop()
        {
            timer.Change(System.Threading.Timeout.Infinite, 300);
        }

        public ManualSourceControl(string msid)
        {
            InitializeComponent();

            this.HandleDestroyed += (sender, e) =>
            {
                started = false;
                timer.Change(System.Threading.Timeout.Infinite, 300);
                timer.Dispose();
                timer = null;
            };

            source.MSID = msid;
            source.Weight = 0;
            source.State = NxDeviceState.DS_NONE;

            reg_state.Properties.Items.AddEnum<NxDeviceState>();

            reg_msid.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "MSID", true, DataSourceUpdateMode.OnPropertyChanged));
            reg_weight.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "Weight", true, DataSourceUpdateMode.OnPropertyChanged));
            reg_state.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "State", true, DataSourceUpdateMode.OnPropertyChanged));

            timer = new System.Threading.Timer((unused) => PostMessage(), null, System.Threading.Timeout.Infinite, 300);

            btnStartStop.Click += (sender, e) =>
            {
                if (started)
                {
                    btnStartStop.Text = "Старт";
                    started = false;
                    timer.Change(System.Threading.Timeout.Infinite, 300);
                }
                else
                {
                    btnStartStop.Text = "Стоп";
                    started = true;
                    timer.Change(0, 300);
                }
            };
        }

        DateTime last = DateTime.MinValue;
        protected void PostMessage()
        {
            DateTime now = DateTime.Now;
            if (last == DateTime.MinValue)
            {
                last = now;
            }
            else
            {
                //System.Diagnostics.Trace.TraceInformation("Manual control Post interval " + (now - last).TotalMilliseconds);
                last = now;
            }

            Program.PostMessage(source.MSID, string.Format("weight={0};state={1}", source.Weight, source.State));
            
            /*if ((DateTime.Now - now).TotalMilliseconds > 500)
                System.Diagnostics.Trace.TraceInformation("Post time spand " + (DateTime.Now - now).TotalMilliseconds);*/
        }

        public class ScalesData : INotifyPropertyChanged
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

            private String hMSID;
            private Decimal hWeight;
            private NxDeviceState hState;

            public String MSID
            {
                get { return hMSID; }
                set { SetField(ref hMSID, value); }
            }

            public Decimal Weight
            {
                get { return hWeight; }
                set { SetField(ref hWeight, value); }
            }

            public NxDeviceState State
            {
                get { return hState; }
                set { SetField(ref hState, value); }
            }
        }
    }
}
