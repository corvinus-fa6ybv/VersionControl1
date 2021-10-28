using _6het.Entities;
using _6het.MnbServiceReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace _6het
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates;
        public string results;
        public Form1()
        {
            InitializeComponent();

            webszolgaltatasMeghivasa();

            XML();

            Diagram();

            dataGridView1.DataSource = Rates;
            
        }

        private void webszolgaltatasMeghivasa()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-30"
            };

            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;

            results = result;
        }

        private void XML()
        {
            var xml = new XmlDocument();
            
            xml.LoadXml(results);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateData();
                Rates.Add(rate);

                rate.Date = DateTime.Parse(element.GetAttribute("date"));

                var childElement = (XmlElement)element.ChildNodes[0];
                rate.Currency = childElement.GetAttribute("currency");

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                {
                    rate.Value = value / unit;
                }
            }
        }

        private void Diagram()
        {
            chartRatesData.DataSource = Rates;

            var series = chartRatesData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;

            var legend = chartRatesData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRatesData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }
    }
}
