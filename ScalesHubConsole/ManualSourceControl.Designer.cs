namespace ScalesHubConsole
{
    partial class ManualSourceControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.reg_layout = new DevExpress.XtraLayout.LayoutControl();
            this.reg_msid = new DevExpress.XtraEditors.TextEdit();
            this.reg_state = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.btnStartStop = new DevExpress.XtraEditors.SimpleButton();
            this.reg_weight = new DevExpress.XtraEditors.SpinEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lcValue = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcState = new DevExpress.XtraLayout.LayoutControlItem();
            this.lcName = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.reg_layout)).BeginInit();
            this.reg_layout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reg_msid.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reg_state.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reg_weight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // reg_layout
            // 
            this.reg_layout.Controls.Add(this.reg_msid);
            this.reg_layout.Controls.Add(this.reg_state);
            this.reg_layout.Controls.Add(this.btnStartStop);
            this.reg_layout.Controls.Add(this.reg_weight);
            this.reg_layout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reg_layout.Location = new System.Drawing.Point(0, 0);
            this.reg_layout.Name = "reg_layout";
            this.reg_layout.OptionsView.UseDefaultDragAndDropRendering = false;
            this.reg_layout.Root = this.layoutControlGroup1;
            this.reg_layout.Size = new System.Drawing.Size(342, 201);
            this.reg_layout.TabIndex = 0;
            this.reg_layout.Text = "layoutControl1";
            // 
            // reg_msid
            // 
            this.reg_msid.Location = new System.Drawing.Point(73, 12);
            this.reg_msid.Name = "reg_msid";
            this.reg_msid.Properties.ReadOnly = true;
            this.reg_msid.Size = new System.Drawing.Size(257, 20);
            this.reg_msid.StyleController = this.reg_layout;
            this.reg_msid.TabIndex = 7;
            // 
            // reg_state
            // 
            this.reg_state.Location = new System.Drawing.Point(73, 60);
            this.reg_state.Name = "reg_state";
            this.reg_state.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.reg_state.Size = new System.Drawing.Size(257, 20);
            this.reg_state.StyleController = this.reg_layout;
            this.reg_state.TabIndex = 6;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(234, 167);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(96, 22);
            this.btnStartStop.StyleController = this.reg_layout;
            this.btnStartStop.TabIndex = 5;
            this.btnStartStop.Text = "Старт";
            // 
            // reg_weight
            // 
            this.reg_weight.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.reg_weight.Location = new System.Drawing.Point(73, 36);
            this.reg_weight.Name = "reg_weight";
            this.reg_weight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.reg_weight.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.reg_weight.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.None;
            this.reg_weight.Size = new System.Drawing.Size(257, 20);
            this.reg_weight.StyleController = this.reg_layout;
            this.reg_weight.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lcValue,
            this.layoutControlItem2,
            this.lcState,
            this.lcName,
            this.emptySpaceItem1,
            this.emptySpaceItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(342, 201);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // lcValue
            // 
            this.lcValue.Control = this.reg_weight;
            this.lcValue.Location = new System.Drawing.Point(0, 24);
            this.lcValue.Name = "lcValue";
            this.lcValue.Size = new System.Drawing.Size(322, 24);
            this.lcValue.Text = "Значение:";
            this.lcValue.TextSize = new System.Drawing.Size(58, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnStartStop;
            this.layoutControlItem2.Location = new System.Drawing.Point(222, 155);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(100, 26);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // lcState
            // 
            this.lcState.Control = this.reg_state;
            this.lcState.Location = new System.Drawing.Point(0, 48);
            this.lcState.Name = "lcState";
            this.lcState.Size = new System.Drawing.Size(322, 24);
            this.lcState.Text = "Состояние:";
            this.lcState.TextSize = new System.Drawing.Size(58, 13);
            // 
            // lcName
            // 
            this.lcName.Control = this.reg_msid;
            this.lcName.Location = new System.Drawing.Point(0, 0);
            this.lcName.Name = "lcName";
            this.lcName.Size = new System.Drawing.Size(322, 24);
            this.lcName.Text = "Источник:";
            this.lcName.TextSize = new System.Drawing.Size(58, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 155);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(222, 26);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(0, 72);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(322, 83);
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // ManualSourceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.reg_layout);
            this.Name = "ManualSourceControl";
            this.Size = new System.Drawing.Size(342, 201);
            ((System.ComponentModel.ISupportInitialize)(this.reg_layout)).EndInit();
            this.reg_layout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reg_msid.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reg_state.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reg_weight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lcName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl reg_layout;
        private DevExpress.XtraEditors.TextEdit reg_msid;
        private DevExpress.XtraEditors.ImageComboBoxEdit reg_state;
        private DevExpress.XtraEditors.SimpleButton btnStartStop;
        private DevExpress.XtraEditors.SpinEdit reg_weight;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem lcValue;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem lcState;
        private DevExpress.XtraLayout.LayoutControlItem lcName;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;



    }
}
