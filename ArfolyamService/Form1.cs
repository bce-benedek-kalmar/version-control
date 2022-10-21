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
using System.Xml;

namespace ArfolyamService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = Rates;
            string mnbresult = CallMnb();
            ProcessXml(mnbresult);
        }

        string CallMnb()
        {
            MNBArfolyamServiceSoapClient mnbService = new MNBArfolyamServiceSoapClient();
            GetExchangeRatesRequestBody request = new GetExchangeRatesRequestBody()
            {
                currencyNames = "EUR",
                startDate = "2020-01-01",
                endDate = "2020-06-03"
            };

            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            MessageBox.Show(result);
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
                    Date = DateTime.Parse(elem.GetAttribute("Date")),
                    Currency = gyerek.GetAttribute("curr"),
                    Value = excRate
                };

                Rates.Add(rd);
            }
        }
    }
}
