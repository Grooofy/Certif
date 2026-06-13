namespace Certif
{
    partial class lblHours
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pbPreview = new PictureBox();
            gbNameSettings = new GroupBox();
            button2 = new Button();
            numFioSize = new NumericUpDown();
            numFioY = new NumericUpDown();
            numFioX = new NumericUpDown();
            gbRegionSettings = new GroupBox();
            btnPlaceReg = new Button();
            numRegSize = new NumericUpDown();
            numRegY = new NumericUpDown();
            numRegX = new NumericUpDown();
            gbTableSettings = new GroupBox();
            lblSquadHint = new Label();
            cmbSquadCol = new ComboBox();
            lblLevelHint = new Label();
            cmbLevelCol = new ComboBox();
            lblWorkshopHint = new Label();
            cmbWorkshopCol = new ComboBox();
            lblDataInfo = new Label();
            lblRegHint = new Label();
            lblFioHint = new Label();
            cmbRegCol = new ComboBox();
            cmbFioCol = new ComboBox();
            btnSelectData = new Button();
            btnSelectBg = new Button();
            btnSelectFont = new Button();
            btnGenerate = new Button();
            lblFontName = new Label();
            lblStatus = new Label();
            txtHours = new TextBox();
            btnPlaceHours = new Button();
            groupBox3 = new GroupBox();
            numHoursSize = new NumericUpDown();
            numHoursY = new NumericUpDown();
            numHoursX = new NumericUpDown();
            groupBox1 = new GroupBox();
            btnPlaceWorkshop = new Button();
            numWorkshopSize = new NumericUpDown();
            numWorkshopY = new NumericUpDown();
            numWorkshopX = new NumericUpDown();
            groupBox2 = new GroupBox();
            numLevelXText = new Label();
            btnPlaceLevel = new Button();
            numLevelSize = new NumericUpDown();
            numLevelY = new NumericUpDown();
            numLevelX = new NumericUpDown();
            groupBox4 = new GroupBox();
            btnPlaceSquad = new Button();
            numSquadSize = new NumericUpDown();
            numSquadY = new NumericUpDown();
            numSquadX = new NumericUpDown();
            numLevelYText = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pbPreview).BeginInit();
            gbNameSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numFioSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFioY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numFioX).BeginInit();
            gbRegionSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRegSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRegY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numRegX).BeginInit();
            gbTableSettings.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numHoursSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHoursY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numHoursX).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numWorkshopSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWorkshopY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numWorkshopX).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numLevelSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLevelY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numLevelX).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSquadSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSquadY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numSquadX).BeginInit();
            SuspendLayout();
            // 
            // pbPreview
            // 
            pbPreview.Location = new Point(24, 21);
            pbPreview.Margin = new Padding(2);
            pbPreview.Name = "pbPreview";
            pbPreview.Size = new Size(488, 589);
            pbPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pbPreview.TabIndex = 0;
            pbPreview.TabStop = false;
            pbPreview.MouseClick += pbPreview_MouseClick;
            // 
            // gbNameSettings
            // 
            gbNameSettings.Controls.Add(button2);
            gbNameSettings.Controls.Add(numFioSize);
            gbNameSettings.Controls.Add(numFioY);
            gbNameSettings.Controls.Add(numFioX);
            gbNameSettings.Location = new Point(528, 511);
            gbNameSettings.Margin = new Padding(2);
            gbNameSettings.Name = "gbNameSettings";
            gbNameSettings.Padding = new Padding(2);
            gbNameSettings.Size = new Size(210, 176);
            gbNameSettings.TabIndex = 1;
            gbNameSettings.TabStop = false;
            gbNameSettings.Text = "Параметры ФИО";
            gbNameSettings.Enter += gbTextSettings_Enter;
            // 
            // button2
            // 
            button2.Location = new Point(17, 136);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(189, 37);
            button2.TabIndex = 4;
            button2.Text = "Разместить ФИО на картинке";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // numFioSize
            // 
            numFioSize.Location = new Point(64, 106);
            numFioSize.Margin = new Padding(2);
            numFioSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numFioSize.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numFioSize.Name = "numFioSize";
            numFioSize.Size = new Size(126, 23);
            numFioSize.TabIndex = 2;
            numFioSize.Value = new decimal(new int[] { 48, 0, 0, 0 });
            // 
            // numFioY
            // 
            numFioY.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numFioY.Location = new Point(64, 67);
            numFioY.Margin = new Padding(2);
            numFioY.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numFioY.Name = "numFioY";
            numFioY.Size = new Size(126, 23);
            numFioY.TabIndex = 1;
            // 
            // numFioX
            // 
            numFioX.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numFioX.Location = new Point(64, 31);
            numFioX.Margin = new Padding(2);
            numFioX.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numFioX.Name = "numFioX";
            numFioX.Size = new Size(126, 23);
            numFioX.TabIndex = 0;
            // 
            // gbRegionSettings
            // 
            gbRegionSettings.Controls.Add(btnPlaceReg);
            gbRegionSettings.Controls.Add(numRegSize);
            gbRegionSettings.Controls.Add(numRegY);
            gbRegionSettings.Controls.Add(numRegX);
            gbRegionSettings.Location = new Point(753, 331);
            gbRegionSettings.Margin = new Padding(2);
            gbRegionSettings.Name = "gbRegionSettings";
            gbRegionSettings.Padding = new Padding(2);
            gbRegionSettings.Size = new Size(210, 176);
            gbRegionSettings.TabIndex = 3;
            gbRegionSettings.TabStop = false;
            gbRegionSettings.Text = "Параметры региона";
            // 
            // btnPlaceReg
            // 
            btnPlaceReg.Location = new Point(10, 136);
            btnPlaceReg.Margin = new Padding(2);
            btnPlaceReg.Name = "btnPlaceReg";
            btnPlaceReg.Size = new Size(189, 37);
            btnPlaceReg.TabIndex = 3;
            btnPlaceReg.Text = "Разместить Регион на картинке";
            btnPlaceReg.UseVisualStyleBackColor = true;
            btnPlaceReg.Click += btnPlaceReg_Click;
            // 
            // numRegSize
            // 
            numRegSize.Location = new Point(64, 106);
            numRegSize.Margin = new Padding(2);
            numRegSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numRegSize.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numRegSize.Name = "numRegSize";
            numRegSize.Size = new Size(126, 23);
            numRegSize.TabIndex = 2;
            numRegSize.Value = new decimal(new int[] { 36, 0, 0, 0 });
            // 
            // numRegY
            // 
            numRegY.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numRegY.Location = new Point(64, 67);
            numRegY.Margin = new Padding(2);
            numRegY.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numRegY.Name = "numRegY";
            numRegY.Size = new Size(126, 23);
            numRegY.TabIndex = 1;
            // 
            // numRegX
            // 
            numRegX.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numRegX.Location = new Point(64, 31);
            numRegX.Margin = new Padding(2);
            numRegX.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numRegX.Name = "numRegX";
            numRegX.Size = new Size(126, 23);
            numRegX.TabIndex = 0;
            // 
            // gbTableSettings
            // 
            gbTableSettings.Controls.Add(lblSquadHint);
            gbTableSettings.Controls.Add(cmbSquadCol);
            gbTableSettings.Controls.Add(lblLevelHint);
            gbTableSettings.Controls.Add(cmbLevelCol);
            gbTableSettings.Controls.Add(lblWorkshopHint);
            gbTableSettings.Controls.Add(cmbWorkshopCol);
            gbTableSettings.Controls.Add(lblDataInfo);
            gbTableSettings.Controls.Add(lblRegHint);
            gbTableSettings.Controls.Add(lblFioHint);
            gbTableSettings.Controls.Add(cmbRegCol);
            gbTableSettings.Controls.Add(cmbFioCol);
            gbTableSettings.Controls.Add(btnSelectData);
            gbTableSettings.Location = new Point(528, 21);
            gbTableSettings.Margin = new Padding(2);
            gbTableSettings.Name = "gbTableSettings";
            gbTableSettings.Padding = new Padding(2);
            gbTableSettings.Size = new Size(239, 307);
            gbTableSettings.TabIndex = 4;
            gbTableSettings.TabStop = false;
            gbTableSettings.Text = "Таблица";
            // 
            // lblSquadHint
            // 
            lblSquadHint.AutoSize = true;
            lblSquadHint.Location = new Point(10, 199);
            lblSquadHint.Margin = new Padding(2, 0, 2, 0);
            lblSquadHint.Name = "lblSquadHint";
            lblSquadHint.Size = new Size(149, 15);
            lblSquadHint.TabIndex = 20;
            lblSquadHint.Text = "Колонка отряда освоения";
            // 
            // cmbSquadCol
            // 
            cmbSquadCol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSquadCol.FormattingEnabled = true;
            cmbSquadCol.Location = new Point(10, 216);
            cmbSquadCol.Margin = new Padding(2);
            cmbSquadCol.Name = "cmbSquadCol";
            cmbSquadCol.Size = new Size(129, 23);
            cmbSquadCol.TabIndex = 19;
            // 
            // lblLevelHint
            // 
            lblLevelHint.AutoSize = true;
            lblLevelHint.Location = new Point(10, 161);
            lblLevelHint.Margin = new Padding(2, 0, 2, 0);
            lblLevelHint.Name = "lblLevelHint";
            lblLevelHint.Size = new Size(157, 15);
            lblLevelHint.TabIndex = 18;
            lblLevelHint.Text = "Колонка уровеня освоения";
            // 
            // cmbLevelCol
            // 
            cmbLevelCol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLevelCol.FormattingEnabled = true;
            cmbLevelCol.Location = new Point(10, 178);
            cmbLevelCol.Margin = new Padding(2);
            cmbLevelCol.Name = "cmbLevelCol";
            cmbLevelCol.Size = new Size(129, 23);
            cmbLevelCol.TabIndex = 17;
            // 
            // lblWorkshopHint
            // 
            lblWorkshopHint.AutoSize = true;
            lblWorkshopHint.Location = new Point(10, 117);
            lblWorkshopHint.Margin = new Padding(2, 0, 2, 0);
            lblWorkshopHint.Name = "lblWorkshopHint";
            lblWorkshopHint.Size = new Size(95, 15);
            lblWorkshopHint.TabIndex = 16;
            lblWorkshopHint.Text = "Колонка Студия";
            // 
            // cmbWorkshopCol
            // 
            cmbWorkshopCol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWorkshopCol.FormattingEnabled = true;
            cmbWorkshopCol.Location = new Point(10, 134);
            cmbWorkshopCol.Margin = new Padding(2);
            cmbWorkshopCol.Name = "cmbWorkshopCol";
            cmbWorkshopCol.Size = new Size(129, 23);
            cmbWorkshopCol.TabIndex = 15;
            cmbWorkshopCol.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // lblDataInfo
            // 
            lblDataInfo.AutoSize = true;
            lblDataInfo.Location = new Point(97, 269);
            lblDataInfo.Margin = new Padding(2, 0, 2, 0);
            lblDataInfo.Name = "lblDataInfo";
            lblDataInfo.Size = new Size(130, 15);
            lblDataInfo.TabIndex = 11;
            lblDataInfo.Text = "Таблица не загружена";
            lblDataInfo.Click += lblDataInfo_Click;
            // 
            // lblRegHint
            // 
            lblRegHint.AutoSize = true;
            lblRegHint.Location = new Point(10, 67);
            lblRegHint.Margin = new Padding(2, 0, 2, 0);
            lblRegHint.Name = "lblRegHint";
            lblRegHint.Size = new Size(96, 15);
            lblRegHint.TabIndex = 14;
            lblRegHint.Text = "Колонка Регион";
            // 
            // lblFioHint
            // 
            lblFioHint.AutoSize = true;
            lblFioHint.Location = new Point(10, 22);
            lblFioHint.Margin = new Padding(2, 0, 2, 0);
            lblFioHint.Name = "lblFioHint";
            lblFioHint.Size = new Size(84, 15);
            lblFioHint.TabIndex = 13;
            lblFioHint.Text = "Колонка ФИО";
            lblFioHint.Click += label1_Click;
            // 
            // cmbRegCol
            // 
            cmbRegCol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRegCol.FormattingEnabled = true;
            cmbRegCol.Location = new Point(10, 87);
            cmbRegCol.Margin = new Padding(2);
            cmbRegCol.Name = "cmbRegCol";
            cmbRegCol.Size = new Size(129, 23);
            cmbRegCol.TabIndex = 4;
            // 
            // cmbFioCol
            // 
            cmbFioCol.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFioCol.FormattingEnabled = true;
            cmbFioCol.Location = new Point(10, 39);
            cmbFioCol.Margin = new Padding(2);
            cmbFioCol.Name = "cmbFioCol";
            cmbFioCol.Size = new Size(129, 23);
            cmbFioCol.TabIndex = 3;
            // 
            // btnSelectData
            // 
            btnSelectData.Location = new Point(10, 256);
            btnSelectData.Margin = new Padding(2);
            btnSelectData.Name = "btnSelectData";
            btnSelectData.Size = new Size(82, 42);
            btnSelectData.TabIndex = 7;
            btnSelectData.Text = "Выбрать таблицу";
            btnSelectData.UseVisualStyleBackColor = true;
            btnSelectData.Click += btnSelectData_Click;
            // 
            // btnSelectBg
            // 
            btnSelectBg.Location = new Point(24, 622);
            btnSelectBg.Margin = new Padding(2);
            btnSelectBg.Name = "btnSelectBg";
            btnSelectBg.Size = new Size(128, 55);
            btnSelectBg.TabIndex = 5;
            btnSelectBg.Text = "Загрузить подложку";
            btnSelectBg.UseVisualStyleBackColor = true;
            btnSelectBg.Click += btnSelectBg_Click;
            // 
            // btnSelectFont
            // 
            btnSelectFont.Location = new Point(172, 635);
            btnSelectFont.Margin = new Padding(2);
            btnSelectFont.Name = "btnSelectFont";
            btnSelectFont.Size = new Size(153, 52);
            btnSelectFont.TabIndex = 6;
            btnSelectFont.Text = "Выбрать шрифт (.ttf)";
            btnSelectFont.UseVisualStyleBackColor = true;
            btnSelectFont.Click += btnSelectFont_Click;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(346, 625);
            btnGenerate.Margin = new Padding(2);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(153, 51);
            btnGenerate.TabIndex = 9;
            btnGenerate.Text = "Создать PDF";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += btnGenerate_Click;
            // 
            // lblFontName
            // 
            lblFontName.AutoSize = true;
            lblFontName.Location = new Point(195, 618);
            lblFontName.Margin = new Padding(2, 0, 2, 0);
            lblFontName.Name = "lblFontName";
            lblFontName.Size = new Size(107, 15);
            lblFontName.TabIndex = 10;
            lblFontName.Text = "Шрифт не выбран";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(1, 685);
            lblStatus.Margin = new Padding(2, 0, 2, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(38, 15);
            lblStatus.TabIndex = 12;
            lblStatus.Text = "Готов";
            // 
            // txtHours
            // 
            txtHours.Location = new Point(4, 22);
            txtHours.Margin = new Padding(2);
            txtHours.Name = "txtHours";
            txtHours.Size = new Size(106, 23);
            txtHours.TabIndex = 14;
            // 
            // btnPlaceHours
            // 
            btnPlaceHours.Location = new Point(76, 144);
            btnPlaceHours.Margin = new Padding(2);
            btnPlaceHours.Name = "btnPlaceHours";
            btnPlaceHours.Size = new Size(113, 20);
            btnPlaceHours.TabIndex = 15;
            btnPlaceHours.Text = "Разместить часы";
            btnPlaceHours.UseVisualStyleBackColor = true;
            btnPlaceHours.Click += btnPlaceHours_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(numHoursSize);
            groupBox3.Controls.Add(btnPlaceHours);
            groupBox3.Controls.Add(txtHours);
            groupBox3.Controls.Add(numHoursY);
            groupBox3.Controls.Add(numHoursX);
            groupBox3.Location = new Point(528, 331);
            groupBox3.Margin = new Padding(2);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(2);
            groupBox3.Size = new Size(210, 176);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            groupBox3.Text = "Количество часов";
            // 
            // numHoursSize
            // 
            numHoursSize.Location = new Point(64, 106);
            numHoursSize.Margin = new Padding(2);
            numHoursSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numHoursSize.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numHoursSize.Name = "numHoursSize";
            numHoursSize.Size = new Size(126, 23);
            numHoursSize.TabIndex = 2;
            numHoursSize.Value = new decimal(new int[] { 48, 0, 0, 0 });
            // 
            // numHoursY
            // 
            numHoursY.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numHoursY.Location = new Point(64, 84);
            numHoursY.Margin = new Padding(2);
            numHoursY.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numHoursY.Name = "numHoursY";
            numHoursY.Size = new Size(126, 23);
            numHoursY.TabIndex = 1;
            // 
            // numHoursX
            // 
            numHoursX.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numHoursX.Location = new Point(64, 62);
            numHoursX.Margin = new Padding(2);
            numHoursX.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numHoursX.Name = "numHoursX";
            numHoursX.Size = new Size(126, 23);
            numHoursX.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnPlaceWorkshop);
            groupBox1.Controls.Add(numWorkshopSize);
            groupBox1.Controls.Add(numWorkshopY);
            groupBox1.Controls.Add(numWorkshopX);
            groupBox1.Location = new Point(757, 511);
            groupBox1.Margin = new Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(2);
            groupBox1.Size = new Size(210, 176);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Параметры Студию";
            // 
            // btnPlaceWorkshop
            // 
            btnPlaceWorkshop.Location = new Point(10, 136);
            btnPlaceWorkshop.Margin = new Padding(2);
            btnPlaceWorkshop.Name = "btnPlaceWorkshop";
            btnPlaceWorkshop.Size = new Size(189, 37);
            btnPlaceWorkshop.TabIndex = 3;
            btnPlaceWorkshop.Text = "Разместить Студию на картинке";
            btnPlaceWorkshop.UseVisualStyleBackColor = true;
            btnPlaceWorkshop.Click += btnPlaceWorkshop_Click;
            // 
            // numWorkshopSize
            // 
            numWorkshopSize.Location = new Point(64, 106);
            numWorkshopSize.Margin = new Padding(2);
            numWorkshopSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numWorkshopSize.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numWorkshopSize.Name = "numWorkshopSize";
            numWorkshopSize.Size = new Size(126, 23);
            numWorkshopSize.TabIndex = 2;
            numWorkshopSize.Value = new decimal(new int[] { 36, 0, 0, 0 });
            // 
            // numWorkshopY
            // 
            numWorkshopY.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numWorkshopY.Location = new Point(64, 67);
            numWorkshopY.Margin = new Padding(2);
            numWorkshopY.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numWorkshopY.Name = "numWorkshopY";
            numWorkshopY.Size = new Size(126, 23);
            numWorkshopY.TabIndex = 1;
            // 
            // numWorkshopX
            // 
            numWorkshopX.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numWorkshopX.Location = new Point(64, 31);
            numWorkshopX.Margin = new Padding(2);
            numWorkshopX.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numWorkshopX.Name = "numWorkshopX";
            numWorkshopX.Size = new Size(126, 23);
            numWorkshopX.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(numLevelYText);
            groupBox2.Controls.Add(numLevelXText);
            groupBox2.Controls.Add(btnPlaceLevel);
            groupBox2.Controls.Add(numLevelSize);
            groupBox2.Controls.Add(numLevelY);
            groupBox2.Controls.Add(numLevelX);
            groupBox2.Location = new Point(1292, 43);
            groupBox2.Margin = new Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(2);
            groupBox2.Size = new Size(155, 155);
            groupBox2.TabIndex = 5;
            groupBox2.TabStop = false;
            groupBox2.Text = "Параметры уровня освоения";
            // 
            // numLevelXText
            // 
            numLevelXText.AutoSize = true;
            numLevelXText.Location = new Point(57, 32);
            numLevelXText.Name = "numLevelXText";
            numLevelXText.Size = new Size(38, 15);
            numLevelXText.TabIndex = 4;
            numLevelXText.Text = "Ось Х";
            // 
            // btnPlaceLevel
            // 
            btnPlaceLevel.Location = new Point(4, 111);
            btnPlaceLevel.Margin = new Padding(2);
            btnPlaceLevel.Name = "btnPlaceLevel";
            btnPlaceLevel.Size = new Size(140, 40);
            btnPlaceLevel.TabIndex = 3;
            btnPlaceLevel.Text = "Разместить Уровень на картинке";
            btnPlaceLevel.UseVisualStyleBackColor = true;
            btnPlaceLevel.Click += btnPlaceLevel_Click;
            // 
            // numLevelSize
            // 
            numLevelSize.Location = new Point(4, 84);
            numLevelSize.Margin = new Padding(2);
            numLevelSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numLevelSize.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numLevelSize.Name = "numLevelSize";
            numLevelSize.Size = new Size(48, 23);
            numLevelSize.TabIndex = 2;
            numLevelSize.Value = new decimal(new int[] { 36, 0, 0, 0 });
            // 
            // numLevelY
            // 
            numLevelY.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numLevelY.Location = new Point(4, 57);
            numLevelY.Margin = new Padding(2);
            numLevelY.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numLevelY.Name = "numLevelY";
            numLevelY.Size = new Size(48, 23);
            numLevelY.TabIndex = 1;
            // 
            // numLevelX
            // 
            numLevelX.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numLevelX.Location = new Point(4, 30);
            numLevelX.Margin = new Padding(2);
            numLevelX.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numLevelX.Name = "numLevelX";
            numLevelX.Size = new Size(48, 23);
            numLevelX.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(btnPlaceSquad);
            groupBox4.Controls.Add(numSquadSize);
            groupBox4.Controls.Add(numSquadY);
            groupBox4.Controls.Add(numSquadX);
            groupBox4.Location = new Point(979, 46);
            groupBox4.Margin = new Padding(2);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(2);
            groupBox4.Size = new Size(210, 176);
            groupBox4.TabIndex = 13;
            groupBox4.TabStop = false;
            groupBox4.Text = "Параметры отряда";
            // 
            // btnPlaceSquad
            // 
            btnPlaceSquad.Location = new Point(10, 136);
            btnPlaceSquad.Margin = new Padding(2);
            btnPlaceSquad.Name = "btnPlaceSquad";
            btnPlaceSquad.Size = new Size(189, 37);
            btnPlaceSquad.TabIndex = 3;
            btnPlaceSquad.Text = "Разместить Отряд на картинке";
            btnPlaceSquad.UseVisualStyleBackColor = true;
            btnPlaceSquad.Click += btnPlaceSquad_Click;
            // 
            // numSquadSize
            // 
            numSquadSize.Location = new Point(64, 106);
            numSquadSize.Margin = new Padding(2);
            numSquadSize.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            numSquadSize.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            numSquadSize.Name = "numSquadSize";
            numSquadSize.Size = new Size(126, 23);
            numSquadSize.TabIndex = 2;
            numSquadSize.Value = new decimal(new int[] { 36, 0, 0, 0 });
            // 
            // numSquadY
            // 
            numSquadY.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numSquadY.Location = new Point(64, 67);
            numSquadY.Margin = new Padding(2);
            numSquadY.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numSquadY.Name = "numSquadY";
            numSquadY.Size = new Size(126, 23);
            numSquadY.TabIndex = 1;
            // 
            // numSquadX
            // 
            numSquadX.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            numSquadX.Location = new Point(64, 31);
            numSquadX.Margin = new Padding(2);
            numSquadX.Maximum = new decimal(new int[] { 5000, 0, 0, 0 });
            numSquadX.Name = "numSquadX";
            numSquadX.Size = new Size(126, 23);
            numSquadX.TabIndex = 0;
            // 
            // numLevelYText
            // 
            numLevelYText.AutoSize = true;
            numLevelYText.Location = new Point(57, 59);
            numLevelYText.Name = "numLevelYText";
            numLevelYText.Size = new Size(38, 15);
            numLevelYText.TabIndex = 5;
            numLevelYText.Text = "Ось Y";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(57, 88);
            label1.Name = "label1";
            label1.Size = new Size(95, 15);
            label1.TabIndex = 6;
            label1.Text = "Размер шрифта";
            // 
            // lblHours
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1474, 750);
            Controls.Add(groupBox4);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(groupBox3);
            Controls.Add(lblStatus);
            Controls.Add(lblFontName);
            Controls.Add(btnGenerate);
            Controls.Add(btnSelectFont);
            Controls.Add(btnSelectBg);
            Controls.Add(gbTableSettings);
            Controls.Add(gbRegionSettings);
            Controls.Add(gbNameSettings);
            Controls.Add(pbPreview);
            Margin = new Padding(2);
            Name = "lblHours";
            Text = "Сертификат в 2 клика";
            KeyDown += Form1_KeyDown;
            ((System.ComponentModel.ISupportInitialize)pbPreview).EndInit();
            gbNameSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numFioSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFioY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numFioX).EndInit();
            gbRegionSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numRegSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRegY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numRegX).EndInit();
            gbTableSettings.ResumeLayout(false);
            gbTableSettings.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numHoursSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHoursY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numHoursX).EndInit();
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numWorkshopSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWorkshopY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numWorkshopX).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numLevelSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numLevelY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numLevelX).EndInit();
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numSquadSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSquadY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numSquadX).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pbPreview;
        private GroupBox gbNameSettings;
        private NumericUpDown numFioSize;
        private NumericUpDown numFioY;
        private NumericUpDown numFioX;
        private GroupBox gbRegionSettings;
        private NumericUpDown numRegSize;
        private NumericUpDown numRegY;
        private NumericUpDown numRegX;
        private GroupBox gbTableSettings;
        private ComboBox cmbRegCol;
        private ComboBox cmbFioCol;
        private Button btnSelectBg;
        private Button btnSelectFont;
        private Button btnSelectData;
        private Button btnGenerate;
        private Label lblFontName;
        private Label lblDataInfo;
        private Label lblFioHint;
        private Label lblStatus;
        private Label lblRegHint;
        private Button button2;
        private Button btnPlaceReg;
        private TextBox txtHours;
        private Button btnPlaceHours;
        private GroupBox groupBox3;
        private NumericUpDown numHoursSize;
        private NumericUpDown numHoursY;
        private NumericUpDown numHoursX;
        private Label lblWorkshopHint;
        private ComboBox cmbWorkshopCol;
        private GroupBox groupBox1;
        private Button btnPlaceWorkshop;
        private NumericUpDown numWorkshopSize;
        private NumericUpDown numWorkshopY;
        private NumericUpDown numWorkshopX;
        private Label lblLevelHint;
        private ComboBox cmbLevelCol;
        private GroupBox groupBox2;
        private Button btnPlaceLevel;
        private NumericUpDown numLevelSize;
        private NumericUpDown numLevelY;
        private NumericUpDown numLevelX;
        private Label lblSquadHint;
        private ComboBox cmbSquadCol;
        private GroupBox groupBox4;
        private Button btnPlaceSquad;
        private NumericUpDown numSquadSize;
        private NumericUpDown numSquadY;
        private NumericUpDown numSquadX;
        private Label numLevelXText;
        private Label label1;
        private Label numLevelYText;
    }
}
