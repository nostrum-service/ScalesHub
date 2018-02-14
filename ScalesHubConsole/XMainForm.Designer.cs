namespace ScalesHubConsole
{
    partial class XMainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule2 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression1 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule3 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression2 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule4 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression3 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            DevExpress.XtraGrid.GridFormatRule gridFormatRule5 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleExpression formatConditionRuleExpression4 = new DevExpress.XtraEditors.FormatConditionRuleExpression();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XMainForm));
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.btnRegister = new DevExpress.XtraBars.BarButtonItem();
            this.btnEdit = new DevExpress.XtraBars.BarButtonItem();
            this.btnActivate = new DevExpress.XtraBars.BarButtonItem();
            this.btnDeactivate = new DevExpress.XtraBars.BarButtonItem();
            this.btnRemove = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.btnClearLog = new DevExpress.XtraBars.BarButtonItem();
            this.btnExit = new DevExpress.XtraBars.BarButtonItem();
            this.skinBarSubItem1 = new DevExpress.XtraBars.SkinBarSubItem();
            this.btnAbout = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barTime = new DevExpress.XtraBars.BarStaticItem();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.imageCollection = new DevExpress.Utils.ImageCollection(this.components);
            this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.reg_grid = new DevExpress.XtraGrid.GridControl();
            this.mainView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAcceptedValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAcceptedTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMeasureValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMeasureTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActive = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDeviceState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.noDocumentsView1 = new DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView(this.components);
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.reg_log = new DevExpress.XtraTreeList.TreeList();
            this.colTimestamp = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colMessage = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reg_grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.noDocumentsView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reg_log)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // colStatus
            // 
            this.colStatus.Caption = "Состояние";
            this.colStatus.FieldName = "State";
            this.colStatus.Name = "colStatus";
            this.colStatus.OptionsColumn.FixedWidth = true;
            this.colStatus.Width = 146;
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2,
            this.bar3});
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.Form = this;
            this.barManager.HideBarsWhenMerging = false;
            this.barManager.Images = this.imageCollection;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.btnExit,
            this.skinBarSubItem1,
            this.btnClearLog,
            this.btnActivate,
            this.btnDeactivate,
            this.btnRemove,
            this.btnEdit,
            this.btnRegister,
            this.barTime,
            this.btnAbout});
            this.barManager.MainMenu = this.bar2;
            this.barManager.MaxItemId = 14;
            this.barManager.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnRegister, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnEdit, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnActivate, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDeactivate, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnRemove, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.Text = "Tools";
            // 
            // btnRegister
            // 
            this.btnRegister.Caption = "Регистрация канала";
            this.btnRegister.Id = 11;
            this.btnRegister.ImageIndex = 2;
            this.btnRegister.Name = "btnRegister";
            // 
            // btnEdit
            // 
            this.btnEdit.Caption = "Редактировать";
            this.btnEdit.Id = 10;
            this.btnEdit.ImageIndex = 1;
            this.btnEdit.Name = "btnEdit";
            // 
            // btnActivate
            // 
            this.btnActivate.Caption = "Активировать";
            this.btnActivate.Id = 7;
            this.btnActivate.ImageIndex = 4;
            this.btnActivate.Name = "btnActivate";
            // 
            // btnDeactivate
            // 
            this.btnDeactivate.Caption = "Деактивировать";
            this.btnDeactivate.Id = 8;
            this.btnDeactivate.ImageIndex = 3;
            this.btnDeactivate.Name = "btnDeactivate";
            // 
            // btnRemove
            // 
            this.btnRemove.Caption = "Удалить";
            this.btnRemove.Id = 9;
            this.btnRemove.ImageIndex = 0;
            this.btnRemove.Name = "btnRemove";
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.skinBarSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAbout)});
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "Файл";
            this.barSubItem1.Id = 1;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnClearLog),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExit, true)});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // btnClearLog
            // 
            this.btnClearLog.Caption = "Очистить протокол";
            this.btnClearLog.Id = 5;
            this.btnClearLog.Name = "btnClearLog";
            // 
            // btnExit
            // 
            this.btnExit.Caption = "Выход";
            this.btnExit.Id = 2;
            this.btnExit.Name = "btnExit";
            // 
            // skinBarSubItem1
            // 
            this.skinBarSubItem1.Caption = "Тема";
            this.skinBarSubItem1.Id = 4;
            this.skinBarSubItem1.Name = "skinBarSubItem1";
            // 
            // btnAbout
            // 
            this.btnAbout.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.btnAbout.Caption = "О программе";
            this.btnAbout.Id = 13;
            this.btnAbout.Name = "btnAbout";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barTime)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barTime
            // 
            this.barTime.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barTime.Caption = "Время";
            this.barTime.Id = 12;
            this.barTime.ImageIndex = 7;
            this.barTime.Name = "barTime";
            this.barTime.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barTime.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(894, 51);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 459);
            this.barDockControlBottom.Size = new System.Drawing.Size(894, 27);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 51);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 408);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(894, 51);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 408);
            // 
            // imageCollection
            // 
            this.imageCollection.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection.ImageStream")));
            this.imageCollection.Images.SetKeyName(0, "Action_Delete.png");
            this.imageCollection.Images.SetKeyName(1, "Action_Edit.png");
            this.imageCollection.Images.SetKeyName(2, "Action_LinkUnlink_Link.png");
            this.imageCollection.Images.SetKeyName(3, "BO_Rules.png");
            this.imageCollection.Images.SetKeyName(4, "ModelEditor_Actions.png");
            this.imageCollection.Images.SetKeyName(5, "State_Validation_Valid.png");
            this.imageCollection.InsertGalleryImage("time_16x16.png", "office2013/scheduling/time_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("office2013/scheduling/time_16x16.png"), 6);
            this.imageCollection.Images.SetKeyName(6, "time_16x16.png");
            this.imageCollection.Images.SetKeyName(7, "BO_Scheduler.png");
            this.imageCollection.Images.SetKeyName(8, "offline.png");
            this.imageCollection.Images.SetKeyName(9, "online.png");
            // 
            // documentManager
            // 
            this.documentManager.ClientControl = this.reg_grid;
            this.documentManager.MenuManager = this.barManager;
            this.documentManager.RibbonAndBarsMergeStyle = DevExpress.XtraBars.Docking2010.Views.RibbonAndBarsMergeStyle.Always;
            this.documentManager.ShowThumbnailsInTaskBar = DevExpress.Utils.DefaultBoolean.False;
            this.documentManager.View = this.noDocumentsView1;
            this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.noDocumentsView1});
            // 
            // reg_grid
            // 
            this.reg_grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reg_grid.Location = new System.Drawing.Point(0, 51);
            this.reg_grid.MainView = this.mainView;
            this.reg_grid.Name = "reg_grid";
            this.reg_grid.ShowOnlyPredefinedDetails = true;
            this.reg_grid.Size = new System.Drawing.Size(894, 257);
            this.reg_grid.TabIndex = 6;
            this.reg_grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.mainView});
            // 
            // mainView
            // 
            this.mainView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colName,
            this.colCode,
            this.colAcceptedValue,
            this.colAcceptedTime,
            this.colMeasureValue,
            this.colMeasureTime,
            this.colStatus,
            this.colActive,
            this.colId,
            this.colDeviceState});
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Inactive";
            formatConditionRuleValue1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            formatConditionRuleValue1.Appearance.ForeColor = System.Drawing.Color.Gray;
            formatConditionRuleValue1.Appearance.Options.UseBackColor = true;
            formatConditionRuleValue1.Appearance.Options.UseForeColor = true;
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.Expression;
            formatConditionRuleValue1.Expression = "[Active] = False";
            gridFormatRule1.Rule = formatConditionRuleValue1;
            gridFormatRule1.StopIfTrue = true;
            gridFormatRule2.ApplyToRow = true;
            gridFormatRule2.Name = "Offline";
            formatConditionRuleExpression1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            formatConditionRuleExpression1.Appearance.ForeColor = System.Drawing.Color.Maroon;
            formatConditionRuleExpression1.Appearance.Options.UseBackColor = true;
            formatConditionRuleExpression1.Appearance.Options.UseForeColor = true;
            formatConditionRuleExpression1.Expression = "[Active] = True And [State] = \'CH_ERROR\'";
            gridFormatRule2.Rule = formatConditionRuleExpression1;
            gridFormatRule3.ApplyToRow = true;
            gridFormatRule3.Name = "Accepted";
            formatConditionRuleExpression2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            formatConditionRuleExpression2.Appearance.Options.UseBackColor = true;
            formatConditionRuleExpression2.Expression = "[State] = \'CH_WEIGHT_ACCEPTED\'";
            gridFormatRule3.Rule = formatConditionRuleExpression2;
            gridFormatRule4.ApplyToRow = true;
            gridFormatRule4.Name = "WaitZero";
            formatConditionRuleExpression3.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            formatConditionRuleExpression3.Appearance.Options.UseBackColor = true;
            formatConditionRuleExpression3.Expression = "[State] = \'CH_WAITING_ZERO\'";
            gridFormatRule4.Rule = formatConditionRuleExpression3;
            gridFormatRule5.ApplyToRow = true;
            gridFormatRule5.Name = "Exposuring";
            formatConditionRuleExpression4.Appearance.BackColor = System.Drawing.Color.SteelBlue;
            formatConditionRuleExpression4.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            formatConditionRuleExpression4.Appearance.ForeColor = System.Drawing.Color.White;
            formatConditionRuleExpression4.Appearance.Options.UseBackColor = true;
            formatConditionRuleExpression4.Appearance.Options.UseFont = true;
            formatConditionRuleExpression4.Appearance.Options.UseForeColor = true;
            formatConditionRuleExpression4.Expression = "[State] = \'CH_EXPOSURING\'";
            gridFormatRule5.Rule = formatConditionRuleExpression4;
            this.mainView.FormatRules.Add(gridFormatRule1);
            this.mainView.FormatRules.Add(gridFormatRule2);
            this.mainView.FormatRules.Add(gridFormatRule3);
            this.mainView.FormatRules.Add(gridFormatRule4);
            this.mainView.FormatRules.Add(gridFormatRule5);
            this.mainView.GridControl = this.reg_grid;
            this.mainView.Name = "mainView";
            this.mainView.OptionsBehavior.AutoPopulateColumns = false;
            this.mainView.OptionsBehavior.Editable = false;
            this.mainView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.mainView.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.mainView.OptionsView.ShowGroupPanel = false;
            this.mainView.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colName, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colName
            // 
            this.colName.Caption = "Наименование";
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.Visible = true;
            this.colName.VisibleIndex = 1;
            this.colName.Width = 98;
            // 
            // colCode
            // 
            this.colCode.Caption = "Код";
            this.colCode.FieldName = "Code";
            this.colCode.Name = "colCode";
            this.colCode.Visible = true;
            this.colCode.VisibleIndex = 0;
            this.colCode.Width = 58;
            // 
            // colAcceptedValue
            // 
            this.colAcceptedValue.Caption = "Замер";
            this.colAcceptedValue.FieldName = "AcceptedValue";
            this.colAcceptedValue.Name = "colAcceptedValue";
            this.colAcceptedValue.OptionsColumn.FixedWidth = true;
            this.colAcceptedValue.Width = 84;
            // 
            // colAcceptedTime
            // 
            this.colAcceptedTime.Caption = "Время замера";
            this.colAcceptedTime.DisplayFormat.FormatString = "dd.MM.yy HH:mm:ss";
            this.colAcceptedTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colAcceptedTime.FieldName = "AcceptedTime";
            this.colAcceptedTime.Name = "colAcceptedTime";
            this.colAcceptedTime.OptionsColumn.FixedWidth = true;
            this.colAcceptedTime.Width = 119;
            // 
            // colMeasureValue
            // 
            this.colMeasureValue.Caption = "Значение";
            this.colMeasureValue.FieldName = "MeasureValue";
            this.colMeasureValue.Name = "colMeasureValue";
            this.colMeasureValue.OptionsColumn.FixedWidth = true;
            this.colMeasureValue.Visible = true;
            this.colMeasureValue.VisibleIndex = 2;
            this.colMeasureValue.Width = 88;
            // 
            // colMeasureTime
            // 
            this.colMeasureTime.Caption = "Время";
            this.colMeasureTime.DisplayFormat.FormatString = "HH:mm:ss";
            this.colMeasureTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colMeasureTime.FieldName = "MeasureTime";
            this.colMeasureTime.Name = "colMeasureTime";
            this.colMeasureTime.OptionsColumn.FixedWidth = true;
            this.colMeasureTime.Visible = true;
            this.colMeasureTime.VisibleIndex = 3;
            this.colMeasureTime.Width = 78;
            // 
            // colActive
            // 
            this.colActive.Caption = "Активный канал";
            this.colActive.FieldName = "Active";
            this.colActive.Name = "colActive";
            this.colActive.OptionsColumn.FixedWidth = true;
            this.colActive.Visible = true;
            this.colActive.VisibleIndex = 4;
            this.colActive.Width = 97;
            // 
            // colId
            // 
            this.colId.Caption = "Идентификатор";
            this.colId.FieldName = "Id";
            this.colId.Name = "colId";
            // 
            // colDeviceState
            // 
            this.colDeviceState.Caption = "Состояние весов";
            this.colDeviceState.FieldName = "DeviceStateString";
            this.colDeviceState.Name = "colDeviceState";
            this.colDeviceState.Visible = true;
            this.colDeviceState.VisibleIndex = 5;
            // 
            // dockManager
            // 
            this.dockManager.Form = this;
            this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.StatusBar",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.dockPanel1.ID = new System.Guid("e3dc1ced-46ac-4000-8d0e-ce8447ede55f");
            this.dockPanel1.Location = new System.Drawing.Point(0, 308);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 151);
            this.dockPanel1.Size = new System.Drawing.Size(894, 151);
            this.dockPanel1.Text = "Протокол";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.reg_log);
            this.dockPanel1_Container.Location = new System.Drawing.Point(4, 23);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(886, 124);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // reg_log
            // 
            this.reg_log.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colTimestamp,
            this.colMessage});
            this.reg_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reg_log.Location = new System.Drawing.Point(0, 0);
            this.reg_log.Name = "reg_log";
            this.reg_log.OptionsBehavior.Editable = false;
            this.reg_log.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.reg_log.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.RowFullFocus;
            this.reg_log.OptionsView.ShowButtons = false;
            this.reg_log.OptionsView.ShowHorzLines = false;
            this.reg_log.OptionsView.ShowIndicator = false;
            this.reg_log.OptionsView.ShowRoot = false;
            this.reg_log.OptionsView.ShowVertLines = false;
            this.reg_log.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.reg_log.Size = new System.Drawing.Size(886, 124);
            this.reg_log.TabIndex = 0;
            // 
            // colTimestamp
            // 
            this.colTimestamp.Caption = "Время";
            this.colTimestamp.FieldName = "Timestamp";
            this.colTimestamp.Format.FormatString = "dd.MM.yy HH:mm:ss.ff";
            this.colTimestamp.Format.FormatType = DevExpress.Utils.FormatType.Custom;
            this.colTimestamp.Name = "colTimestamp";
            this.colTimestamp.OptionsColumn.AllowSort = false;
            this.colTimestamp.OptionsColumn.FixedWidth = true;
            this.colTimestamp.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.colTimestamp.Visible = true;
            this.colTimestamp.VisibleIndex = 0;
            this.colTimestamp.Width = 132;
            // 
            // colMessage
            // 
            this.colMessage.Caption = "Сообщение";
            this.colMessage.ColumnEdit = this.repositoryItemMemoEdit1;
            this.colMessage.FieldName = "Message";
            this.colMessage.Name = "colMessage";
            this.colMessage.OptionsColumn.AllowSort = false;
            this.colMessage.Visible = true;
            this.colMessage.VisibleIndex = 1;
            this.colMessage.Width = 752;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // XMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 486);
            this.Controls.Add(this.reg_grid);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "XMainForm";
            this.Text = "Весовой монитор";
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reg_grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.noDocumentsView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reg_log)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem btnExit;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraGrid.GridControl reg_grid;
        private DevExpress.XtraGrid.Views.Grid.GridView mainView;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colCode;
        private DevExpress.XtraGrid.Columns.GridColumn colAcceptedValue;
        private DevExpress.XtraGrid.Columns.GridColumn colAcceptedTime;
        private DevExpress.XtraGrid.Columns.GridColumn colMeasureValue;
        private DevExpress.XtraGrid.Columns.GridColumn colMeasureTime;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colActive;
        private DevExpress.XtraGrid.Columns.GridColumn colId;
        private DevExpress.XtraBars.Docking2010.Views.NoDocuments.NoDocumentsView noDocumentsView1;
        private DevExpress.XtraBars.SkinBarSubItem skinBarSubItem1;
        private DevExpress.XtraTreeList.TreeList reg_log;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colTimestamp;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colMessage;
        private DevExpress.XtraBars.BarButtonItem btnClearLog;
        private DevExpress.XtraBars.BarButtonItem btnActivate;
        private DevExpress.XtraBars.BarButtonItem btnDeactivate;
        private DevExpress.XtraBars.BarButtonItem btnRemove;
        private DevExpress.XtraBars.BarButtonItem btnEdit;
        private DevExpress.XtraBars.BarButtonItem btnRegister;
        private DevExpress.Utils.ImageCollection imageCollection;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraBars.BarStaticItem barTime;
        private DevExpress.XtraBars.BarButtonItem btnAbout;
        private DevExpress.XtraGrid.Columns.GridColumn colDeviceState;
        internal DevExpress.XtraBars.Docking.DockManager dockManager;

    }
}

