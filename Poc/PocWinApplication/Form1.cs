using ExcelDataReader;
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

namespace PocWinApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void importButton_Click(object sender, EventArgs e)
        {
            importFileDialog.ShowDialog();

        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            exportFileDialog.ShowDialog();
        }

        private void exportFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            ExportAsTxt(); 

            ShowStats();
        }

        private void ExportAsTxt()
        {
            var txtStringBuilder = new StringBuilder();
            var columns = dataGridView1.Columns;
            for (int i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                txtStringBuilder.Append($"{column.Name}{(i == columns.Count - 1 ? "\n" : ";")}");
            }

            var rows = dataGridView1.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                for (int j = 0; j < row.Cells.Count; j++)
                {
                    var cell = row.Cells[j];
                    txtStringBuilder.Append($"{cell.Value}{(j == row.Cells.Count - 1 ? "\n" : ";")}");
                }
            }

            File.WriteAllText(exportFileDialog.FileName, txtStringBuilder.ToString());
        }

        private void ShowStats()
        {
            var data = DataGridToDto();
            var numberOfRows = data.Count();

            var message = $@"Toplam Satır = {numberOfRows}";

            MessageBox.Show(message, "Sonuç");
        }

        IEnumerable<PocDto> DataGridToDto()
        {
            var rows = dataGridView1.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                yield return new PocDto
                {
                    DetaySinif = (string)row.Cells[1].Value
                };
            }
        }

        private void importFileDialog_FileOk(object sender, CancelEventArgs e)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var stream = File.Open(importFileDialog.FileName, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);

            var result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            var table = result.Tables[0]; 

            dataGridView1.DataSource = table; 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
