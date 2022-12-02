using MySim.Entities;
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

namespace MySim
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        Random rng = new Random(1337);

        public Form1()
        {
            InitializeComponent();

            Population = loadPopulus(@"C:\Temp\nép-teszt.csv");
            BirthProbabilities = loadBirthPs(@"C:\Temp\születés.csv");
            DeathProbabilities = loadDeathPs(@"C:\Temp\halál.csv");

            
        }

        private void Simulation(decimal endYear)
        {
            for (int y = 2005; y < endYear; y++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(y, Population[i]);
                }

                int numMales = (from x in Population
                                where x.Gender == Gender.Male && x.Living
                                select x).Count();
                int numFemales = (from x in Population
                                  where x.Gender == Gender.Female && x.Living
                                  select x).Count();

                MessageBox.Show(string.Format("{0} M: {1} F: {2}", y, numMales, numFemales));
            }
        }

        void SimStep(int year, Person p)
        {
            if (!p.Living) return;

            int age = year - p.BirthYear;

            double dp = (from x in DeathProbabilities
                         where x.Age == age
                             && x.Gender == p.Gender
                         select x.Probability).FirstOrDefault();

            if (rng.NextDouble() < dp)
            {
                p.Living = false;
                return;
            }

            if (p.Gender == Gender.Male) return;

            double bp = (from x in BirthProbabilities
                         where x.Age == age
                            && x.NumChildren == p.NumChildren
                         select x.Probability).FirstOrDefault();

            if (rng.NextDouble() < bp)
            {
                Person baby = new Person()
                {
                    BirthYear = year,
                    Gender = (Gender)rng.Next(1,3),
                    NumChildren = 0
                };

                Population.Add(baby);
            }
        }

        List<Person> loadPopulus(string path)
        {

            List<Person> populus = new List<Person>();
            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(';');
                Person p = new Person()
                {
                    BirthYear = int.Parse(line[0]),
                    Gender = (Gender)byte.Parse(line[1]),
                    NumChildren = int.Parse(line[2])
                };

                populus.Add(p);
            }

            sr.Close();
            return populus;
        }

        List<BirthProbability> loadBirthPs(string path)
        {

            List<BirthProbability> birthPs = new List<BirthProbability>();
            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(';');
                BirthProbability p = new BirthProbability()
                {
                    Age = int.Parse(line[0]),
                    NumChildren = byte.Parse(line[1]),
                    Probability = double.Parse(line[2])
                };

                birthPs.Add(p);
            }

            sr.Close();
            return birthPs;
        }

        List<DeathProbability> loadDeathPs(string path)
        {

            List<DeathProbability> deathPs = new List<DeathProbability>();
            StreamReader sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split(';');
                DeathProbability p = new DeathProbability()
                {
                    Gender = (Gender)byte.Parse(line[0]),
                    Age = int.Parse(line[1]),
                    Probability = double.Parse(line[2])
                };

                deathPs.Add(p);
            }

            sr.Close();
            return deathPs;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Simulation(numericUpDown1.Value);
        }
    }
}
