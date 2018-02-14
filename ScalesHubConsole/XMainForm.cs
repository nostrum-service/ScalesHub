using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking2010.Views.Widget;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using LiteDB;
using Newtonsoft.Json;
using NLog;
using Nostrum.ScalesComponent;
using Nostrum.ScalesComponent.Serial;

namespace ScalesHubConsole
{
    public partial class XMainForm : DevExpress.XtraEditors.XtraForm
    {
        protected HostPlatform platform = new HostPlatform();

        protected System.Threading.Timer timer = null;

        public XMainForm()
        {
            InitializeComponent();

            timer = new System.Threading.Timer((unused) => { 
                ControlExtensions.Invoke(this, () => { 
                    barTime.Caption = DateTime.Now.ToString("HH:mm:ss"); }); 
            }, null, System.Threading.Timeout.Infinite, 500);

            barManager.ItemClick += barManager_ItemClick;

            Program.dbPath = String.Empty;
            try
            {
                Program.dbPath = ConfigurationManager.AppSettings["Database"];
                if (Program.dbPath.StartsWith("{CommonApplicationData}"))
                {
                    String Company = (from tmp in Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false) select (tmp as AssemblyCompanyAttribute).Company).FirstOrDefault();
                    String Product = (from tmp in Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false) select (tmp as AssemblyProductAttribute).Product).FirstOrDefault();

                    Program.dbPath = Program.dbPath.Replace("{CommonApplicationData}", String.Format(@"{0}\{1}\{2}", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), Company, Product));
                }
            }
            catch
            {
                Program.dbPath = String.Empty;
            }

            reg_grid.DataSource = Program.MeasuringChannels;
            mainView.FocusedRowObjectChanged += (sender, e) => SetupChanellCmd();

        }

        void SetupChanellCmd()
        {
            ScalesChannel channel = mainView.GetFocusedRow() as ScalesChannel;
            if (channel == null)
            {
                btnRemove.Enabled = false;
                btnEdit.Enabled = false;
                btnActivate.Enabled = false;
                btnDeactivate.Enabled = false;
            }
            else
            {
                btnRemove.Enabled = true;
                btnEdit.Enabled = !channel.Active;
                btnActivate.Enabled = !channel.Active;
                btnDeactivate.Enabled = channel.Active;
            }
        }

        void EditChannel()
        {
            ScalesChannel channel = mainView.GetFocusedRow() as ScalesChannel;
            if (channel != null)
            {
                XChannelEdit dlg = new XChannelEdit(channel.Id);
                if (dlg.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    Program.MeasuringChannels.Remove(channel);
                    using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
                    {
                        DbScalesMeasureData lastMeaure = dba.GetCollection<DbScalesMeasureData>("measure").Find(x => x.ScalesId == dlg.ViewModel.Id).OrderByDescending(x => x.Time).FirstOrDefault();
                        platform.AddChannel(dba.GetCollection<ScalesDescriptor>("scales").FindById(dlg.ViewModel.Id), lastMeaure);
                    }
                }
            }
        }

        void DeactivateChannel()
        {
            ScalesChannel channel = mainView.GetFocusedRow() as ScalesChannel;
            if (channel != null)
            {
                if (XtraMessageBox.Show(string.Format("Деактивировать канал [{0}]?", channel.Name), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    platform.DeactivateChannel(channel);

                    SetupChanellCmd();
                }
            }
        }

        void ActivateChannel()
        {
            ScalesChannel channel = mainView.GetFocusedRow() as ScalesChannel;
            if (channel != null)
            {
                if (XtraMessageBox.Show(string.Format("Активизировать канал [{0}]?", channel.Name), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    platform.ActivateChannel(channel);

                    SetupChanellCmd();
                }
            }
        }

        void RemoveChannel()
        {
            ScalesChannel channel = mainView.GetFocusedRow() as ScalesChannel;
            if (channel != null)
            {
                if (XtraMessageBox.Show(string.Format("Удалить канал [{0}]?", channel.Name), this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    platform.RemoveChannel(channel);
                }
            }
        }

        protected void LoggerAction(DateTime timestamp, String message)
        {
            try
            {
                System.Diagnostics.Trace.WriteLine(string.Format("logged {0}, {1:HH:mm:ss}", message, timestamp));

                reg_log.BeginInvoke((Action) delegate() {
                    if (reg_log.Nodes.Count > 400)
                    {
                        reg_log.BeginUpdate();
                        var toremove = (from TreeListNode tmp in reg_log.Nodes orderby (DateTime)tmp.GetValue("Timestamp") ascending select tmp).Take(Math.Min(200, reg_log.Nodes.Count)).ToArray();
                        foreach (TreeListNode tmp in toremove)
                            reg_log.Nodes.Remove(tmp);
                        reg_log.EndUpdate();
                    }

                    bool setFocus = reg_log.FocusedNode != null ? reg_log.FocusedNode.NextVisibleNode == null : true;
                    TreeListNode node = reg_log.AppendNode(new object[] { timestamp, message }, null);

                    //TreeListNode last = (from TreeListNode tmp in reg_log.Nodes orderby (DateTime)tmp.GetValue("Timestamp") descending select tmp).FirstOrDefault();
                    //reg_log.SetFocusedNode(last);

                    if (setFocus)
                    {
                        reg_log.FocusedNode = node;
                    }
                });
            }
            catch
            {

            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var logger = LogManager.Configuration.AllTargets.FirstOrDefault(tmp => tmp is NxLogTarget) as NxLogTarget;
            if (logger != null)
            {
                logger.LoggerAction = LoggerAction;
            }

            timer.Change(0, 500);

            platform.LoadMetadata();
            SetupChanellCmd();

            platform.Startup();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (XtraMessageBox.Show("Закрыть приложение?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            {
                e.Cancel = true;
                return;
            }

            if (timer != null)
                timer.Change(System.Threading.Timeout.Infinite, 500);

            var logger = LogManager.Configuration.AllTargets.FirstOrDefault(tmp => tmp is NxLogTarget) as NxLogTarget;
            if (logger != null)
            {
                logger.LoggerAction = null;
            }

            base.OnClosing(e);

            platform.Shutdown();
        }

        void barManager_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "btnAbout":
                    XtraMessageBox.Show(string.Format("Nostrum scales hub V{0}", GetType().Assembly.GetName().Version), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case "btnClearLog":
                    reg_log.Nodes.Clear();
                    break;

                case "btnEdit":
                    EditChannel();
                    break;

                case "btnActivate":
                    ActivateChannel();
                    break;

                case "btnDeactivate":
                    DeactivateChannel();
                    break;

                case "btnRemove":
                    RemoveChannel();
                    break;

                case "btnRegister":
                    XChannelRegister dlgRegister = new XChannelRegister();
                    if (dlgRegister.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        using (LiteDatabase dba = new LiteDatabase(string.Format("filename={0};", Program.dbPath)))
                        {
                            platform.AddChannel(dba.GetCollection<ScalesDescriptor>("scales").FindById(dlgRegister.ViewModel.Id));
                        }
                    }

                    break;

                case "btnExit":
                    Close();
                    break;
            }
        }

    }


}
