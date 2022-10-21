using ArfolyamService.Entities;
using ArfolyamService.MnbServiceReference;
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

namespace ArfolyamService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            RefreshData();
        }

        void RefreshData()
        {
            Rates.Clear();

            dataGridView1.DataSource = Rates;
            chartRateData.DataSource = Rates;
            string mnbresult = CallMnb();
            ProcessXml(mnbresult);
            DrawChart();
        }

        string CallMnb()
        {
            MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();
            GetExchangeRatesRequestBody request = new GetExchangeRatesRequestBody()
            {
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dateTimePicker1.Value.ToString(),
                endDate = dateTimePicker2.Value.ToString()
            };

            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            //MessageBox.Show(result);
            return result;
        }

        BindingList<RateData> Rates = new BindingList<RateData>();

        void ProcessXml(string result_in)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result_in);

            foreach (XmlElement elem in xml.DocumentElement)
            {
                XmlElement gyerek = (XmlElement)elem.ChildNodes[0];
                int unit;
                decimal val;
                decimal excRate;
                int.TryParse(gyerek.GetAttribute("unit"), out unit);
                decimal.TryParse(gyerek.InnerText, out val);
                switch (unit)
                {
                    case 0:
                        excRate = 0;
                        break;
                    default:
                        excRate = val / unit;
                        break;
                }
                
                RateData rd = new RateData()
                {
                    Date = DateTime.Parse(elem.GetAttribute("date")),
                    Currency = gyerek.GetAttribute("curr"),
                    Value = excRate
                };

                Rates.Add(rd);
            }
        }

        void DrawChart()
        {
            var serie = chartRateData.Series[0];
            serie.ChartType = SeriesChartType.Line;
            serie.XValueMember = "Date";
            serie.YValueMembers = "Value";
            serie.BorderWidth = 2;

            var area = chartRateData.ChartAreas[0];
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.Enabled = false;
            area.AxisY.IsStartedFromZero = false;

            chartRateData.Legends[0].Enabled = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
