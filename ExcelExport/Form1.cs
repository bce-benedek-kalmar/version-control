using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ExcelExport
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadData();
            CreateExcel();
        }

        List<Flat> flats;
        RealEstateEntities context = new RealEstateEntities();

        private void LoadData()
        {
            flats = context.Flats.ToList();
        }

        Excel.Application xlApp;
        Excel.Workbook xlWb;
        Excel.Worksheet xlSheet;

        private void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();
                xlWb = xlApp.Workbooks.Add(Missing.Value);
                xlSheet = xlWb.ActiveSheet;

                CreateTable();

                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception exc)
            {
                string ErrorMessage = string.Format("Error: {0}\nLine: {1}", exc.Message, exc.Source);
                MessageBox.Show(ErrorMessage, "Error");

                xlWb.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWb = null;
                xlApp = null;
            }
        }
    }
}
