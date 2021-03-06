﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;

namespace ScalesHubConsole
{
    public partial class XChannelEdit : DevExpress.XtraEditors.XtraForm
    {
        protected BindingList<ScalesChannelViewModel> source = new BindingList<ScalesChannelViewModel>();

        public XChannelEdit(int id)
        {
            InitializeComponent();

            Setup(id);
        }

        public ScalesChannelViewModel ViewModel
        {
            get
            {
                return source[0];
            }
        }

        protected void SetupSettings(String settingsType)
        {
            if (settingsType == "tcp")
            {
                reg_layout.BeginUpdate();
                lcPortName.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcBaudRate.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcDataBits.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcParity.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                lcStopBits.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                lcAddress.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                reg_layout.EndUpdate();
            }

            if (settingsType == "serial")
            {
                reg_layout.BeginUpdate();
                lcPortName.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcBaudRate.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcDataBits.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcParity.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                lcStopBits.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;

                lcAddress.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                reg_layout.EndUpdate();
            }
        }

        protected void Setup(int id)
        {
            var decoders = new List<string>();
            foreach(var module in Program.Plugins.DecoderModules)
            {
                if (decoders.Contains(module.Metadata.Name)) continue;
                decoders.Add(module.Metadata.Name);
            }
            ///заполнение combobox
            reg_data_frame.Properties.Items.AddRange(decoders);//new string[]{ "НВТ-1", "НВТ-3", "НВТ-9" });
            //reg_data_frame.EditValueChanging += (sender, e) => SetupDecoder(Convert.ToString(e.NewValue));

            source.Add(ScalesChannelViewModel.LoadFromDb(id));

            reg_stop_bits.Properties.Items.AddEnum<System.IO.Ports.StopBits>();
            reg_parity.Properties.Items.AddEnum<System.IO.Ports.Parity>();

            reg_code.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "Code", true, DataSourceUpdateMode.OnValidation));
            reg_name.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "Name", true, DataSourceUpdateMode.OnValidation));
            reg_decoder.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "Decoder", true, DataSourceUpdateMode.OnValidation));
            reg_data_frame.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "Decoder", true, DataSourceUpdateMode.OnValidation));

            reg_active.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "Active", true, DataSourceUpdateMode.OnValidation));
            reg_description.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "Description", true, DataSourceUpdateMode.OnValidation));
            reg_trigger_empty.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "TriggerEmpty", true, DataSourceUpdateMode.OnValidation));
            reg_exposure.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "ExposureMilliseconds", true, DataSourceUpdateMode.OnValidation));
            reg_timeout.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "TimeoutMilliseconds", true, DataSourceUpdateMode.OnValidation));

            reg_address.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "Address", true, DataSourceUpdateMode.OnValidation));
            reg_port_name.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "PortName", true, DataSourceUpdateMode.OnValidation));
            reg_baud_rate.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "BaudRate", true, DataSourceUpdateMode.OnValidation));
            reg_data_bits.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "DataBits", true, DataSourceUpdateMode.OnValidation));
            reg_parity.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "Parity", true, DataSourceUpdateMode.OnValidation));
            reg_stop_bits.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "StopBits", true, DataSourceUpdateMode.OnValidation));

            reg_type.EditValueChanging += (sender, e) => SetupSettings(Convert.ToString(e.NewValue));
            reg_type.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", source, "SettingsType", true, DataSourceUpdateMode.OnPropertyChanged));

            SetupSettings(source[0].SettingsType);

            btnOk.Click += (sender, e) =>
            {
                var vr = ViewModel.Validate();
                if (vr.Count() != 0)
                {
                    String errors = String.Concat((from tmp in vr select tmp.Message + "\r\n"));
                    XtraMessageBox.Show(string.Format("Данные содержат ошибки:\r\n{0}", errors), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                ViewModel.Update();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            };
        }
        protected void SetupDecoder(string decoder)
        {

        }
    }
}