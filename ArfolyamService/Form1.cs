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

namespace ArfolyamService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dataGridView1.DataSource = Rates;
            CallMnb();
        }

        void CallMnb()
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
        }

        BindingList<RateData> Rates = new BindingList<RateData>();
    }
}
