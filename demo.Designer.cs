namespace BakeryBI
{
    partial class demo
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
            textBox1 = new TextBox();
            panelFilters = new Panel();
            btnExport = new Button();
            btnReset = new Button();
            btnFilter = new Button();
            cmbMonth = new ComboBox();
            label4 = new Label();
            cmbYear = new ComboBox();
            Year = new Label();
            cmbCustomerType = new ComboBox();
            label3 = new Label();
            cmbCategory = new ComboBox();
            label2 = new Label();
            cmbStore = new ComboBox();
            label1 = new Label();
            dgvSales = new DataGridView();
            chartSalesByStore = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartSalesByCategory = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartMonthlyTrend = new System.Windows.Forms.DataVisualization.Charting.Chart();
            chartCustomerType = new System.Windows.Forms.DataVisualization.Charting.Chart();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            label5 = new Label();
            panelFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartSalesByStore).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartSalesByCategory).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartMonthlyTrend).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chartCustomerType).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(371, 23);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(184, 35);
            textBox1.TabIndex = 0;
            textBox1.Text = "Bakery Dashboard";
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // panelFilters
            // 
            panelFilters.BackColor = SystemColors.ButtonHighlight;
            panelFilters.Controls.Add(btnExport);
            panelFilters.Controls.Add(btnReset);
            panelFilters.Controls.Add(btnFilter);
            panelFilters.Controls.Add(cmbMonth);
            panelFilters.Controls.Add(label4);
            panelFilters.Controls.Add(cmbYear);
            panelFilters.Controls.Add(Year);
            panelFilters.Controls.Add(cmbCustomerType);
            panelFilters.Controls.Add(label3);
            panelFilters.Controls.Add(cmbCategory);
            panelFilters.Controls.Add(label2);
            panelFilters.Controls.Add(cmbStore);
            panelFilters.Controls.Add(label1);
            panelFilters.Location = new Point(12, 12);
            panelFilters.Name = "panelFilters";
            panelFilters.Size = new Size(1360, 100);
            panelFilters.TabIndex = 1;
            // 
            // btnExport
            // 
            btnExport.BackColor = SystemColors.ControlLightLight;
            btnExport.Location = new Point(1131, 3);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(120, 38);
            btnExport.TabIndex = 12;
            btnExport.Text = "Export to Excel";
            btnExport.UseMnemonic = false;
            btnExport.UseVisualStyleBackColor = false;
            btnExport.Click += btnExport_Click;
            // 
            // btnReset
            // 
            btnReset.BackColor = SystemColors.ControlLightLight;
            btnReset.Location = new Point(1188, 48);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(120, 38);
            btnReset.TabIndex = 11;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // btnFilter
            // 
            btnFilter.BackColor = SystemColors.ControlLightLight;
            btnFilter.Location = new Point(1062, 48);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(120, 38);
            btnFilter.TabIndex = 10;
            btnFilter.Text = "Apply ";
            btnFilter.UseVisualStyleBackColor = false;
            btnFilter.Click += btnFilter_Click;
            // 
            // cmbMonth
            // 
            cmbMonth.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMonth.FormattingEnabled = true;
            cmbMonth.Location = new Point(842, 48);
            cmbMonth.Name = "cmbMonth";
            cmbMonth.Size = new Size(200, 38);
            cmbMonth.TabIndex = 9;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(842, 15);
            label4.Name = "label4";
            label4.Size = new Size(75, 30);
            label4.TabIndex = 8;
            label4.Text = "Month";
            label4.Click += label4_Click;
            // 
            // cmbYear
            // 
            cmbYear.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbYear.FormattingEnabled = true;
            cmbYear.Location = new Point(636, 48);
            cmbYear.Name = "cmbYear";
            cmbYear.Size = new Size(200, 38);
            cmbYear.TabIndex = 7;
            // 
            // Year
            // 
            Year.AutoSize = true;
            Year.Location = new Point(636, 15);
            Year.Name = "Year";
            Year.Size = new Size(52, 30);
            Year.TabIndex = 6;
            Year.Text = "Year";
            // 
            // cmbCustomerType
            // 
            cmbCustomerType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCustomerType.FormattingEnabled = true;
            cmbCustomerType.Location = new Point(430, 48);
            cmbCustomerType.Name = "cmbCustomerType";
            cmbCustomerType.Size = new Size(200, 38);
            cmbCustomerType.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(430, 15);
            label3.Name = "label3";
            label3.Size = new Size(151, 30);
            label3.TabIndex = 4;
            label3.Text = "Customer Type";
            // 
            // cmbCategory
            // 
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(220, 48);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(200, 38);
            cmbCategory.TabIndex = 3;
            cmbCategory.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(220, 15);
            label2.Name = "label2";
            label2.Size = new Size(96, 30);
            label2.TabIndex = 2;
            label2.Text = "Category";
            label2.Click += label2_Click;
            // 
            // cmbStore
            // 
            cmbStore.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStore.FormattingEnabled = true;
            cmbStore.Location = new Point(10, 48);
            cmbStore.Name = "cmbStore";
            cmbStore.Size = new Size(200, 38);
            cmbStore.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 15);
            label1.Name = "label1";
            label1.Size = new Size(60, 30);
            label1.TabIndex = 0;
            label1.Text = "Store";
            // 
            // dgvSales
            // 
            dgvSales.AllowUserToAddRows = false;
            dgvSales.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            dgvSales.BackgroundColor = SystemColors.Window;
            dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSales.Location = new Point(12, 120);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersWidth = 72;
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new Size(650, 1034);
            dgvSales.TabIndex = 2;
            // 
            // chartSalesByStore
            // 
            chartArea1.Name = "ChartArea1";
            chartSalesByStore.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            chartSalesByStore.Legends.Add(legend1);
            chartSalesByStore.Location = new Point(902, 118);
            chartSalesByStore.Name = "chartSalesByStore";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chartSalesByStore.Series.Add(series1);
            chartSalesByStore.Size = new Size(1011, 302);
            chartSalesByStore.TabIndex = 3;
            chartSalesByStore.Text = "chartSalesByStore";
            title1.Name = "Sales by Store";
            chartSalesByStore.Titles.Add(title1);
            // 
            // chartSalesByCategory
            // 
            chartArea2.Name = "ChartArea1";
            chartSalesByCategory.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            chartSalesByCategory.Legends.Add(legend2);
            chartSalesByCategory.Location = new Point(680, 434);
            chartSalesByCategory.Name = "chartSalesByCategory";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            chartSalesByCategory.Series.Add(series2);
            chartSalesByCategory.Size = new Size(727, 426);
            chartSalesByCategory.TabIndex = 4;
            chartSalesByCategory.Text = "chart2";
            title2.Name = "Sales by Category";
            chartSalesByCategory.Titles.Add(title2);
            // 
            // chartMonthlyTrend
            // 
            chartArea3.Name = "ChartArea1";
            chartMonthlyTrend.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            chartMonthlyTrend.Legends.Add(legend3);
            chartMonthlyTrend.Location = new Point(1430, 434);
            chartMonthlyTrend.Name = "chartMonthlyTrend";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series3.Name = "Series1";
            chartMonthlyTrend.Series.Add(series3);
            chartMonthlyTrend.Size = new Size(785, 703);
            chartMonthlyTrend.TabIndex = 5;
            chartMonthlyTrend.Text = "chart2";
            title3.Name = "Monthly Sales Trend";
            chartMonthlyTrend.Titles.Add(title3);
            // 
            // chartCustomerType
            // 
            chartArea4.Name = "ChartArea1";
            chartCustomerType.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            chartCustomerType.Legends.Add(legend4);
            chartCustomerType.Location = new Point(680, 877);
            chartCustomerType.Name = "chartCustomerType";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            chartCustomerType.Series.Add(series4);
            chartCustomerType.Size = new Size(727, 260);
            chartCustomerType.TabIndex = 6;
            chartCustomerType.Text = "Sales by Customer Type";
            title4.Name = "Sales by Category";
            chartCustomerType.Titles.Add(title4);
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(28, 28);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 1140);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(2227, 39);
            statusStrip1.TabIndex = 7;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(206, 30);
            lblStatus.Text = "toolStripStatusLabel1";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Size = new Size(68, 30);
            label5.TabIndex = 8;
            label5.Text = "label5";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2227, 1179);
            Controls.Add(label5);
            Controls.Add(statusStrip1);
            Controls.Add(chartCustomerType);
            Controls.Add(chartMonthlyTrend);
            Controls.Add(chartSalesByCategory);
            Controls.Add(chartSalesByStore);
            Controls.Add(dgvSales);
            Controls.Add(panelFilters);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            panelFilters.ResumeLayout(false);
            panelFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartSalesByStore).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartSalesByCategory).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartMonthlyTrend).EndInit();
            ((System.ComponentModel.ISupportInitialize)chartCustomerType).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        private void label4_Click(object sender, EventArgs e)
        {
            // Empty - no action needed
        }

        #endregion

        private TextBox textBox1;
        private Panel panelFilters;
        private ComboBox cmbStore;
        private Label label1;
        private ComboBox cmbCategory;
        private Label label2;
        private ComboBox cmbYear;
        private Label Year;
        private ComboBox cmbCustomerType;
        private Label label3;
        private ComboBox cmbMonth;
        private Label label4;
       // private Button button1;
        private Button btnReset;
        private Button btnFilter;
        protected Button btnExport;
        private DataGridView dgvSales;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSalesByStore;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSalesByCategory;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMonthlyTrend;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCustomerType;
        private StatusStrip statusStrip1;
        private Label label5;
        private ToolStripStatusLabel lblStatus;
    }
}
