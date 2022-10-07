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

        private void CreateTable()
        {
            string[] headers = new string[]
            {
                 "Kód",
                 "Eladó",
                 "Oldal",
                 "Kerület",
                 "Lift",
                 "Szobák száma",
                 "Alapterület (m2)",
                 "Ár (mFt)",
                 "Négyzetméter ár (Ft/m2)"
            };

            for (int i = 0; i < headers.Length; i++)
            {
                xlSheet.Cells[1, i + 1] = headers[i];
            }

            object[,] values = new object[flats.Count, headers.Length];
            int c = 0;

            foreach (Flat f in flats)
            {
                values[c, 0] = f.Code;
                values[c, 1] = f.Vendor;
                values[c, 2] = f.Side;
                values[c, 3] = f.District;
                switch (f.Elevator)
                {
                    case true:
                        values[c, 4] = "Van";
                        break;
                    case false:
                        values[c, 4] = "Nincs";
                        break;
                }
                values[c, 5] = f.NumberOfRooms;
                values[c, 6] = f.FloorArea;
                values[c, 7] = f.Price;
                values[c, 8] = "";
                c++;
            }

            xlSheet.get_Range(
                GetCell(2, 1),
                GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;

            for (int i = 0; i < values.GetLength(0); i++)
            {
                xlSheet.Cells[i + 2, 9] = string.Format("={0}*1000000/{1}", GetCell(i+2, 8), GetCell(i+2, 7));
            }

            //Formázás

            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 40;
            headerRange.Interior.Color = Color.LightBlue;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            Excel.Range tableRange = xlSheet.get_Range(GetCell(1, 1),
                                                        GetCell(1 + values.GetLength(0), values.GetLength(1)));
            tableRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            Excel.Range firstColRange = xlSheet.get_Range(GetCell(2,1),GetCell(values.GetLength(0) + 1, 1));
            firstColRange.Font.Bold = true;
            firstColRange.Interior.Color = Color.LightGoldenrodYellow;

            Excel.Range lastColRange = xlSheet.get_Range(GetCell(2, 9), GetCell(values.GetLength(0) + 1, 9));
            lastColRange.Interior.Color = Color.LightGreen;
            lastColRange.NumberFormat = "#,##0.00";
        }

        private string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }
    }
}
