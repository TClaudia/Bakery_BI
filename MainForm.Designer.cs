namespace BakeryBI
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.btnApplyFilters = new System.Windows.Forms.Button();
            this.cmbStore = new System.Windows.Forms.ComboBox();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblStore = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblDateFrom = new System.Windows.Forms.Label();
            this.lblDateTo = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSalesOverTime = new System.Windows.Forms.TabPage();
            this.splitContainerSales = new System.Windows.Forms.SplitContainer();
            this.chartSalesOverTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvSalesTimeData = new System.Windows.Forms.DataGridView();
            this.tabMaxMinProducts = new System.Windows.Forms.TabPage();
            this.panelMaxMin = new System.Windows.Forms.Panel();
            this.lblMinProduct = new System.Windows.Forms.Label();
            this.lblMaxProduct = new System.Windows.Forms.Label();
            this.splitContainerMaxMin = new System.Windows.Forms.SplitContainer();
            this.chartMaxMinProducts = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvProductSales = new System.Windows.Forms.DataGridView();
            this.panelFilters.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabSalesOverTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSales)).BeginInit();
            this.splitContainerSales.Panel1.SuspendLayout();
            this.splitContainerSales.Panel2.SuspendLayout();
            this.splitContainerSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSalesOverTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalesTimeData)).BeginInit();
            this.tabMaxMinProducts.SuspendLayout();
            this.panelMaxMin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaxMin)).BeginInit();
            this.splitContainerMaxMin.Panel1.SuspendLayout();
            this.splitContainerMaxMin.Panel2.SuspendLayout();
            this.splitContainerMaxMin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMaxMinProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSales)).BeginInit();
            this.SuspendLayout();
            // 
            // panelFilters
            // 
            this.panelFilters.BackColor = System.Drawing.Color.LightSteelBlue;
            this.panelFilters.Controls.Add(this.lblDateTo);
            this.panelFilters.Controls.Add(this.lblDateFrom);
            this.panelFilters.Controls.Add(this.dtpTo);
            this.panelFilters.Controls.Add(this.dtpFrom);
            this.panelFilters.Controls.Add(this.btnApplyFilters);
            this.panelFilters.Controls.Add(this.cmbStore);
            this.panelFilters.Controls.Add(this.cmbProduct);
            this.panelFilters.Controls.Add(this.lblProduct);
            this.panelFilters.Controls.Add(this.lblStore);
            this.panelFilters.Controls.Add(this.lblTitle);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 0);
            this.panelFilters.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Size = new System.Drawing.Size(2100, 180);
            this.panelFilters.TabIndex = 0;
            // 
            // btnApplyFilters
            // 
            this.btnApplyFilters.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnApplyFilters.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.btnApplyFilters.ForeColor = System.Drawing.Color.White;
            this.btnApplyFilters.Location = new System.Drawing.Point(700, 75);
            this.btnApplyFilters.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnApplyFilters.Name = "btnApplyFilters";
            this.btnApplyFilters.Size = new System.Drawing.Size(200, 50);
            this.btnApplyFilters.TabIndex = 5;
            this.btnApplyFilters.Text = "Apply Filters";
            this.btnApplyFilters.UseVisualStyleBackColor = false;
            this.btnApplyFilters.Click += new System.EventHandler(this.btnApplyFilters_Click);
            // 
            // cmbStore
            // 
            this.cmbStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStore.FormattingEnabled = true;
            this.cmbStore.Location = new System.Drawing.Point(100, 84);
            this.cmbStore.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.Size = new System.Drawing.Size(250, 38);
            this.cmbStore.TabIndex = 4;
            // 
            // cmbProduct
            // 
            this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(450, 84);
            this.cmbProduct.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(220, 38);
            this.cmbProduct.TabIndex = 3;
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(360, 87);
            this.lblProduct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(92, 30);
            this.lblProduct.TabIndex = 2;
            this.lblProduct.Text = "Product:";
            // 
            // lblStore
            // 
            this.lblStore.AutoSize = true;
            this.lblStore.Location = new System.Drawing.Point(15, 87);
            this.lblStore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStore.Name = "lblStore";
            this.lblStore.Size = new System.Drawing.Size(65, 30);
            this.lblStore.TabIndex = 1;
            this.lblStore.Text = "Store:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(15, 25);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(550, 38);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "BAKERY SALES ANALYSIS - Filters";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(100, 130);
            this.dtpFrom.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(200, 35);
            this.dtpFrom.TabIndex = 6;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(400, 130);
            this.dtpTo.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(200, 35);
            this.dtpTo.TabIndex = 7;
            // 
            // lblDateFrom
            // 
            this.lblDateFrom.AutoSize = true;
            this.lblDateFrom.Location = new System.Drawing.Point(15, 135);
            this.lblDateFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDateFrom.Name = "lblDateFrom";
            this.lblDateFrom.Size = new System.Drawing.Size(68, 30);
            this.lblDateFrom.TabIndex = 8;
            this.lblDateFrom.Text = "From:";
            // 
            // lblDateTo
            // 
            this.lblDateTo.AutoSize = true;
            this.lblDateTo.Location = new System.Drawing.Point(340, 135);
            this.lblDateTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDateTo.Name = "lblDateTo";
            this.lblDateTo.Size = new System.Drawing.Size(42, 30);
            this.lblDateTo.TabIndex = 9;
            this.lblDateTo.Text = "To:";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSalesOverTime);
            this.tabControl.Controls.Add(this.tabMaxMinProducts);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 180);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(2100, 1320);
            this.tabControl.TabIndex = 1;
            // 
            // tabSalesOverTime
            // 
            this.tabSalesOverTime.Controls.Add(this.splitContainerSales);
            this.tabSalesOverTime.Location = new System.Drawing.Point(4, 39);
            this.tabSalesOverTime.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tabSalesOverTime.Name = "tabSalesOverTime";
            this.tabSalesOverTime.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tabSalesOverTime.Size = new System.Drawing.Size(2092, 1277);
            this.tabSalesOverTime.TabIndex = 0;
            this.tabSalesOverTime.Text = "Sales Over Time";
            this.tabSalesOverTime.UseVisualStyleBackColor = true;
            // 
            // splitContainerSales
            // 
            this.splitContainerSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSales.Name = "splitContainerSales";
            // 
            // splitContainerSales.Panel1
            // 
            this.splitContainerSales.Panel1.Controls.Add(this.chartSalesOverTime);
            // 
            // splitContainerSales.Panel2
            // 
            this.splitContainerSales.Panel2.Controls.Add(this.dgvSalesTimeData);
            this.splitContainerSales.SplitterDistance = 1300;
            this.splitContainerSales.TabIndex = 0;
            // 
            // chartSalesOverTime
            // 
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.LabelStyle.Angle = -45;
            chartArea1.AxisX.Title = "Time Period (Month-Year)";
            chartArea1.AxisY.Title = "Sales Amount ($)";
            chartArea1.BackColor = System.Drawing.Color.Honeydew;
            chartArea1.Name = "ChartArea1";
            this.chartSalesOverTime.ChartAreas.Add(chartArea1);
            this.chartSalesOverTime.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chartSalesOverTime.Legends.Add(legend1);
            this.chartSalesOverTime.Location = new System.Drawing.Point(0, 0);
            this.chartSalesOverTime.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.chartSalesOverTime.Name = "chartSalesOverTime";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Green;
            series1.Legend = "Legend1";
            series1.MarkerColor = System.Drawing.Color.DarkGreen;
            series1.MarkerSize = 8;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Sales";
            this.chartSalesOverTime.Series.Add(series1);
            this.chartSalesOverTime.Size = new System.Drawing.Size(1300, 1265);
            this.chartSalesOverTime.TabIndex = 0;
            this.chartSalesOverTime.Text = "chart1";
            // 
            // dgvSalesTimeData
            // 
            this.dgvSalesTimeData.AllowUserToAddRows = false;
            this.dgvSalesTimeData.AllowUserToDeleteRows = false;
            this.dgvSalesTimeData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSalesTimeData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSalesTimeData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSalesTimeData.Location = new System.Drawing.Point(0, 0);
            this.dgvSalesTimeData.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dgvSalesTimeData.Name = "dgvSalesTimeData";
            this.dgvSalesTimeData.ReadOnly = true;
            this.dgvSalesTimeData.RowHeadersWidth = 51;
            this.dgvSalesTimeData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSalesTimeData.Size = new System.Drawing.Size(780, 1265);
            this.dgvSalesTimeData.TabIndex = 0;
            // 
            // tabMaxMinProducts
            // 
            this.tabMaxMinProducts.Controls.Add(this.panelMaxMin);
            this.tabMaxMinProducts.Controls.Add(this.splitContainerMaxMin);
            this.tabMaxMinProducts.Location = new System.Drawing.Point(4, 39);
            this.tabMaxMinProducts.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tabMaxMinProducts.Name = "tabMaxMinProducts";
            this.tabMaxMinProducts.Padding = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tabMaxMinProducts.Size = new System.Drawing.Size(2092, 1277);
            this.tabMaxMinProducts.TabIndex = 1;
            this.tabMaxMinProducts.Text = "Max/Min Products";
            this.tabMaxMinProducts.UseVisualStyleBackColor = true;
            // 
            // panelMaxMin
            // 
            this.panelMaxMin.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelMaxMin.Controls.Add(this.lblMinProduct);
            this.panelMaxMin.Controls.Add(this.lblMaxProduct);
            this.panelMaxMin.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMaxMin.Location = new System.Drawing.Point(4, 6);
            this.panelMaxMin.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.panelMaxMin.Name = "panelMaxMin";
            this.panelMaxMin.Size = new System.Drawing.Size(2084, 100);
            this.panelMaxMin.TabIndex = 1;
            // 
            // lblMinProduct
            // 
            this.lblMinProduct.AutoSize = true;
            this.lblMinProduct.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.lblMinProduct.ForeColor = System.Drawing.Color.Red;
            this.lblMinProduct.Location = new System.Drawing.Point(30, 55);
            this.lblMinProduct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMinProduct.Name = "lblMinProduct";
            this.lblMinProduct.Size = new System.Drawing.Size(90, 32);
            this.lblMinProduct.TabIndex = 1;
            this.lblMinProduct.Text = "MIN: -";
            // 
            // lblMaxProduct
            // 
            this.lblMaxProduct.AutoSize = true;
            this.lblMaxProduct.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.lblMaxProduct.ForeColor = System.Drawing.Color.Green;
            this.lblMaxProduct.Location = new System.Drawing.Point(30, 15);
            this.lblMaxProduct.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxProduct.Name = "lblMaxProduct";
            this.lblMaxProduct.Size = new System.Drawing.Size(100, 32);
            this.lblMaxProduct.TabIndex = 0;
            this.lblMaxProduct.Text = "MAX: -";
            // 
            // splitContainerMaxMin
            // 
            this.splitContainerMaxMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMaxMin.Name = "splitContainerMaxMin";
            // 
            // splitContainerMaxMin.Panel1
            // 
            this.splitContainerMaxMin.Panel1.Controls.Add(this.chartMaxMinProducts);
            // 
            // splitContainerMaxMin.Panel2
            // 
            this.splitContainerMaxMin.Panel2.Controls.Add(this.dgvProductSales);
            this.splitContainerMaxMin.SplitterDistance = 1300;
            this.splitContainerMaxMin.TabIndex = 2;
            // 
            // chartMaxMinProducts
            // 
            chartArea2.AxisX.Interval = 1D;
            chartArea2.AxisX.LabelStyle.Angle = -45;
            chartArea2.AxisX.Title = "Product";
            chartArea2.AxisY.Title = "Sales Amount ($)";
            chartArea2.BackColor = System.Drawing.Color.LightYellow;
            chartArea2.Name = "ChartArea1";
            this.chartMaxMinProducts.ChartAreas.Add(chartArea2);
            this.chartMaxMinProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Alignment = System.Drawing.StringAlignment.Center;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend2.Name = "Legend1";
            this.chartMaxMinProducts.Legends.Add(legend2);
            this.chartMaxMinProducts.Location = new System.Drawing.Point(0, 0);
            this.chartMaxMinProducts.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.chartMaxMinProducts.Name = "chartMaxMinProducts";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            series2.Legend = "Legend1";
            series2.Name = "Sales";
            this.chartMaxMinProducts.Series.Add(series2);
            this.chartMaxMinProducts.Size = new System.Drawing.Size(1300, 1165);
            this.chartMaxMinProducts.TabIndex = 0;
            this.chartMaxMinProducts.Text = "chart2";
            // 
            // dgvProductSales
            // 
            this.dgvProductSales.AllowUserToAddRows = false;
            this.dgvProductSales.AllowUserToDeleteRows = false;
            this.dgvProductSales.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProductSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProductSales.Location = new System.Drawing.Point(0, 0);
            this.dgvProductSales.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dgvProductSales.Name = "dgvProductSales";
            this.dgvProductSales.ReadOnly = true;
            this.dgvProductSales.RowHeadersWidth = 51;
            this.dgvProductSales.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductSales.Size = new System.Drawing.Size(780, 1165);
            this.dgvProductSales.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2100, 1500);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panelFilters);
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bakery BI - Sales Analysis";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabSalesOverTime.ResumeLayout(false);
            this.splitContainerSales.Panel1.ResumeLayout(false);
            this.splitContainerSales.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSales)).EndInit();
            this.splitContainerSales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSalesOverTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSalesTimeData)).EndInit();
            this.tabMaxMinProducts.ResumeLayout(false);
            this.panelMaxMin.ResumeLayout(false);
            this.panelMaxMin.PerformLayout();
            this.splitContainerMaxMin.Panel1.ResumeLayout(false);
            this.splitContainerMaxMin.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMaxMin)).EndInit();
            this.splitContainerMaxMin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMaxMinProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSales)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.Button btnApplyFilters;
        private System.Windows.Forms.ComboBox cmbStore;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblStore;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblDateFrom;
        private System.Windows.Forms.Label lblDateTo;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSalesOverTime;
        private System.Windows.Forms.SplitContainer splitContainerSales;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSalesOverTime;
        private System.Windows.Forms.DataGridView dgvSalesTimeData;
        private System.Windows.Forms.TabPage tabMaxMinProducts;
        private System.Windows.Forms.Panel panelMaxMin;
        private System.Windows.Forms.Label lblMinProduct;
        private System.Windows.Forms.Label lblMaxProduct;
        private System.Windows.Forms.SplitContainer splitContainerMaxMin;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMaxMinProducts;
        private System.Windows.Forms.DataGridView dgvProductSales;
    }
}