using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace lms_it13.Views
{
    public class DashboardControl : UserControl
    {
        private Panel panelStats;
        private Panel panelChart;


        public DashboardControl()
        {
            BuildDashboard();
            this.DoubleBuffered = true;
            this.Load += DashboardControl_Load;

        }
        private void DashboardControl_Load(object sender, EventArgs e)
        {
            BuildDashboard();
        }

        private void BuildDashboard()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = ColorTranslator.FromHtml("#F7F8F0");

            Panel container = new Panel();
            container.Dock = DockStyle.Fill;
            container.Padding = new Padding(30);
            this.Controls.Add(container);

            // 🔹 CHART (Add FIRST so it stays at bottom)
            BuildChartSection(container);

            // 🔹 STATS (Add SECOND)
            BuildStatsSection(container);

            // 🔹 TITLE (Add LAST so it appears on TOP)
            Label lblTitle = new Label();
            lblTitle.Text = "Dashboard";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = ColorTranslator.FromHtml("#355872");
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 60;

            container.Controls.Add(lblTitle);
        }

        private void BuildStatsSection(Panel parent)
        {
            panelStats = new Panel();
            panelStats.Dock = DockStyle.Top;
            panelStats.Height = 180;
            panelStats.Padding = new Padding(0, 10, 0, 10);
            parent.Controls.Add(panelStats);

            TableLayoutPanel table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.ColumnCount = 5;
            table.RowCount = 1;

            for (int i = 0; i < 5; i++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));

            panelStats.Controls.Add(table);

            table.Controls.Add(CreateStatCard("85", "All Books"), 0, 0);
            table.Controls.Add(CreateStatCard("80", "Remaining Books"), 1, 0);
            table.Controls.Add(CreateStatCard("5", "Issued Books"), 2, 0);
            table.Controls.Add(CreateStatCard("9", "All Students"), 3, 0);
            table.Controls.Add(CreateStatCard("2", "Holding Books"), 4, 0);
        }

        private Panel CreateStatCard(string number, string text)
        {
            Panel card = new Panel();
            card.Dock = DockStyle.Fill;
            card.Margin = new Padding(10);
            card.BackColor = Color.White;

            Panel circle = new Panel();
            circle.Size = new Size(70, 70);
            circle.BackColor = ColorTranslator.FromHtml("#355872");
            circle.Location = new Point(
                (card.Width - 70) / 2,
                15
            );

            circle.Anchor = AnchorStyles.Top;
            circle.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode =
                    System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                e.Graphics.FillEllipse(
                    new SolidBrush(circle.BackColor),
                    0, 0, 70, 70);
            };

            Label lblNumber = new Label();
            lblNumber.Text = number;
            lblNumber.ForeColor = Color.White;
            lblNumber.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblNumber.AutoSize = true;
            lblNumber.BackColor = Color.Transparent;
            lblNumber.Location = new Point(20, 20);

            circle.Controls.Add(lblNumber);
            card.Controls.Add(circle);

            Label lblText = new Label();
            lblText.Text = text;
            lblText.Dock = DockStyle.Bottom;
            lblText.Height = 40;
            lblText.TextAlign = ContentAlignment.MiddleCenter;
            lblText.Font = new Font("Segoe UI", 9);
            lblText.ForeColor = Color.Gray;

            card.Controls.Add(lblText);

            return card;
        }
                private void EnableDoubleBuffering(Control control)
        {
            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)
                ?.SetValue(control, true, null);
        }
        private void BuildChartSection(Panel parent)
        {
            Panel chartContainer = new Panel();
            chartContainer.Dock = DockStyle.Fill;
            chartContainer.BackColor = Color.White;
            chartContainer.Padding = new Padding(20);

            parent.Controls.Add(chartContainer);

            // Delay chart creation until panel has size
            chartContainer.HandleCreated += (s, e) =>
            {
                CreateChart(chartContainer);
            };
        }

        private void PanelChart_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            int[] values = { 85, 80, 5, 9, 2 };
            string[] labels = { "All Books", "Remaining", "Issued", "Students", "Holding" };

            int width = panelChart.ClientSize.Width;
            int height = panelChart.ClientSize.Height;

            int marginBottom = 50;
            int maxBarHeight = height - marginBottom - 20;

            int barWidth = width / (values.Length * 2);
            int spacing = width / (values.Length + 1);

            for (int i = 0; i < values.Length; i++)
            {
                int barHeight = (values[i] * maxBarHeight) / 100;

                int x = spacing * (i + 1) - barWidth / 2;
                int y = height - barHeight - marginBottom;

                Rectangle bar = new Rectangle(x, y, barWidth, barHeight);

                e.Graphics.FillRectangle(
                    new SolidBrush(ColorTranslator.FromHtml("#7AAACE")),
                    bar);

                // Draw label
                SizeF labelSize = e.Graphics.MeasureString(labels[i],
                    new Font("Segoe UI", 8));

                e.Graphics.DrawString(
                    labels[i],
                    new Font("Segoe UI", 8),
                    Brushes.Black,
                    x + (barWidth - labelSize.Width) / 2,
                    height - marginBottom + 5);
            }
        }
        private void CreateChart(Panel container)
        {
            if (container.Height <= 0) return;

            Chart chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.White;

            ChartArea area = new ChartArea();
            area.BackColor = Color.White;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;

            chart.ChartAreas.Add(area);

            Series series = new Series();
            series.ChartType = SeriesChartType.Column;
            series.Color = ColorTranslator.FromHtml("#7AAACE");
            series.IsValueShownAsLabel = true;

            series.Points.AddXY("All Books", 85);
            series.Points.AddXY("Remaining", 80);
            series.Points.AddXY("Issued", 5);
            series.Points.AddXY("Students", 9);
            series.Points.AddXY("Holding", 2);

            chart.Series.Add(series);

            container.Controls.Clear();
            container.Controls.Add(chart);
        }
    }
}