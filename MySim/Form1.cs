﻿using MySim.Entities;
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


        public Form1()
        {
            InitializeComponent();

            Population = loadPopulus(@"C:\Temp\nép-teszt.csv");
            BirthProbabilities = loadBirthPs(@"C:\Temp\születés.csv");
            DeathProbabilities = loadDeathPs(@"C:\Temp\halál.csv");

            dataGridView1.DataSource = Population;
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
    }
}
