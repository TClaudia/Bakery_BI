namespace BakeryBI
{
    partial class MainForm
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
            panelFilters = new Panel();
            btnReset = new Button();
            btnFilter = new Button();
            dtpEndDate = new DateTimePicker();
            dtpStartDate = new DateTimePicker();
            cboCustomerType = new ComboBox();
            cboProduct = new ComboBox();
            cboStore = new ComboBox();
            lblEndDate = new Label();
            lblStartDate = new Label();
            lblCustomer = new Label();
            lblProduct = new Label();
            lblStore = new Label();
            lblTitle = new Label();
            tabControl = new TabControl();
            tabData = new TabPage();
            dgvSales = new DataGridView();
            tabSalesCosts = new TabPage();
            chartSalesCosts = new System.Windows.Forms.DataVisualization.Charting.Chart();
            panelMaxMin = new Panel();
            lblMinProduct = new Label();
            lblMaxProduct = new Label();
            tabProfit = new TabPage();
            chartProfitEvolution = new System.Windows.Forms.DataVisualization.Charting.Chart();
            panelForecast = new Panel();
            lblForecast = new Label();
            panelFilters.SuspendLayout();
            tabControl.SuspendLayout();
            tabData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSales).BeginInit();
            tabSalesCosts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartSalesCosts).BeginInit();
            panelMaxMin.SuspendLayout();
            tabProfit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartProfitEvolution).BeginInit();
            panelForecast.SuspendLayout();
            SuspendLayout();
            // 
            // panelFilters
            // 
            panelFilters.BackColor = Color.LightGray;
            panelFilters.BorderStyle = BorderStyle.FixedSingle;
            panelFilters.Controls.Add(btnReset);
            panelFilters.Controls.Add(btnFilter);
            panelFilters.Controls.Add(dtpEndDate);
            panelFilters.Controls.Add(dtpStartDate);
            panelFilters.Controls.Add(cboCustomerType);
            panelFilters.Controls.Add(cboProduct);
            panelFilters.Controls.Add(cboStore);
            panelFilters.Controls.Add(lblEndDate);
            panelFilters.Controls.Add(lblStartDate);
            panelFilters.Controls.Add(lblCustomer);
            panelFilters.Controls.Add(lblProduct);
            panelFilters.Controls.Add(lblStore);
            panelFilters.Controls.Add(lblTitle);
            panelFilters.Dock = DockStyle.Top;
            panelFilters.Location = new Point(0, 0);
            panelFilters.Margin = new Padding(4, 6, 4, 6);
            panelFilters.Name = "panelFilters";
            panelFilters.Size = new Size(2100, 223);
            panelFilters.TabIndex = 0;
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.Gray;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(960, 131);
            btnReset.Margin = new Padding(4, 6, 4, 6);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(150, 56);
            btnReset.TabIndex = 12;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btnReset_Click;
            // 
            // btnFilter
            // 
            btnFilter.BackColor = Color.DodgerBlue;
            btnFilter.FlatStyle = FlatStyle.Flat;
            btnFilter.ForeColor = Color.White;
            btnFilter.Location = new Point(795, 131);
            btnFilter.Margin = new Padding(4, 6, 4, 6);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(150, 56);
            btnFilter.TabIndex = 11;
            btnFilter.Text = "Apply Filter";
            btnFilter.UseVisualStyleBackColor = false;
            btnFilter.Click += btnFilter_Click;
            // 
            // dtpEndDate
            // 
            dtpEndDate.Format = DateTimePickerFormat.Short;
            dtpEndDate.Location = new Point(540, 135);
            dtpEndDate.Margin = new Padding(4, 6, 4, 6);
            dtpEndDate.Name = "dtpEndDate";
            dtpEndDate.Size = new Size(223, 35);
            dtpEndDate.TabIndex = 10;
            // 
            // dtpStartDate
            // 
            dtpStartDate.Format = DateTimePickerFormat.Short;
            dtpStartDate.Location = new Point(150, 135);
            dtpStartDate.Margin = new Padding(4, 6, 4, 6);
            dtpStartDate.Name = "dtpStartDate";
            dtpStartDate.Size = new Size(223, 35);
            dtpStartDate.TabIndex = 9;
            // 
            // cboCustomerType
            // 
            cboCustomerType.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCustomerType.FormattingEnabled = true;
            cboCustomerType.Location = new Point(930, 79);
            cboCustomerType.Margin = new Padding(4, 6, 4, 6);
            cboCustomerType.Name = "cboCustomerType";
            cboCustomerType.Size = new Size(178, 38);
            cboCustomerType.TabIndex = 8;
            // 
            // cboProduct
            // 
            cboProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProduct.FormattingEnabled = true;
            cboProduct.Location = new Point(540, 79);
            cboProduct.Margin = new Padding(4, 6, 4, 6);
            cboProduct.Name = "cboProduct";
            cboProduct.Size = new Size(223, 38);
            cboProduct.TabIndex = 7;
            // 
            // cboStore
            // 
            cboStore.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStore.FormattingEnabled = true;
            cboStore.Location = new Point(150, 79);
            cboStore.Margin = new Padding(4, 6, 4, 6);
            cboStore.Name = "cboStore";
            cboStore.Size = new Size(223, 38);
            cboStore.TabIndex = 6;
            // 
            // lblEndDate
            // 
            lblEndDate.AutoSize = true;
            lblEndDate.Location = new Point(405, 141);
            lblEndDate.Margin = new Padding(4, 0, 4, 0);
            lblEndDate.Name = "lblEndDate";
            lblEndDate.Size = new Size(39, 30);
            lblEndDate.TabIndex = 5;
            lblEndDate.Text = "To:";
            // 
            // lblStartDate
            // 
            lblStartDate.AutoSize = true;
            lblStartDate.Location = new Point(15, 141);
            lblStartDate.Margin = new Padding(4, 0, 4, 0);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.Size = new Size(65, 30);
            lblStartDate.TabIndex = 4;
            lblStartDate.Text = "From:";
            // 
            // lblCustomer
            // 
            lblCustomer.AutoSize = true;
            lblCustomer.Location = new Point(795, 84);
            lblCustomer.Margin = new Padding(4, 0, 4, 0);
            lblCustomer.Name = "lblCustomer";
            lblCustomer.Size = new Size(107, 30);
            lblCustomer.TabIndex = 3;
            lblCustomer.Text = "Customer:";
            // 
            // lblProduct
            // 
            lblProduct.AutoSize = true;
            lblProduct.Location = new Point(405, 84);
            lblProduct.Margin = new Padding(4, 0, 4, 0);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(90, 30);
            lblProduct.TabIndex = 2;
            lblProduct.Text = "Product:";
            // 
            // lblStore
            // 
            lblStore.AutoSize = true;
            lblStore.Location = new Point(15, 84);
            lblStore.Margin = new Padding(4, 0, 4, 0);
            lblStore.Name = "lblStore";
            lblStore.Size = new Size(65, 30);
            lblStore.TabIndex = 1;
            lblStore.Text = "Store:";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(15, 19);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(481, 33);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "BAKERY SALES ANALYSIS - Filters";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabData);
            tabControl.Controls.Add(tabSalesCosts);
            tabControl.Controls.Add(tabProfit);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 223);
            tabControl.Margin = new Padding(4, 6, 4, 6);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(2100, 1277);
            tabControl.TabIndex = 1;
            // 
            // tabData
            // 
            tabData.Controls.Add(dgvSales);
            tabData.Location = new Point(4, 39);
            tabData.Margin = new Padding(4, 6, 4, 6);
            tabData.Name = "tabData";
            tabData.Padding = new Padding(4, 6, 4, 6);
            tabData.Size = new Size(2092, 1234);
            tabData.TabIndex = 0;
            tabData.Text = "Sales Data";
            tabData.UseVisualStyleBackColor = true;
            // 
            // dgvSales
            // 
            dgvSales.AllowUserToAddRows = false;
            dgvSales.AllowUserToDeleteRows = false;
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSales.Dock = DockStyle.Fill;
            dgvSales.Location = new Point(4, 6);
            dgvSales.Margin = new Padding(4, 6, 4, 6);
            dgvSales.Name = "dgvSales";
            dgvSales.ReadOnly = true;
            dgvSales.RowHeadersWidth = 51;
            dgvSales.RowTemplate.Height = 24;
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.Size = new Size(2084, 1222);
            dgvSales.TabIndex = 0;
            // 
            // tabSalesCosts
            // 
            tabSalesCosts.Controls.Add(chartSalesCosts);
            tabSalesCosts.Controls.Add(panelMaxMin);
            tabSalesCosts.Location = new Point(4, 39);
            tabSalesCosts.Margin = new Padding(4, 6, 4, 6);
            tabSalesCosts.Name = "tabSalesCosts";
            tabSalesCosts.Padding = new Padding(4, 6, 4, 6);
            tabSalesCosts.Size = new Size(2092, 1234);
            tabSalesCosts.TabIndex = 1;
            tabSalesCosts.Text = "Sales & Costs Analysis";
            tabSalesCosts.UseVisualStyleBackColor = true;
            // 
            // chartSalesCosts
            // 
            chartSalesCosts.Dock = DockStyle.Fill;
            chartSalesCosts.Location = new Point(4, 156);
            chartSalesCosts.Margin = new Padding(4, 6, 4, 6);
            chartSalesCosts.Name = "chartSalesCosts";
            chartSalesCosts.Size = new Size(2084, 1072);
            chartSalesCosts.TabIndex = 0;
            chartSalesCosts.Text = "chart1";
            // 
            // panelMaxMin
            // 
            panelMaxMin.BackColor = Color.WhiteSmoke;
            panelMaxMin.Controls.Add(lblMinProduct);
            panelMaxMin.Controls.Add(lblMaxProduct);
            panelMaxMin.Dock = DockStyle.Top;
            panelMaxMin.Location = new Point(4, 6);
            panelMaxMin.Margin = new Padding(4, 6, 4, 6);
            panelMaxMin.Name = "panelMaxMin";
            panelMaxMin.Size = new Size(2084, 150);
            panelMaxMin.TabIndex = 1;
            // 
            // lblMinProduct
            // 
            lblMinProduct.AutoSize = true;
            lblMinProduct.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMinProduct.ForeColor = Color.Red;
            lblMinProduct.Location = new Point(30, 75);
            lblMinProduct.Margin = new Padding(4, 0, 4, 0);
            lblMinProduct.Name = "lblMinProduct";
            lblMinProduct.Size = new Size(80, 29);
            lblMinProduct.TabIndex = 1;
            lblMinProduct.Text = "MIN: -";
            // 
            // lblMaxProduct
            // 
            lblMaxProduct.AutoSize = true;
            lblMaxProduct.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMaxProduct.ForeColor = Color.Green;
            lblMaxProduct.Location = new Point(30, 19);
            lblMaxProduct.Margin = new Padding(4, 0, 4, 0);
            lblMaxProduct.Name = "lblMaxProduct";
            lblMaxProduct.Size = new Size(89, 29);
            lblMaxProduct.TabIndex = 0;
            lblMaxProduct.Text = "MAX: -";
            // 
            // tabProfit
            // 
            tabProfit.Controls.Add(chartProfitEvolution);
            tabProfit.Controls.Add(panelForecast);
            tabProfit.Location = new Point(4, 39);
            tabProfit.Margin = new Padding(4, 6, 4, 6);
            tabProfit.Name = "tabProfit";
            tabProfit.Size = new Size(2092, 1234);
            tabProfit.TabIndex = 2;
            tabProfit.Text = "Profit Evolution";
            tabProfit.UseVisualStyleBackColor = true;
            // 
            // chartProfitEvolution
            // 
            chartProfitEvolution.Dock = DockStyle.Fill;
            chartProfitEvolution.Location = new Point(0, 112);
            chartProfitEvolution.Margin = new Padding(4, 6, 4, 6);
            chartProfitEvolution.Name = "chartProfitEvolution";
            chartProfitEvolution.Size = new Size(2092, 1122);
            chartProfitEvolution.TabIndex = 0;
            chartProfitEvolution.Text = "chart2";
            // 
            // panelForecast
            // 
            panelForecast.BackColor = Color.LightYellow;
            panelForecast.Controls.Add(lblForecast);
            panelForecast.Dock = DockStyle.Top;
            panelForecast.Location = new Point(0, 0);
            panelForecast.Margin = new Padding(4, 6, 4, 6);
            panelForecast.Name = "panelForecast";
            panelForecast.Size = new Size(2092, 112);
            panelForecast.TabIndex = 1;
            // 
            // lblForecast
            // 
            lblForecast.AutoSize = true;
            lblForecast.Font = new Font("Arial", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblForecast.ForeColor = Color.DarkBlue;
            lblForecast.Location = new Point(30, 28);
            lblForecast.Margin = new Padding(4, 0, 4, 0);
            lblForecast.Name = "lblForecast";
            lblForecast.Size = new Size(134, 29);
            lblForecast.TabIndex = 0;
            lblForecast.Text = "Forecast: -";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2100, 1500);
            Controls.Add(tabControl);
            Controls.Add(panelFilters);
            Margin = new Padding(4, 6, 4, 6);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bakery BI - Decision Support System";
            Load += MainForm_Load;
            panelFilters.ResumeLayout(false);
            panelFilters.PerformLayout();
            tabControl.ResumeLayout(false);
            tabData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvSales).EndInit();
            tabSalesCosts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartSalesCosts).EndInit();
            panelMaxMin.ResumeLayout(false);
            panelMaxMin.PerformLayout();
            tabProfit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartProfitEvolution).EndInit();
            panelForecast.ResumeLayout(false);
            panelForecast.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblStore;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.ComboBox cboStore;
        private System.Windows.Forms.ComboBox cboProduct;
        private System.Windows.Forms.ComboBox cboCustomerType;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabData;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.TabPage tabSalesCosts;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSalesCosts;
        private System.Windows.Forms.Panel panelMaxMin;
        private System.Windows.Forms.Label lblMaxProduct;
        private System.Windows.Forms.Label lblMinProduct;
        private System.Windows.Forms.TabPage tabProfit;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartProfitEvolution;
        private System.Windows.Forms.Panel panelForecast;
        private System.Windows.Forms.Label lblForecast;
    }
}