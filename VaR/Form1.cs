using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VaR.Entities;

namespace VaR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Ticks = context.Ticks.ToList();
            dataGridView1.DataSource = Ticks;
            CreatePortfolio();
            CalculateValueatRisk();

        }

        List<Tick> Ticks;
        PortfolioEntities context = new PortfolioEntities();
        List<PortfolioItem> Portfolio = new List<PortfolioItem>();
        List<Nyereseg> nyereségekRendezve;
        int intervalum = 30;

        void CreatePortfolio()
        {
            Portfolio.Add(new PortfolioItem() { Index = "OTP", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ZWACK", Volume = 10 });
            Portfolio.Add(new PortfolioItem() { Index = "ELMU", Volume = 10 });

            dataGridView2.DataSource = Portfolio;
        }

        private decimal GetPortfolioValue(DateTime date)
        {
            decimal value = 0;
            foreach (var item in Portfolio)
            {
                var last = (from x in Ticks
                            where item.Index == x.Index.Trim() 
                                    && date <= x.TradingDay
                            select x)
                            .First();
                value += (decimal)last.Price * item.Volume;
            }
            return value;
        }

        public void CalculateValueatRisk()
        {
            List<Nyereseg> Nyereségek = new List<Nyereseg>();
            DateTime kezdőDátum = (from x in Ticks select x.TradingDay).Min();
            DateTime záróDátum = new DateTime(2016, 12, 30);
            TimeSpan z = záróDátum - kezdőDátum;
            for (int i = 0; i < z.Days - intervalum; i++)
            {
                decimal ny = GetPortfolioValue(kezdőDátum.AddDays(i + intervalum))
                           - GetPortfolioValue(kezdőDátum.AddDays(i));
                Nyereségek.Add(new Nyereseg() {NyeresegErtek = ny, 
                                                KezdoNap = kezdőDátum.AddDays(i),
                                                Sorszam = Nyereségek.Count} );
                Console.WriteLine(i + " " + ny);
            }

            nyereségekRendezve = (from x in Nyereségek
                                      orderby x.NyeresegErtek
                                      select x)
                                        .ToList();
            MessageBox.Show(nyereségekRendezve[nyereségekRendezve.Count() / 5].NyeresegErtek.ToString());
        }

        private void b_Sav_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog() { DefaultExt = ".csv"};
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName);
                sw.WriteLine(";Időszak;Nyereségek");
                foreach (Nyereseg earning in nyereségekRendezve)
                {
                    sw.WriteLine("{0};{1} - {2};{3}",earning.Sorszam.ToString(),
                                            earning.KezdoNap.ToShortDateString(),
                                            earning.KezdoNap.AddDays(intervalum).ToShortDateString(),
                                            earning.NyeresegErtek.ToString());
                }
                sw.Close();
            }
            
        }
    }
}
