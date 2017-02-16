namespace WiiBalanceWalker
{
    partial class FormMain
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
            this.label_rwWT = new System.Windows.Forms.Label();
            this.button_Connect = new System.Windows.Forms.Button();
            this.label_brX = new System.Windows.Forms.Label();
            this.label_brY = new System.Windows.Forms.Label();
            this.label_brDL = new System.Windows.Forms.Label();
            this.label_brDR = new System.Windows.Forms.Label();
            this.groupBox_RawWeight = new System.Windows.Forms.GroupBox();
            this.label_rwBR = new System.Windows.Forms.Label();
            this.label_rwBL = new System.Windows.Forms.Label();
            this.label_rwTR = new System.Windows.Forms.Label();
            this.label_rwTL = new System.Windows.Forms.Label();
            this.groupBox_OffsetWeight = new System.Windows.Forms.GroupBox();
            this.label_owWT = new System.Windows.Forms.Label();
            this.label_owTL = new System.Windows.Forms.Label();
            this.label_owTR = new System.Windows.Forms.Label();
            this.label_owBL = new System.Windows.Forms.Label();
            this.label_owBR = new System.Windows.Forms.Label();
            this.groupBox_OffsetWeightRatio = new System.Windows.Forms.GroupBox();
            this.label_owrTL = new System.Windows.Forms.Label();
            this.label_owrTR = new System.Windows.Forms.Label();
            this.label_owrBL = new System.Windows.Forms.Label();
            this.label_owrBR = new System.Windows.Forms.Label();
            this.groupBox_General = new System.Windows.Forms.GroupBox();
            this.button_SetCenterOffset = new System.Windows.Forms.Button();
            this.button_ResetDefaults = new System.Windows.Forms.Button();
            this.button_BluetoothAddDevice = new System.Windows.Forms.Button();
            this.groupBox_BalanceRatio = new System.Windows.Forms.GroupBox();
            this.label_brDF = new System.Windows.Forms.Label();
            this.groupBox_BalanceRatioTriggers = new System.Windows.Forms.GroupBox();
            this.numericUpDown_TMFB = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_TMLR = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_TFB = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_TLR = new System.Windows.Forms.NumericUpDown();
            this.label_TMFB = new System.Windows.Forms.Label();
            this.label_TMLR = new System.Windows.Forms.Label();
            this.label_TFB = new System.Windows.Forms.Label();
            this.label_TLR = new System.Windows.Forms.Label();
            this.label_Status = new System.Windows.Forms.Label();
            this.groupBox_RawWeight.SuspendLayout();
            this.groupBox_OffsetWeight.SuspendLayout();
            this.groupBox_OffsetWeightRatio.SuspendLayout();
            this.groupBox_General.SuspendLayout();
            this.groupBox_BalanceRatio.SuspendLayout();
            this.groupBox_BalanceRatioTriggers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TMFB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TMLR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TFB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TLR)).BeginInit();
            this.SuspendLayout();
            // 
            // label_rwWT
            // 
            this.label_rwWT.AutoSize = true;
            this.label_rwWT.Location = new System.Drawing.Point(95, 174);
            this.label_rwWT.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_rwWT.Name = "label_rwWT";
            this.label_rwWT.Size = new System.Drawing.Size(33, 20);
            this.label_rwWT.TabIndex = 0;
            this.label_rwWT.Text = "WT";
            this.label_rwWT.Click += new System.EventHandler(this.label_rwWT_Click);
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(261, 117);
            this.button_Connect.Margin = new System.Windows.Forms.Padding(5);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(275, 74);
            this.button_Connect.TabIndex = 0;
            this.button_Connect.Text = "Connect to Wii balance board";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // label_brX
            // 
            this.label_brX.AutoSize = true;
            this.label_brX.Location = new System.Drawing.Point(38, 50);
            this.label_brX.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_brX.Name = "label_brX";
            this.label_brX.Size = new System.Drawing.Size(20, 20);
            this.label_brX.TabIndex = 0;
            this.label_brX.Text = "X";
            this.label_brX.Click += new System.EventHandler(this.label_brX_Click);
            // 
            // label_brY
            // 
            this.label_brY.AutoSize = true;
            this.label_brY.Location = new System.Drawing.Point(152, 50);
            this.label_brY.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_brY.Name = "label_brY";
            this.label_brY.Size = new System.Drawing.Size(20, 20);
            this.label_brY.TabIndex = 0;
            this.label_brY.Text = "Y";
            // 
            // label_brDL
            // 
            this.label_brDL.AutoSize = true;
            this.label_brDL.Location = new System.Drawing.Point(38, 117);
            this.label_brDL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_brDL.Name = "label_brDL";
            this.label_brDL.Size = new System.Drawing.Size(30, 20);
            this.label_brDL.TabIndex = 0;
            this.label_brDL.Text = "DL";
            // 
            // label_brDR
            // 
            this.label_brDR.AutoSize = true;
            this.label_brDR.Location = new System.Drawing.Point(152, 117);
            this.label_brDR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_brDR.Name = "label_brDR";
            this.label_brDR.Size = new System.Drawing.Size(33, 20);
            this.label_brDR.TabIndex = 0;
            this.label_brDR.Text = "DR";
            // 
            // groupBox_RawWeight
            // 
            this.groupBox_RawWeight.Controls.Add(this.label_rwBR);
            this.groupBox_RawWeight.Controls.Add(this.label_rwBL);
            this.groupBox_RawWeight.Controls.Add(this.label_rwTR);
            this.groupBox_RawWeight.Controls.Add(this.label_rwTL);
            this.groupBox_RawWeight.Controls.Add(this.label_rwWT);
            this.groupBox_RawWeight.Location = new System.Drawing.Point(18, 18);
            this.groupBox_RawWeight.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox_RawWeight.Name = "groupBox_RawWeight";
            this.groupBox_RawWeight.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox_RawWeight.Size = new System.Drawing.Size(225, 214);
            this.groupBox_RawWeight.TabIndex = 3;
            this.groupBox_RawWeight.TabStop = false;
            this.groupBox_RawWeight.Text = "Raw Weight";
            // 
            // label_rwBR
            // 
            this.label_rwBR.AutoSize = true;
            this.label_rwBR.Location = new System.Drawing.Point(152, 117);
            this.label_rwBR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_rwBR.Name = "label_rwBR";
            this.label_rwBR.Size = new System.Drawing.Size(32, 20);
            this.label_rwBR.TabIndex = 0;
            this.label_rwBR.Text = "BR";
            // 
            // label_rwBL
            // 
            this.label_rwBL.AutoSize = true;
            this.label_rwBL.Location = new System.Drawing.Point(38, 117);
            this.label_rwBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_rwBL.Name = "label_rwBL";
            this.label_rwBL.Size = new System.Drawing.Size(29, 20);
            this.label_rwBL.TabIndex = 0;
            this.label_rwBL.Text = "BL";
            // 
            // label_rwTR
            // 
            this.label_rwTR.AutoSize = true;
            this.label_rwTR.Location = new System.Drawing.Point(152, 50);
            this.label_rwTR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_rwTR.Name = "label_rwTR";
            this.label_rwTR.Size = new System.Drawing.Size(30, 20);
            this.label_rwTR.TabIndex = 0;
            this.label_rwTR.Text = "TR";
            // 
            // label_rwTL
            // 
            this.label_rwTL.AutoSize = true;
            this.label_rwTL.Location = new System.Drawing.Point(38, 50);
            this.label_rwTL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_rwTL.Name = "label_rwTL";
            this.label_rwTL.Size = new System.Drawing.Size(27, 20);
            this.label_rwTL.TabIndex = 0;
            this.label_rwTL.Text = "TL";
            // 
            // groupBox_OffsetWeight
            // 
            this.groupBox_OffsetWeight.Controls.Add(this.label_owWT);
            this.groupBox_OffsetWeight.Controls.Add(this.label_owTL);
            this.groupBox_OffsetWeight.Controls.Add(this.label_owTR);
            this.groupBox_OffsetWeight.Controls.Add(this.label_owBL);
            this.groupBox_OffsetWeight.Controls.Add(this.label_owBR);
            this.groupBox_OffsetWeight.Location = new System.Drawing.Point(252, 18);
            this.groupBox_OffsetWeight.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox_OffsetWeight.Name = "groupBox_OffsetWeight";
            this.groupBox_OffsetWeight.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox_OffsetWeight.Size = new System.Drawing.Size(225, 214);
            this.groupBox_OffsetWeight.TabIndex = 4;
            this.groupBox_OffsetWeight.TabStop = false;
            this.groupBox_OffsetWeight.Text = "Offset Weight";
            // 
            // label_owWT
            // 
            this.label_owWT.AutoSize = true;
            this.label_owWT.Location = new System.Drawing.Point(95, 174);
            this.label_owWT.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_owWT.Name = "label_owWT";
            this.label_owWT.Size = new System.Drawing.Size(33, 20);
            this.label_owWT.TabIndex = 1;
            this.label_owWT.Text = "WT";
            // 
            // label_owTL
            // 
            this.label_owTL.AutoSize = true;
            this.label_owTL.Location = new System.Drawing.Point(38, 50);
            this.label_owTL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_owTL.Name = "label_owTL";
            this.label_owTL.Size = new System.Drawing.Size(27, 20);
            this.label_owTL.TabIndex = 0;
            this.label_owTL.Text = "TL";
            // 
            // label_owTR
            // 
            this.label_owTR.AutoSize = true;
            this.label_owTR.Location = new System.Drawing.Point(152, 50);
            this.label_owTR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_owTR.Name = "label_owTR";
            this.label_owTR.Size = new System.Drawing.Size(30, 20);
            this.label_owTR.TabIndex = 0;
            this.label_owTR.Text = "TR";
            // 
            // label_owBL
            // 
            this.label_owBL.AutoSize = true;
            this.label_owBL.Location = new System.Drawing.Point(38, 117);
            this.label_owBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_owBL.Name = "label_owBL";
            this.label_owBL.Size = new System.Drawing.Size(29, 20);
            this.label_owBL.TabIndex = 0;
            this.label_owBL.Text = "BL";
            // 
            // label_owBR
            // 
            this.label_owBR.AutoSize = true;
            this.label_owBR.Location = new System.Drawing.Point(152, 117);
            this.label_owBR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_owBR.Name = "label_owBR";
            this.label_owBR.Size = new System.Drawing.Size(32, 20);
            this.label_owBR.TabIndex = 0;
            this.label_owBR.Text = "BR";
            // 
            // groupBox_OffsetWeightRatio
            // 
            this.groupBox_OffsetWeightRatio.Controls.Add(this.label_owrTL);
            this.groupBox_OffsetWeightRatio.Controls.Add(this.label_owrTR);
            this.groupBox_OffsetWeightRatio.Controls.Add(this.label_owrBL);
            this.groupBox_OffsetWeightRatio.Controls.Add(this.label_owrBR);
            this.groupBox_OffsetWeightRatio.Location = new System.Drawing.Point(486, 18);
            this.groupBox_OffsetWeightRatio.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox_OffsetWeightRatio.Name = "groupBox_OffsetWeightRatio";
            this.groupBox_OffsetWeightRatio.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox_OffsetWeightRatio.Size = new System.Drawing.Size(225, 214);
            this.groupBox_OffsetWeightRatio.TabIndex = 4;
            this.groupBox_OffsetWeightRatio.TabStop = false;
            this.groupBox_OffsetWeightRatio.Text = "Offset Weight Ratio";
            this.groupBox_OffsetWeightRatio.Enter += new System.EventHandler(this.groupBox_OffsetWeightRatio_Enter);
            // 
            // label_owrTL
            // 
            this.label_owrTL.AutoSize = true;
            this.label_owrTL.Location = new System.Drawing.Point(38, 50);
            this.label_owrTL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_owrTL.Name = "label_owrTL";
            this.label_owrTL.Size = new System.Drawing.Size(27, 20);
            this.label_owrTL.TabIndex = 0;
            this.label_owrTL.Text = "TL";
            // 
            // label_owrTR
            // 
            this.label_owrTR.AutoSize = true;
            this.label_owrTR.Location = new System.Drawing.Point(152, 50);
            this.label_owrTR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_owrTR.Name = "label_owrTR";
            this.label_owrTR.Size = new System.Drawing.Size(30, 20);
            this.label_owrTR.TabIndex = 0;
            this.label_owrTR.Text = "TR";
            // 
            // label_owrBL
            // 
            this.label_owrBL.AutoSize = true;
            this.label_owrBL.Location = new System.Drawing.Point(38, 117);
            this.label_owrBL.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_owrBL.Name = "label_owrBL";
            this.label_owrBL.Size = new System.Drawing.Size(29, 20);
            this.label_owrBL.TabIndex = 0;
            this.label_owrBL.Text = "BL";
            // 
            // label_owrBR
            // 
            this.label_owrBR.AutoSize = true;
            this.label_owrBR.Location = new System.Drawing.Point(152, 117);
            this.label_owrBR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_owrBR.Name = "label_owrBR";
            this.label_owrBR.Size = new System.Drawing.Size(32, 20);
            this.label_owrBR.TabIndex = 0;
            this.label_owrBR.Text = "BR";
            // 
            // groupBox_General
            // 
            this.groupBox_General.Controls.Add(this.button_SetCenterOffset);
            this.groupBox_General.Controls.Add(this.button_ResetDefaults);
            this.groupBox_General.Controls.Add(this.button_BluetoothAddDevice);
            this.groupBox_General.Controls.Add(this.button_Connect);
            this.groupBox_General.Location = new System.Drawing.Point(386, 242);
            this.groupBox_General.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox_General.Name = "groupBox_General";
            this.groupBox_General.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox_General.Size = new System.Drawing.Size(560, 210);
            this.groupBox_General.TabIndex = 0;
            this.groupBox_General.TabStop = false;
            this.groupBox_General.Text = "General";
            // 
            // button_SetCenterOffset
            // 
            this.button_SetCenterOffset.Location = new System.Drawing.Point(23, 40);
            this.button_SetCenterOffset.Margin = new System.Windows.Forms.Padding(5);
            this.button_SetCenterOffset.Name = "button_SetCenterOffset";
            this.button_SetCenterOffset.Size = new System.Drawing.Size(207, 43);
            this.button_SetCenterOffset.TabIndex = 2;
            this.button_SetCenterOffset.Text = "Set balance as center";
            this.button_SetCenterOffset.UseVisualStyleBackColor = true;
            this.button_SetCenterOffset.Click += new System.EventHandler(this.button_SetCenterOffset_Click);
            // 
            // button_ResetDefaults
            // 
            this.button_ResetDefaults.Location = new System.Drawing.Point(23, 143);
            this.button_ResetDefaults.Margin = new System.Windows.Forms.Padding(5);
            this.button_ResetDefaults.Name = "button_ResetDefaults";
            this.button_ResetDefaults.Size = new System.Drawing.Size(207, 43);
            this.button_ResetDefaults.TabIndex = 3;
            this.button_ResetDefaults.Text = "Reset defaults and close";
            this.button_ResetDefaults.UseVisualStyleBackColor = true;
            this.button_ResetDefaults.Click += new System.EventHandler(this.button_ResetDefaults_Click);
            // 
            // button_BluetoothAddDevice
            // 
            this.button_BluetoothAddDevice.Location = new System.Drawing.Point(261, 40);
            this.button_BluetoothAddDevice.Margin = new System.Windows.Forms.Padding(5);
            this.button_BluetoothAddDevice.Name = "button_BluetoothAddDevice";
            this.button_BluetoothAddDevice.Size = new System.Drawing.Size(275, 42);
            this.button_BluetoothAddDevice.TabIndex = 1;
            this.button_BluetoothAddDevice.Text = "Add bluetooth Wii device";
            this.button_BluetoothAddDevice.UseVisualStyleBackColor = true;
            this.button_BluetoothAddDevice.Click += new System.EventHandler(this.button_BluetoothAddDevice_Click);
            // 
            // groupBox_BalanceRatio
            // 
            this.groupBox_BalanceRatio.Controls.Add(this.label_brDF);
            this.groupBox_BalanceRatio.Controls.Add(this.label_brX);
            this.groupBox_BalanceRatio.Controls.Add(this.label_brDR);
            this.groupBox_BalanceRatio.Controls.Add(this.label_brDL);
            this.groupBox_BalanceRatio.Controls.Add(this.label_brY);
            this.groupBox_BalanceRatio.Location = new System.Drawing.Point(720, 18);
            this.groupBox_BalanceRatio.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox_BalanceRatio.Name = "groupBox_BalanceRatio";
            this.groupBox_BalanceRatio.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox_BalanceRatio.Size = new System.Drawing.Size(225, 214);
            this.groupBox_BalanceRatio.TabIndex = 5;
            this.groupBox_BalanceRatio.TabStop = false;
            this.groupBox_BalanceRatio.Text = "Balance Ratio";
            this.groupBox_BalanceRatio.Enter += new System.EventHandler(this.groupBox_BalanceRatio_Enter);
            // 
            // label_brDF
            // 
            this.label_brDF.AutoSize = true;
            this.label_brDF.Location = new System.Drawing.Point(98, 174);
            this.label_brDF.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_brDF.Name = "label_brDF";
            this.label_brDF.Size = new System.Drawing.Size(31, 20);
            this.label_brDF.TabIndex = 0;
            this.label_brDF.Text = "DF";
            this.label_brDF.Click += new System.EventHandler(this.label_brDF_Click);
            // 
            // groupBox_BalanceRatioTriggers
            // 
            this.groupBox_BalanceRatioTriggers.Controls.Add(this.numericUpDown_TMFB);
            this.groupBox_BalanceRatioTriggers.Controls.Add(this.numericUpDown_TMLR);
            this.groupBox_BalanceRatioTriggers.Controls.Add(this.numericUpDown_TFB);
            this.groupBox_BalanceRatioTriggers.Controls.Add(this.numericUpDown_TLR);
            this.groupBox_BalanceRatioTriggers.Controls.Add(this.label_TMFB);
            this.groupBox_BalanceRatioTriggers.Controls.Add(this.label_TMLR);
            this.groupBox_BalanceRatioTriggers.Controls.Add(this.label_TFB);
            this.groupBox_BalanceRatioTriggers.Controls.Add(this.label_TLR);
            this.groupBox_BalanceRatioTriggers.Location = new System.Drawing.Point(18, 242);
            this.groupBox_BalanceRatioTriggers.Margin = new System.Windows.Forms.Padding(5);
            this.groupBox_BalanceRatioTriggers.Name = "groupBox_BalanceRatioTriggers";
            this.groupBox_BalanceRatioTriggers.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox_BalanceRatioTriggers.Size = new System.Drawing.Size(358, 210);
            this.groupBox_BalanceRatioTriggers.TabIndex = 1;
            this.groupBox_BalanceRatioTriggers.TabStop = false;
            this.groupBox_BalanceRatioTriggers.Text = "Balance Ratio Triggers";
            // 
            // numericUpDown_TMFB
            // 
            this.numericUpDown_TMFB.Location = new System.Drawing.Point(267, 160);
            this.numericUpDown_TMFB.Margin = new System.Windows.Forms.Padding(5);
            this.numericUpDown_TMFB.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.numericUpDown_TMFB.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_TMFB.Name = "numericUpDown_TMFB";
            this.numericUpDown_TMFB.Size = new System.Drawing.Size(74, 26);
            this.numericUpDown_TMFB.TabIndex = 3;
            this.numericUpDown_TMFB.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_TMFB.ValueChanged += new System.EventHandler(this.numericUpDown_TMFB_ValueChanged);
            // 
            // numericUpDown_TMLR
            // 
            this.numericUpDown_TMLR.Location = new System.Drawing.Point(267, 120);
            this.numericUpDown_TMLR.Margin = new System.Windows.Forms.Padding(5);
            this.numericUpDown_TMLR.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.numericUpDown_TMLR.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_TMLR.Name = "numericUpDown_TMLR";
            this.numericUpDown_TMLR.Size = new System.Drawing.Size(74, 26);
            this.numericUpDown_TMLR.TabIndex = 2;
            this.numericUpDown_TMLR.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_TMLR.ValueChanged += new System.EventHandler(this.numericUpDown_TMLR_ValueChanged);
            // 
            // numericUpDown_TFB
            // 
            this.numericUpDown_TFB.Location = new System.Drawing.Point(267, 80);
            this.numericUpDown_TFB.Margin = new System.Windows.Forms.Padding(5);
            this.numericUpDown_TFB.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.numericUpDown_TFB.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_TFB.Name = "numericUpDown_TFB";
            this.numericUpDown_TFB.Size = new System.Drawing.Size(74, 26);
            this.numericUpDown_TFB.TabIndex = 1;
            this.numericUpDown_TFB.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_TFB.ValueChanged += new System.EventHandler(this.numericUpDown_TFB_ValueChanged);
            // 
            // numericUpDown_TLR
            // 
            this.numericUpDown_TLR.Location = new System.Drawing.Point(267, 40);
            this.numericUpDown_TLR.Margin = new System.Windows.Forms.Padding(5);
            this.numericUpDown_TLR.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.numericUpDown_TLR.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_TLR.Name = "numericUpDown_TLR";
            this.numericUpDown_TLR.Size = new System.Drawing.Size(74, 26);
            this.numericUpDown_TLR.TabIndex = 0;
            this.numericUpDown_TLR.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_TLR.ValueChanged += new System.EventHandler(this.numericUpDown_TLR_ValueChanged);
            // 
            // label_TMFB
            // 
            this.label_TMFB.AutoSize = true;
            this.label_TMFB.Location = new System.Drawing.Point(9, 163);
            this.label_TMFB.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_TMFB.Name = "label_TMFB";
            this.label_TMFB.Size = new System.Drawing.Size(226, 20);
            this.label_TMFB.TabIndex = 0;
            this.label_TMFB.Text = "- Modifier + Foward / Backward";
            // 
            // label_TMLR
            // 
            this.label_TMLR.AutoSize = true;
            this.label_TMLR.Location = new System.Drawing.Point(9, 123);
            this.label_TMLR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_TMLR.Name = "label_TMLR";
            this.label_TMLR.Size = new System.Drawing.Size(169, 20);
            this.label_TMLR.TabIndex = 0;
            this.label_TMLR.Text = "- Modifier + Left / Right";
            // 
            // label_TFB
            // 
            this.label_TFB.AutoSize = true;
            this.label_TFB.Location = new System.Drawing.Point(9, 83);
            this.label_TFB.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_TFB.Name = "label_TFB";
            this.label_TFB.Size = new System.Drawing.Size(158, 20);
            this.label_TFB.TabIndex = 0;
            this.label_TFB.Text = "- Forward / Backward";
            // 
            // label_TLR
            // 
            this.label_TLR.AutoSize = true;
            this.label_TLR.Location = new System.Drawing.Point(9, 43);
            this.label_TLR.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_TLR.Name = "label_TLR";
            this.label_TLR.Size = new System.Drawing.Size(96, 20);
            this.label_TLR.TabIndex = 0;
            this.label_TLR.Text = "- Left / Right";
            // 
            // label_Status
            // 
            this.label_Status.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_Status.Location = new System.Drawing.Point(18, 455);
            this.label_Status.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_Status.Name = "label_Status";
            this.label_Status.Size = new System.Drawing.Size(927, 37);
            this.label_Status.TabIndex = 4;
            this.label_Status.Text = "STATUS";
            this.label_Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 512);
            this.Controls.Add(this.label_Status);
            this.Controls.Add(this.groupBox_BalanceRatioTriggers);
            this.Controls.Add(this.groupBox_BalanceRatio);
            this.Controls.Add(this.groupBox_General);
            this.Controls.Add(this.groupBox_OffsetWeightRatio);
            this.Controls.Add(this.groupBox_OffsetWeight);
            this.Controls.Add(this.groupBox_RawWeight);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MediBalance Wii Balance Board";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox_RawWeight.ResumeLayout(false);
            this.groupBox_RawWeight.PerformLayout();
            this.groupBox_OffsetWeight.ResumeLayout(false);
            this.groupBox_OffsetWeight.PerformLayout();
            this.groupBox_OffsetWeightRatio.ResumeLayout(false);
            this.groupBox_OffsetWeightRatio.PerformLayout();
            this.groupBox_General.ResumeLayout(false);
            this.groupBox_BalanceRatio.ResumeLayout(false);
            this.groupBox_BalanceRatio.PerformLayout();
            this.groupBox_BalanceRatioTriggers.ResumeLayout(false);
            this.groupBox_BalanceRatioTriggers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TMFB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TMLR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TFB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_TLR)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_rwWT;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.Label label_brX;
        private System.Windows.Forms.Label label_brY;
        private System.Windows.Forms.Label label_brDL;
        private System.Windows.Forms.Label label_brDR;
        private System.Windows.Forms.GroupBox groupBox_RawWeight;
        private System.Windows.Forms.Label label_rwBR;
        private System.Windows.Forms.Label label_rwBL;
        private System.Windows.Forms.Label label_rwTR;
        private System.Windows.Forms.Label label_rwTL;
        private System.Windows.Forms.GroupBox groupBox_OffsetWeight;
        private System.Windows.Forms.GroupBox groupBox_OffsetWeightRatio;
        private System.Windows.Forms.Label label_owTL;
        private System.Windows.Forms.Label label_owTR;
        private System.Windows.Forms.Label label_owBL;
        private System.Windows.Forms.Label label_owBR;
        private System.Windows.Forms.Label label_owrTL;
        private System.Windows.Forms.Label label_owrTR;
        private System.Windows.Forms.Label label_owrBL;
        private System.Windows.Forms.Label label_owrBR;
        private System.Windows.Forms.GroupBox groupBox_General;
        private System.Windows.Forms.GroupBox groupBox_BalanceRatio;
        private System.Windows.Forms.Label label_brDF;
        private System.Windows.Forms.GroupBox groupBox_BalanceRatioTriggers;
        private System.Windows.Forms.NumericUpDown numericUpDown_TMFB;
        private System.Windows.Forms.NumericUpDown numericUpDown_TMLR;
        private System.Windows.Forms.NumericUpDown numericUpDown_TFB;
        private System.Windows.Forms.NumericUpDown numericUpDown_TLR;
        private System.Windows.Forms.Label label_TMFB;
        private System.Windows.Forms.Label label_TMLR;
        private System.Windows.Forms.Label label_TFB;
        private System.Windows.Forms.Label label_TLR;
        private System.Windows.Forms.Label label_Status;
        private System.Windows.Forms.Button button_BluetoothAddDevice;
        private System.Windows.Forms.Button button_SetCenterOffset;
        private System.Windows.Forms.Button button_ResetDefaults;
        private System.Windows.Forms.Label label_owWT;
    }
}

