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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
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
            panelMaxMin = new Panel();
            lblMinProduct = new Label();
            lblMaxProduct = new Label();
            splitContainerMaxMin = new SplitContainer();
            chartMaxMinProducts = new System.Windows.Forms.DataVisualization.Charting.Chart();
            dgvProductSales = new DataGridView();
            tabFutureSalesEstimation = new TabPage();
            splitContainerFutureSalesEstimation = new SplitContainer();
            chartFutureSalesEstimation = new System.Windows.Forms.DataVisualization.Charting.Chart();
            cmbForecastMonths = new ComboBox();
            dgvSalesData = new DataGridView();
            tabEvolutionOfProfits = new TabPage();
            splitContainerEvolutionOfProfit = new SplitContainer();
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
            panelMaxMin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMaxMin).BeginInit();
            splitContainerMaxMin.Panel1.SuspendLayout();
            splitContainerMaxMin.Panel2.SuspendLayout();
            splitContainerMaxMin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartMaxMinProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductSales).BeginInit();
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
            panelFilters.Margin = new Padding(2, 3, 2, 3);
            panelFilters.Name = "panelFilters";
            panelFilters.Size = new Size(1122, 90);
            panelFilters.TabIndex = 0;
            // 
            // lblDateTo
            // 
            lblDateTo.AutoSize = true;
            lblDateTo.Location = new Point(198, 68);
            lblDateTo.Margin = new Padding(2, 0, 2, 0);
            lblDateTo.Name = "lblDateTo";
            lblDateTo.Size = new Size(23, 15);
            lblDateTo.TabIndex = 9;
            lblDateTo.Text = "To:";
            // 
            // lblDateFrom
            // 
            lblDateFrom.AutoSize = true;
            lblDateFrom.Location = new Point(9, 68);
            lblDateFrom.Margin = new Padding(2, 0, 2, 0);
            lblDateFrom.Name = "lblDateFrom";
            lblDateFrom.Size = new Size(38, 15);
            lblDateFrom.TabIndex = 8;
            lblDateFrom.Text = "From:";
            // 
            // dtpTo
            // 
            dtpTo.Format = DateTimePickerFormat.Short;
            dtpTo.Location = new Point(233, 65);
            dtpTo.Margin = new Padding(2, 3, 2, 3);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(118, 23);
            dtpTo.TabIndex = 7;
            // 
            // dtpFrom
            // 
            dtpFrom.Format = DateTimePickerFormat.Short;
            dtpFrom.Location = new Point(58, 65);
            dtpFrom.Margin = new Padding(2, 3, 2, 3);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(118, 23);
            dtpFrom.TabIndex = 6;
            // 
            // btnApplyFilters
            // 
            btnApplyFilters.BackColor = Color.MediumSeaGreen;
            btnApplyFilters.Font = new Font("Arial", 10F, FontStyle.Bold);
            btnApplyFilters.ForeColor = Color.White;
            btnApplyFilters.Location = new Point(408, 38);
            btnApplyFilters.Margin = new Padding(2, 3, 2, 3);
            btnApplyFilters.Name = "btnApplyFilters";
            btnApplyFilters.Size = new Size(117, 25);
            btnApplyFilters.TabIndex = 5;
            btnApplyFilters.Text = "Apply Filters";
            btnApplyFilters.UseVisualStyleBackColor = false;
            btnApplyFilters.Click += btnApplyFilters_Click;
            // 
            // cmbStore
            // 
            cmbStore.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStore.FormattingEnabled = true;
            cmbStore.Location = new Point(58, 42);
            cmbStore.Margin = new Padding(2, 3, 2, 3);
            cmbStore.Name = "cmbStore";
            cmbStore.Size = new Size(148, 23);
            cmbStore.TabIndex = 4;
            // 
            // cmbProduct
            // 
            cmbProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProduct.FormattingEnabled = true;
            cmbProduct.Location = new Point(262, 42);
            cmbProduct.Margin = new Padding(2, 3, 2, 3);
            cmbProduct.Name = "cmbProduct";
            cmbProduct.Size = new Size(130, 23);
            cmbProduct.TabIndex = 3;
            // 
            // lblProduct
            // 
            lblProduct.AutoSize = true;
            lblProduct.Location = new Point(210, 44);
            lblProduct.Margin = new Padding(2, 0, 2, 0);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(52, 15);
            lblProduct.TabIndex = 2;
            lblProduct.Text = "Product:";
            // 
            // lblStore
            // 
            lblStore.AutoSize = true;
            lblStore.Location = new Point(9, 44);
            lblStore.Margin = new Padding(2, 0, 2, 0);
            lblStore.Name = "lblStore";
            lblStore.Size = new Size(37, 15);
            lblStore.TabIndex = 1;
            lblStore.Text = "Store:";
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Arial", 14F, FontStyle.Bold);
            lblTitle.Location = new Point(9, 12);
            lblTitle.Margin = new Padding(2, 0, 2, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(329, 22);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "BAKERY SALES ANALYSIS - Filters";
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabSalesOverTime);
            tabControl.Controls.Add(tabMaxMinProducts);
            tabControl.Controls.Add(tabFutureSalesEstimation);
            tabControl.Controls.Add(tabEvolutionOfProfits);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 90);
            tabControl.Margin = new Padding(2, 3, 2, 3);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1122, 440);
            tabControl.TabIndex = 1;
            // 
            // tabSalesOverTime
            // 
            tabSalesOverTime.Controls.Add(splitContainerSales);
            tabSalesOverTime.Location = new Point(4, 24);
            tabSalesOverTime.Margin = new Padding(2, 3, 2, 3);
            tabSalesOverTime.Name = "tabSalesOverTime";
            tabSalesOverTime.Padding = new Padding(2, 3, 2, 3);
            tabSalesOverTime.Size = new Size(1114, 412);
            tabSalesOverTime.TabIndex = 0;
            tabSalesOverTime.Text = "Sales Over Time";
            tabSalesOverTime.UseVisualStyleBackColor = true;
            // 
            // splitContainerSales
            // 
            splitContainerSales.Dock = DockStyle.Fill;
            splitContainerSales.Location = new Point(2, 3);
            splitContainerSales.Margin = new Padding(2);
            splitContainerSales.Name = "splitContainerSales";
            // 
            // splitContainerSales.Panel1
            // 
            splitContainerSales.Panel1.Controls.Add(chartSalesOverTime);
            // 
            // splitContainerSales.Panel2
            // 
            splitContainerSales.Panel2.Controls.Add(dgvSalesTimeData);
            splitContainerSales.Size = new Size(1110, 406);
            splitContainerSales.SplitterDistance = 895;
            splitContainerSales.SplitterWidth = 2;
            splitContainerSales.TabIndex = 0;
            // 
            // chartSalesOverTime
            // 
            chartArea5.AxisX.Interval = 1D;
            chartArea5.AxisX.LabelStyle.Angle = -45;
            chartArea5.AxisX.Title = "Time Period (Month-Year)";
            chartArea5.AxisY.Title = "Sales Amount ($)";
            chartArea5.BackColor = Color.Honeydew;
            chartArea5.Name = "ChartArea1";
            chartSalesOverTime.ChartAreas.Add(chartArea5);
            chartSalesOverTime.Dock = DockStyle.Fill;
            legend5.Alignment = StringAlignment.Center;
            legend5.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend5.Name = "Legend1";
            chartSalesOverTime.Legends.Add(legend5);
            chartSalesOverTime.Location = new Point(0, 0);
            chartSalesOverTime.Margin = new Padding(2, 3, 2, 3);
            chartSalesOverTime.Name = "chartSalesOverTime";
            series5.BorderWidth = 3;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Color = Color.Green;
            series5.Legend = "Legend1";
            series5.MarkerColor = Color.DarkGreen;
            series5.MarkerSize = 8;
            series5.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
            series5.Name = "Sales";
            chartSalesOverTime.Series.Add(series5);
            chartSalesOverTime.Size = new Size(895, 406);
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
            dgvSalesTimeData.Margin = new Padding(2, 3, 2, 3);
            dgvSalesTimeData.Name = "dgvSalesTimeData";
            dgvSalesTimeData.ReadOnly = true;
            dgvSalesTimeData.RowHeadersWidth = 51;
            dgvSalesTimeData.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSalesTimeData.Size = new Size(213, 406);
            dgvSalesTimeData.TabIndex = 0;
            // 
            // tabMaxMinProducts
            // 
            tabMaxMinProducts.Controls.Add(panelMaxMin);
            tabMaxMinProducts.Controls.Add(splitContainerMaxMin);
            tabMaxMinProducts.Location = new Point(4, 24);
            tabMaxMinProducts.Margin = new Padding(2, 3, 2, 3);
            tabMaxMinProducts.Name = "tabMaxMinProducts";
            tabMaxMinProducts.Padding = new Padding(2, 3, 2, 3);
            tabMaxMinProducts.Size = new Size(1114, 412);
            tabMaxMinProducts.TabIndex = 1;
            tabMaxMinProducts.Text = "Max/Min Products";
            tabMaxMinProducts.UseVisualStyleBackColor = true;
            // 
            // panelMaxMin
            // 
            panelMaxMin.BackColor = Color.WhiteSmoke;
            panelMaxMin.Controls.Add(lblMinProduct);
            panelMaxMin.Controls.Add(lblMaxProduct);
            panelMaxMin.Dock = DockStyle.Top;
            panelMaxMin.Location = new Point(2, 3);
            panelMaxMin.Margin = new Padding(2, 3, 2, 3);
            panelMaxMin.Name = "panelMaxMin";
            panelMaxMin.Size = new Size(1110, 50);
            panelMaxMin.TabIndex = 1;
            // 
            // lblMinProduct
            // 
            lblMinProduct.AutoSize = true;
            lblMinProduct.Font = new Font("Arial", 11F, FontStyle.Bold);
            lblMinProduct.ForeColor = Color.Red;
            lblMinProduct.Location = new Point(18, 28);
            lblMinProduct.Margin = new Padding(2, 0, 2, 0);
            lblMinProduct.Name = "lblMinProduct";
            lblMinProduct.Size = new Size(49, 18);
            lblMinProduct.TabIndex = 1;
            lblMinProduct.Text = "MIN: -";
            // 
            // lblMaxProduct
            // 
            lblMaxProduct.AutoSize = true;
            lblMaxProduct.Font = new Font("Arial", 11F, FontStyle.Bold);
            lblMaxProduct.ForeColor = Color.Green;
            lblMaxProduct.Location = new Point(18, 8);
            lblMaxProduct.Margin = new Padding(2, 0, 2, 0);
            lblMaxProduct.Name = "lblMaxProduct";
            lblMaxProduct.Size = new Size(53, 18);
            lblMaxProduct.TabIndex = 0;
            lblMaxProduct.Text = "MAX: -";
            // 
            // splitContainerMaxMin
            // 
            splitContainerMaxMin.Dock = DockStyle.Fill;
            splitContainerMaxMin.Location = new Point(2, 3);
            splitContainerMaxMin.Margin = new Padding(2);
            splitContainerMaxMin.Name = "splitContainerMaxMin";
            // 
            // splitContainerMaxMin.Panel1
            // 
            splitContainerMaxMin.Panel1.Controls.Add(chartMaxMinProducts);
            // 
            // splitContainerMaxMin.Panel2
            // 
            splitContainerMaxMin.Panel2.Controls.Add(dgvProductSales);
            splitContainerMaxMin.Size = new Size(1110, 406);
            splitContainerMaxMin.SplitterDistance = 893;
            splitContainerMaxMin.SplitterWidth = 2;
            splitContainerMaxMin.TabIndex = 2;
            // 
            // chartMaxMinProducts
            // 
            chartArea6.AxisX.Interval = 1D;
            chartArea6.AxisX.LabelStyle.Angle = -45;
            chartArea6.AxisX.Title = "Product";
            chartArea6.AxisY.Title = "Sales Amount ($)";
            chartArea6.BackColor = Color.LightYellow;
            chartArea6.Name = "ChartArea1";
            chartMaxMinProducts.ChartAreas.Add(chartArea6);
            chartMaxMinProducts.Dock = DockStyle.Fill;
            legend6.Alignment = StringAlignment.Center;
            legend6.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend6.Name = "Legend1";
            chartMaxMinProducts.Legends.Add(legend6);
            chartMaxMinProducts.Location = new Point(0, 0);
            chartMaxMinProducts.Margin = new Padding(2, 3, 2, 3);
            chartMaxMinProducts.Name = "chartMaxMinProducts";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Sales";
            chartMaxMinProducts.Series.Add(series6);
            chartMaxMinProducts.Size = new Size(893, 406);
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
            dgvProductSales.Margin = new Padding(2, 3, 2, 3);
            dgvProductSales.Name = "dgvProductSales";
            dgvProductSales.ReadOnly = true;
            dgvProductSales.RowHeadersWidth = 51;
            dgvProductSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductSales.Size = new Size(215, 406);
            dgvProductSales.TabIndex = 0;
            // 
            // tabFutureSalesEstimation
            // 
            tabFutureSalesEstimation.Controls.Add(splitContainerFutureSalesEstimation);
            tabFutureSalesEstimation.Location = new Point(4, 24);
            tabFutureSalesEstimation.Name = "tabFutureSalesEstimation";
            tabFutureSalesEstimation.Padding = new Padding(3);
            tabFutureSalesEstimation.Size = new Size(1114, 412);
            tabFutureSalesEstimation.TabIndex = 2;
            tabFutureSalesEstimation.Text = "Future Sales Estimation";
            tabFutureSalesEstimation.UseVisualStyleBackColor = true;
            // 
            // splitContainerFutureSalesEstimation
            // 
            splitContainerFutureSalesEstimation.Dock = DockStyle.Fill;
            splitContainerFutureSalesEstimation.Location = new Point(3, 3);
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
            splitContainerFutureSalesEstimation.Size = new Size(1108, 406);
            splitContainerFutureSalesEstimation.SplitterDistance = 750;
            splitContainerFutureSalesEstimation.TabIndex = 0;
            // 
            // chartFutureSalesEstimation
            // 
            chartArea7.Name = "ChartArea1";
            chartFutureSalesEstimation.ChartAreas.Add(chartArea7);
            chartFutureSalesEstimation.Dock = DockStyle.Fill;
            legend7.Name = "Legend1";
            chartFutureSalesEstimation.Legends.Add(legend7);
            chartFutureSalesEstimation.Location = new Point(0, 23);
            chartFutureSalesEstimation.Name = "chartFutureSalesEstimation";
            series7.ChartArea = "ChartArea1";
            series7.Legend = "Legend1";
            series7.Name = "Series1";
            chartFutureSalesEstimation.Series.Add(series7);
            chartFutureSalesEstimation.Size = new Size(750, 383);
            chartFutureSalesEstimation.TabIndex = 1;
            chartFutureSalesEstimation.Text = "chart1";
            // 
            // cmbForecastMonths
            // 
            cmbForecastMonths.Dock = DockStyle.Top;
            cmbForecastMonths.FormattingEnabled = true;
            cmbForecastMonths.Location = new Point(0, 0);
            cmbForecastMonths.Name = "cmbForecastMonths";
            cmbForecastMonths.Size = new Size(750, 23);
            cmbForecastMonths.TabIndex = 0;
            // 
            // dgvSalesData
            // 
            dgvSalesData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvSalesData.Dock = DockStyle.Fill;
            dgvSalesData.Location = new Point(0, 0);
            dgvSalesData.Name = "dgvSalesData";
            dgvSalesData.Size = new Size(354, 406);
            dgvSalesData.TabIndex = 0;
            // 
            // tabEvolutionOfProfits
            // 
            tabEvolutionOfProfits.Controls.Add(splitContainerEvolutionOfProfit);
            tabEvolutionOfProfits.Location = new Point(4, 24);
            tabEvolutionOfProfits.Name = "tabEvolutionOfProfits";
            tabEvolutionOfProfits.Padding = new Padding(3);
            tabEvolutionOfProfits.Size = new Size(1114, 412);
            tabEvolutionOfProfits.TabIndex = 3;
            tabEvolutionOfProfits.Text = "Evolution of Profits";
            tabEvolutionOfProfits.UseVisualStyleBackColor = true;
            // 
            // splitContainerEvolutionOfProfit
            // 
            splitContainerEvolutionOfProfit.Dock = DockStyle.Fill;
            splitContainerEvolutionOfProfit.Location = new Point(3, 3);
            splitContainerEvolutionOfProfit.Name = "splitContainerEvolutionOfProfit";
            // 
            // splitContainerEvolutionOfProfit.Panel1
            // 
            splitContainerEvolutionOfProfit.Panel1.Controls.Add(chartEvolutionOfProfits);
            splitContainerEvolutionOfProfit.Panel1.Controls.Add(pnlClientTypeFilters);
            // 
            // splitContainerEvolutionOfProfit.Panel2
            // 
            splitContainerEvolutionOfProfit.Panel2.Controls.Add(dgvProfitData);
            splitContainerEvolutionOfProfit.Size = new Size(1108, 406);
            splitContainerEvolutionOfProfit.SplitterDistance = 750;
            splitContainerEvolutionOfProfit.TabIndex = 0;
            // 
            // chartEvolutionOfProfits
            // 
            chartArea8.Name = "ChartArea1";
            chartEvolutionOfProfits.ChartAreas.Add(chartArea8);
            chartEvolutionOfProfits.Dock = DockStyle.Fill;
            legend8.Name = "Legend1";
            chartEvolutionOfProfits.Legends.Add(legend8);
            chartEvolutionOfProfits.Location = new Point(0, 50);
            chartEvolutionOfProfits.Name = "chartEvolutionOfProfits";
            series8.ChartArea = "ChartArea1";
            series8.Legend = "Legend1";
            series8.Name = "Series1";
            chartEvolutionOfProfits.Series.Add(series8);
            chartEvolutionOfProfits.Size = new Size(750, 356);
            chartEvolutionOfProfits.TabIndex = 1;
            chartEvolutionOfProfits.Text = "chart1";
            // 
            // pnlClientTypeFilters
            // 
            pnlClientTypeFilters.Dock = DockStyle.Top;
            pnlClientTypeFilters.Location = new Point(0, 0);
            pnlClientTypeFilters.Name = "pnlClientTypeFilters";
            pnlClientTypeFilters.Size = new Size(750, 50);
            pnlClientTypeFilters.TabIndex = 0;
            // 
            // dgvProfitData
            // 
            dgvProfitData.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvProfitData.Dock = DockStyle.Fill;
            dgvProfitData.Location = new Point(0, 0);
            dgvProfitData.Name = "dgvProfitData";
            dgvProfitData.Size = new Size(354, 406);
            dgvProfitData.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1122, 530);
            Controls.Add(tabControl);
            Controls.Add(panelFilters);
            Margin = new Padding(2, 3, 2, 3);
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
            panelMaxMin.ResumeLayout(false);
            panelMaxMin.PerformLayout();
            splitContainerMaxMin.Panel1.ResumeLayout(false);
            splitContainerMaxMin.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMaxMin).EndInit();
            splitContainerMaxMin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartMaxMinProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvProductSales).EndInit();
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
    }
}