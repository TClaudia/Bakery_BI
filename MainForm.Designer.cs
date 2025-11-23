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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            panelFilters = new Panel();
            lblDateTo = new Label();
            lblDateFrom = new Label();
            dtpTo = new DateTimePicker();
            dtpFrom = new DateTimePicker();
            btnApplyFilters = new Button();
            cmbStore = new ComboBox();
            cmbProduct = new ComboBox();
            lblProduct = new Label();
            lblStore = new Label();
            lblTitle = new Label();
            tabControl = new TabControl();
            tabSalesOverTime = new TabPage();
            splitContainerSales = new SplitContainer();
            chartSalesOverTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            dgvSalesTimeData = new DataGridView();
            tabMaxMinProducts = new TabPage();
            splitContainerMaxMin = new SplitContainer();
            chartMaxMinProducts = new System.Windows.Forms.DataVisualization.Charting.Chart();
            dgvProductSales = new DataGridView();
            panelMaxMin = new Panel();
            lblMinProduct = new Label();
            lblMaxProduct = new Label();
            tabFutureSalesEstimation = new TabPage();
            splitContainerFutureSalesEstimation = new SplitContainer();
            chartFutureSalesEstimation = new System.Windows.Forms.DataVisualization.Charting.Chart();
            cmbForecastMonths = new ComboBox();
            dgvSalesData = new DataGridView();
            tabEvolutionOfProfits = new TabPage();
            splitContainerEvolutionOfProfit = new SplitContainer();
            pnlStoreFilters = new Panel();
            chartEvolutionOfProfits = new System.Windows.Forms.DataVisualization.Charting.Chart();
            pnlClientTypeFilters = new Panel();
            dgvProfitData = new DataGridView();
            panelFilters.SuspendLayout();
            tabControl.SuspendLayout();
            tabSalesOverTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerSales).BeginInit();
            splitContainerSales.Panel1.SuspendLayout();
            splitContainerSales.Panel2.SuspendLayout();
            splitContainerSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartSalesOverTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSalesTimeData).BeginInit();
            tabMaxMinProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMaxMin).BeginInit();
            splitContainerMaxMin.Panel1.SuspendLayout();
            splitContainerMaxMin.Panel2.SuspendLayout();
            splitContainerMaxMin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartMaxMinProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductSales).BeginInit();
            panelMaxMin.SuspendLayout();
            tabFutureSalesEstimation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerFutureSalesEstimation).BeginInit();
            splitContainerFutureSalesEstimation.Panel1.SuspendLayout();
            splitContainerFutureSalesEstimation.Panel2.SuspendLayout();
            splitContainerFutureSalesEstimation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartFutureSalesEstimation).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvSalesData).BeginInit();
            tabEvolutionOfProfits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerEvolutionOfProfit).BeginInit();
            splitContainerEvolutionOfProfit.Panel1.SuspendLayout();
            splitContainerEvolutionOfProfit.Panel2.SuspendLayout();
            splitContainerEvolutionOfProfit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartEvolutionOfProfits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProfitData).BeginInit();
            SuspendLayout();
            // 
            // panelFilters
            // 
            panelFilters.BackColor = Color.LightSteelBlue;
            panelFilters.Controls.Add(lblDateTo);
            panelFilters.Controls.Add(lblDateFrom);
            panelFilters.Controls.Add(dtpTo);
            panelFilters.Controls.Add(dtpFrom);
            panelFilters.Controls.Add(btnApplyFilters);
            panelFilters.Controls.Add(cmbStore);
            panelFilters.Controls.Add(cmbProduct);
            panelFilters.Controls.Add(lblProduct);
            panelFilters.Controls.Add(lblStore);
            panelFilters.Controls.Add(lblTitle);
            panelFilters.Dock = DockStyle.Top;
            panelFilters.Location = new Point(0, 0);
            panelFilters.Margin = new Padding(3, 6, 3, 6);
            panelFilters.Name = "panelFilters";
            panelFilters.Size = new Size(1923, 180);
            panelFilters.TabIndex = 0;
            // 
            // lblDateTo
            // 
            lblDateTo.AutoSize = true;
            lblDateTo.Location = new Point(339, 136);
            lblDateTo.Name = "lblDateTo";
            lblDateTo.Size = new Size(39, 30);
            lblDateTo.TabIndex = 9;
            lblDateTo.Text = "To:";
            // 
            // lblDateFrom
            // 
            lblDateFrom.AutoSize = true;
            lblDateFrom.Location = new Point(15, 136);
            lblDateFrom.Name = "lblDateFrom";
            lblDateFrom.Size = new Size(65, 30);
            lblDateFrom.TabIndex = 8;
            lblDateFrom.Text = "From:";
            // 
            // dtpTo
            // 
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(399, 130);
            dtpTo.Margin = new Padding(3, 6, 3, 6);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(199, 35);
            dtpTo.TabIndex = 7;
            // 
            // dtpFrom
            // 
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(99, 130);
            dtpFrom.Margin = new Padding(3, 6, 3, 6);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(199, 35);
            dtpFrom.TabIndex = 6;
            // 
            // btnApplyFilters
            // 
            btnApplyFilters.BackColor = Color.MediumSeaGreen;
            btnApplyFilters.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnApplyFilters.ForeColor = Color.White;
            btnApplyFilters.Location = new Point(699, 76);
            btnApplyFilters.Margin = new Padding(3, 6, 3, 6);
            btnApplyFilters.Name = "btnApplyFilters";
            btnApplyFilters.Size = new Size(201, 50);
            btnApplyFilters.TabIndex = 5;
            btnApplyFilters.Text = "Apply Filters";
            btnApplyFilters.UseVisualStyleBackColor = false;
            btnApplyFilters.Click += btnApplyFilters_Click;
            // 
            // cmbStore
            // 
            cmbStore.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStore.FormattingEnabled = true;
            cmbStore.Location = new Point(99, 84);
            cmbStore.Margin = new Padding(3, 6, 3, 6);
            cmbStore.Name = "cmbStore";
            cmbStore.Size = new Size(251, 38);
            cmbStore.TabIndex = 4;
            // 
            // cmbProduct
            // 
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.FormattingEnabled = true;
            cmbProduct.Location = new Point(449, 84);
            cmbProduct.Margin = new Padding(3, 6, 3, 6);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(220, 38);
            cmbProduct.TabIndex = 3;
            // 
            // lblProduct
            // 
            lblProduct.AutoSize = true;
            lblProduct.Location = new Point(360, 88);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(90, 30);
            lblProduct.TabIndex = 2;
            lblProduct.Text = "Product:";
            // 
            // lblStore
            // 
            lblStore.AutoSize = true;
            lblStore.Location = new Point(15, 88);
            lblStore.Name = "lblStore";
            lblStore.Size = new Size(65, 30);
            lblStore.TabIndex = 1;
            lblStore.Text = "Store:";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Arial", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(15, 24);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(442, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "BAKERY SALES ANALYSIS";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabSalesOverTime);
            tabControl.Controls.Add(tabMaxMinProducts);
            tabControl.Controls.Add(tabFutureSalesEstimation);
            tabControl.Controls.Add(tabEvolutionOfProfits);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 180);
            tabControl.Margin = new Padding(3, 6, 3, 6);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1923, 880);
            tabControl.TabIndex = 1;
            // 
            // tabSalesOverTime
            // 
            tabSalesOverTime.Controls.Add(splitContainerSales);
            tabSalesOverTime.Location = new Point(4, 39);
            tabSalesOverTime.Margin = new Padding(3, 6, 3, 6);
            tabSalesOverTime.Name = "tabSalesOverTime";
            tabSalesOverTime.Padding = new Padding(3, 6, 3, 6);
            tabSalesOverTime.Size = new Size(1915, 837);
            tabSalesOverTime.TabIndex = 0;
            tabSalesOverTime.Text = "Sales Over Time";
            tabSalesOverTime.UseVisualStyleBackColor = true;
            // 
            // splitContainerSales
            // 
            splitContainerSales.Dock = DockStyle.Fill;
            splitContainerSales.Location = new Point(3, 6);
            splitContainerSales.Margin = new Padding(3, 4, 3, 4);
            splitContainerSales.Name = "splitContainerSales";
            // 
            // splitContainerSales.Panel1
            // 
            splitContainerSales.Panel1.Controls.Add(chartSalesOverTime);
            // 
            // splitContainerSales.Panel2
            // 
            splitContainerSales.Panel2.Controls.Add(dgvSalesTimeData);
            splitContainerSales.Size = new Size(1909, 825);
            splitContainerSales.SplitterDistance = 1539;
            splitContainerSales.SplitterWidth = 3;
            splitContainerSales.TabIndex = 0;
            // 
            // chartSalesOverTime
            // 
            chartArea1.AxisX.Interval = 1D;
            chartArea1.AxisX.LabelStyle.Angle = -45;
            chartArea1.AxisX.Title = "Time Period (Month-Year)";
            chartArea1.AxisY.Title = "Sales Amount ($)";
            chartArea1.BackColor = Color.Honeydew;
            chartArea1.Name = "ChartArea1";
            chartSalesOverTime.ChartAreas.Add(chartArea1);
            chartSalesOverTime.Dock = DockStyle.Fill;
            legend1.Alignment = StringAlignment.Center;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            chartSalesOverTime.Legends.Add(legend1);
            chartSalesOverTime.Location = new Point(0, 0);
            chartSalesOverTime.Margin = new Padding(3, 6, 3, 6);
            chartSalesOverTime.Name = "chartSalesOverTime";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = Color.Green;
            series1.Legend = "Legend1";
            series1.MarkerColor = Color.DarkGreen;
            series1.MarkerSize = 8;
            series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series1.Name = "Sales";
            chartSalesOverTime.Series.Add(series1);
            chartSalesOverTime.Size = new Size(1539, 825);
            chartSalesOverTime.TabIndex = 0;
            chartSalesOverTime.Text = "chart1";
            // 
            // dgvSalesTimeData
            // 
            dgvSalesTimeData.AllowUserToAddRows = false;
            dgvSalesTimeData.AllowUserToDeleteRows = false;
            dgvSalesTimeData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvSalesTimeData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSalesTimeData.Dock = DockStyle.Fill;
            dgvSalesTimeData.Location = new Point(0, 0);
            dgvSalesTimeData.Margin = new Padding(3, 6, 3, 6);
            dgvSalesTimeData.Name = "dgvSalesTimeData";
            dgvSalesTimeData.ReadOnly = true;
            dgvSalesTimeData.RowHeadersWidth = 51;
            dgvSalesTimeData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSalesTimeData.Size = new Size(367, 825);
            dgvSalesTimeData.TabIndex = 0;
            // 
            // tabMaxMinProducts
            // 
            tabMaxMinProducts.Controls.Add(splitContainerMaxMin);
            tabMaxMinProducts.Controls.Add(panelMaxMin);
            tabMaxMinProducts.Location = new Point(4, 39);
            tabMaxMinProducts.Margin = new Padding(3, 6, 3, 6);
            tabMaxMinProducts.Name = "tabMaxMinProducts";
            tabMaxMinProducts.Padding = new Padding(3, 6, 3, 6);
            tabMaxMinProducts.Size = new Size(1915, 837);
            tabMaxMinProducts.TabIndex = 1;
            tabMaxMinProducts.Text = "Max/Min Products";
            tabMaxMinProducts.UseVisualStyleBackColor = true;
            // 
            // splitContainerMaxMin
            // 
            splitContainerMaxMin.Dock = DockStyle.Fill;
            splitContainerMaxMin.Location = new Point(3, 106);
            splitContainerMaxMin.Margin = new Padding(3, 4, 3, 4);
            splitContainerMaxMin.Name = "splitContainerMaxMin";
            // 
            // splitContainerMaxMin.Panel1
            // 
            splitContainerMaxMin.Panel1.Controls.Add(chartMaxMinProducts);
            // 
            // splitContainerMaxMin.Panel2
            // 
            splitContainerMaxMin.Panel2.Controls.Add(dgvProductSales);
            splitContainerMaxMin.Size = new Size(1909, 725);
            splitContainerMaxMin.SplitterDistance = 1535;
            splitContainerMaxMin.SplitterWidth = 3;
            splitContainerMaxMin.TabIndex = 2;
            // 
            // chartMaxMinProducts
            // 
            chartArea2.AxisX.Interval = 1D;
            chartArea2.AxisX.LabelStyle.Angle = -45;
            chartArea2.AxisX.Title = "Product";
            chartArea2.AxisY.Title = "Sales Amount ($)";
            chartArea2.BackColor = Color.LightYellow;
            chartArea2.Name = "ChartArea1";
            chartMaxMinProducts.ChartAreas.Add(chartArea2);
            chartMaxMinProducts.Dock = DockStyle.Fill;
            legend2.Alignment = StringAlignment.Center;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend2.Name = "Legend1";
            chartMaxMinProducts.Legends.Add(legend2);
            chartMaxMinProducts.Location = new Point(0, 0);
            chartMaxMinProducts.Margin = new Padding(3, 6, 3, 6);
            chartMaxMinProducts.Name = "chartMaxMinProducts";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Sales";
            chartMaxMinProducts.Series.Add(series2);
            chartMaxMinProducts.Size = new Size(1535, 725);
            chartMaxMinProducts.TabIndex = 0;
            chartMaxMinProducts.Text = "chart2";
            // 
            // dgvProductSales
            // 
            dgvProductSales.AllowUserToAddRows = false;
            dgvProductSales.AllowUserToDeleteRows = false;
            dgvProductSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductSales.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProductSales.Dock = DockStyle.Fill;
            dgvProductSales.Location = new Point(0, 0);
            dgvProductSales.Margin = new Padding(3, 6, 3, 6);
            dgvProductSales.Name = "dgvProductSales";
            dgvProductSales.ReadOnly = true;
            dgvProductSales.RowHeadersWidth = 51;
            dgvProductSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductSales.Size = new Size(371, 725);
            dgvProductSales.TabIndex = 0;
            // 
            // panelMaxMin
            // 
            panelMaxMin.BackColor = Color.WhiteSmoke;
            panelMaxMin.Controls.Add(lblMinProduct);
            panelMaxMin.Controls.Add(lblMaxProduct);
            panelMaxMin.Dock = DockStyle.Top;
            panelMaxMin.Location = new Point(3, 6);
            panelMaxMin.Margin = new Padding(3, 6, 3, 6);
            panelMaxMin.Name = "panelMaxMin";
            panelMaxMin.Size = new Size(1909, 100);
            panelMaxMin.TabIndex = 1;
            // 
            // lblMinProduct
            // 
            lblMinProduct.AutoSize = true;
            lblMinProduct.Font = new Font("Arial", 11F, FontStyle.Bold);
            lblMinProduct.ForeColor = Color.Red;
            lblMinProduct.Location = new Point(31, 56);
            lblMinProduct.Name = "lblMinProduct";
            lblMinProduct.Size = new Size(89, 30);
            lblMinProduct.TabIndex = 1;
            lblMinProduct.Text = "MIN: -";
            // 
            // lblMaxProduct
            // 
            lblMaxProduct.AutoSize = true;
            lblMaxProduct.Font = new Font("Arial", 11F, FontStyle.Bold);
            lblMaxProduct.ForeColor = Color.Green;
            lblMaxProduct.Location = new Point(31, 16);
            lblMaxProduct.Name = "lblMaxProduct";
            lblMaxProduct.Size = new Size(98, 30);
            lblMaxProduct.TabIndex = 0;
            lblMaxProduct.Text = "MAX: -";
            // 
            // tabFutureSalesEstimation
            // 
            tabFutureSalesEstimation.Controls.Add(splitContainerFutureSalesEstimation);
            tabFutureSalesEstimation.Location = new Point(4, 39);
            tabFutureSalesEstimation.Margin = new Padding(5, 6, 5, 6);
            tabFutureSalesEstimation.Name = "tabFutureSalesEstimation";
            tabFutureSalesEstimation.Padding = new Padding(5, 6, 5, 6);
            tabFutureSalesEstimation.Size = new Size(1915, 837);
            tabFutureSalesEstimation.TabIndex = 2;
            tabFutureSalesEstimation.Text = "Future Sales Estimation";
            tabFutureSalesEstimation.UseVisualStyleBackColor = true;
            // 
            // splitContainerFutureSalesEstimation
            // 
            splitContainerFutureSalesEstimation.Dock = DockStyle.Fill;
            splitContainerFutureSalesEstimation.Location = new Point(5, 6);
            splitContainerFutureSalesEstimation.Margin = new Padding(5, 6, 5, 6);
            splitContainerFutureSalesEstimation.Name = "splitContainerFutureSalesEstimation";
            // 
            // splitContainerFutureSalesEstimation.Panel1
            // 
            splitContainerFutureSalesEstimation.Panel1.Controls.Add(chartFutureSalesEstimation);
            splitContainerFutureSalesEstimation.Panel1.Controls.Add(cmbForecastMonths);
            // 
            // splitContainerFutureSalesEstimation.Panel2
            // 
            splitContainerFutureSalesEstimation.Panel2.Controls.Add(dgvSalesData);
            splitContainerFutureSalesEstimation.Size = new Size(1905, 825);
            splitContainerFutureSalesEstimation.SplitterDistance = 1289;
            splitContainerFutureSalesEstimation.SplitterWidth = 7;
            splitContainerFutureSalesEstimation.TabIndex = 0;
            // 
            // chartFutureSalesEstimation
            // 
            chartArea3.Name = "ChartArea1";
            chartFutureSalesEstimation.ChartAreas.Add(chartArea3);
            chartFutureSalesEstimation.Dock = DockStyle.Fill;
            legend3.Name = "Legend1";
            chartFutureSalesEstimation.Legends.Add(legend3);
            chartFutureSalesEstimation.Location = new Point(0, 38);
            chartFutureSalesEstimation.Margin = new Padding(5, 6, 5, 6);
            chartFutureSalesEstimation.Name = "chartFutureSalesEstimation";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            chartFutureSalesEstimation.Series.Add(series3);
            chartFutureSalesEstimation.Size = new Size(1289, 787);
            chartFutureSalesEstimation.TabIndex = 1;
            chartFutureSalesEstimation.Text = "chart1";
            // 
            // cmbForecastMonths
            // 
            cmbForecastMonths.Dock = DockStyle.Top;
            cmbForecastMonths.FormattingEnabled = true;
            cmbForecastMonths.Location = new Point(0, 0);
            cmbForecastMonths.Margin = new Padding(5, 6, 5, 6);
            cmbForecastMonths.Name = "cmbForecastMonths";
            cmbForecastMonths.Size = new Size(1289, 38);
            cmbForecastMonths.TabIndex = 0;
            // 
            // dgvSalesData
            // 
            dgvSalesData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSalesData.Dock = DockStyle.Fill;
            dgvSalesData.Location = new Point(0, 0);
            dgvSalesData.Margin = new Padding(5, 6, 5, 6);
            dgvSalesData.Name = "dgvSalesData";
            dgvSalesData.RowHeadersWidth = 72;
            dgvSalesData.Size = new Size(609, 825);
            dgvSalesData.TabIndex = 0;
            // 
            // tabEvolutionOfProfits
            // 
            tabEvolutionOfProfits.Controls.Add(splitContainerEvolutionOfProfit);
            tabEvolutionOfProfits.Location = new Point(4, 39);
            tabEvolutionOfProfits.Margin = new Padding(5, 6, 5, 6);
            tabEvolutionOfProfits.Name = "tabEvolutionOfProfits";
            tabEvolutionOfProfits.Padding = new Padding(5, 6, 5, 6);
            tabEvolutionOfProfits.Size = new Size(1915, 837);
            tabEvolutionOfProfits.TabIndex = 3;
            tabEvolutionOfProfits.Text = "Evolution of Profits";
            tabEvolutionOfProfits.UseVisualStyleBackColor = true;
            // 
            // splitContainerEvolutionOfProfit
            // 
            splitContainerEvolutionOfProfit.Dock = DockStyle.Fill;
            splitContainerEvolutionOfProfit.Location = new Point(5, 6);
            splitContainerEvolutionOfProfit.Margin = new Padding(5, 6, 5, 6);
            splitContainerEvolutionOfProfit.Name = "splitContainerEvolutionOfProfit";
            // 
            // splitContainerEvolutionOfProfit.Panel1
            // 
            splitContainerEvolutionOfProfit.Panel1.Controls.Add(pnlStoreFilters);
            splitContainerEvolutionOfProfit.Panel1.Controls.Add(chartEvolutionOfProfits);
            splitContainerEvolutionOfProfit.Panel1.Controls.Add(pnlClientTypeFilters);
            // 
            // splitContainerEvolutionOfProfit.Panel2
            // 
            splitContainerEvolutionOfProfit.Panel2.Controls.Add(dgvProfitData);
            splitContainerEvolutionOfProfit.Size = new Size(1905, 825);
            splitContainerEvolutionOfProfit.SplitterDistance = 1289;
            splitContainerEvolutionOfProfit.SplitterWidth = 7;
            splitContainerEvolutionOfProfit.TabIndex = 0;
            // 
            // pnlStoreFilters
            // 
            pnlStoreFilters.Dock = DockStyle.Top;
            pnlStoreFilters.Location = new Point(0, 30);
            pnlStoreFilters.Name = "pnlStoreFilters";
            pnlStoreFilters.Size = new Size(750, 100);
            pnlStoreFilters.TabIndex = 2;
            // 
            // chartEvolutionOfProfits
            // 
            chartArea4.Name = "ChartArea1";
            chartEvolutionOfProfits.ChartAreas.Add(chartArea4);
            chartEvolutionOfProfits.Dock = DockStyle.Fill;
            legend4.Name = "Legend1";
            chartEvolutionOfProfits.Legends.Add(legend4);
            chartEvolutionOfProfits.Location = new Point(0, 100);
            chartEvolutionOfProfits.Margin = new Padding(5, 6, 5, 6);
            chartEvolutionOfProfits.Name = "chartEvolutionOfProfits";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            chartEvolutionOfProfits.Series.Add(series4);
            chartEvolutionOfProfits.Size = new Size(1289, 725);
            chartEvolutionOfProfits.TabIndex = 1;
            chartEvolutionOfProfits.Text = "chart1";
            // 
            // pnlClientTypeFilters
            // 
            pnlClientTypeFilters.Dock = DockStyle.Top;
            pnlClientTypeFilters.Location = new Point(0, 0);
            pnlClientTypeFilters.Margin = new Padding(5, 6, 5, 6);
            pnlClientTypeFilters.Name = "pnlClientTypeFilters";
            pnlClientTypeFilters.Size = new Size(1289, 100);
            pnlClientTypeFilters.TabIndex = 0;
            // 
            // dgvProfitData
            // 
            dgvProfitData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProfitData.Dock = DockStyle.Fill;
            dgvProfitData.Location = new Point(0, 0);
            dgvProfitData.Margin = new Padding(5, 6, 5, 6);
            dgvProfitData.Name = "dgvProfitData";
            dgvProfitData.RowHeadersWidth = 72;
            dgvProfitData.Size = new Size(609, 825);
            dgvProfitData.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1923, 1060);
            Controls.Add(tabControl);
            Controls.Add(panelFilters);
            Margin = new Padding(3, 6, 3, 6);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bakery BI - Sales Analysis";
            Load += MainForm_Load;
            panelFilters.ResumeLayout(false);
            panelFilters.PerformLayout();
            tabControl.ResumeLayout(false);
            tabSalesOverTime.ResumeLayout(false);
            splitContainerSales.Panel1.ResumeLayout(false);
            splitContainerSales.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerSales).EndInit();
            splitContainerSales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartSalesOverTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSalesTimeData).EndInit();
            tabMaxMinProducts.ResumeLayout(false);
            splitContainerMaxMin.Panel1.ResumeLayout(false);
            splitContainerMaxMin.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMaxMin).EndInit();
            splitContainerMaxMin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartMaxMinProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductSales).EndInit();
            panelMaxMin.ResumeLayout(false);
            panelMaxMin.PerformLayout();
            tabFutureSalesEstimation.ResumeLayout(false);
            splitContainerFutureSalesEstimation.Panel1.ResumeLayout(false);
            splitContainerFutureSalesEstimation.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerFutureSalesEstimation).EndInit();
            splitContainerFutureSalesEstimation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartFutureSalesEstimation).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvSalesData).EndInit();
            tabEvolutionOfProfits.ResumeLayout(false);
            splitContainerEvolutionOfProfit.Panel1.ResumeLayout(false);
            splitContainerEvolutionOfProfit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerEvolutionOfProfit).EndInit();
            splitContainerEvolutionOfProfit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartEvolutionOfProfits).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProfitData).EndInit();
            ResumeLayout(false);
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
        private TabPage tabFutureSalesEstimation;
        private SplitContainer splitContainerFutureSalesEstimation;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartFutureSalesEstimation;
        private ComboBox cmbForecastMonths;
        private DataGridView dgvSalesData;
        private TabPage tabEvolutionOfProfits;
        private SplitContainer splitContainerEvolutionOfProfit;
        private Panel pnlClientTypeFilters;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartEvolutionOfProfits;
        private DataGridView dgvProfitData;
        private Panel pnlStoreFilters;
    }
}