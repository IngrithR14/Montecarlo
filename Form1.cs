using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Montecarlo
{
    public partial class Form1: Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /*private void button2_Click(object sender, EventArgs e)
        {
            chart2.Series[0].Points.Clear();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            chart2.Series[1].Points.AddXY(1, 1);
            chart2.Series[1].Points.AddXY(1, 5);
            chart2.Series[1].Points.AddXY(5, 5);
            chart2.Series[1].Points.AddXY(5, 1);
            chart2.Series[1].Points.AddXY(1, 1);
            Random random = new Random();
            for (int i = 0; i <100; i++)
            {
                float x = (float)(random.NextDouble()*(5-1)+1);
                float y= (float)(random.NextDouble()*(5-1)+1);
                chart2.Series[0].Points.AddXY(x,y);
            }
            watch.Stop();
            lblTiempo.Text = watch.Elapsed.TotalSeconds.ToString();
        }*/

        private void btnProcesarCongruencial_Click(object sender, EventArgs e)
        {

            // Verificar si los campos de texto están vacíos
            if (string.IsNullOrWhiteSpace(txtSeed.Text) ||
                string.IsNullOrWhiteSpace(txtA.Text) ||
                string.IsNullOrWhiteSpace(txtB.Text) ||
                string.IsNullOrWhiteSpace(txtM.Text))
            {
                MessageBox.Show("Por favor, complete todos los campos antes de continuar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar si se ha seleccionado una opción de radio
            if (!rb10.Checked && !rb100.Checked && !rb1000.Checked && !rb10000.Checked)
            {
                MessageBox.Show("Por favor, seleccione una cantidad de coordenadas aleatorias.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Holi esto limpia datos de la tabla antes de ingresar nuevos
            dgvData.Rows.Clear();


            int seed = Convert.ToInt32(txtSeed.Text);
            int a = Convert.ToInt32(txtA.Text);
            int b = Convert.ToInt32(txtB.Text);
            int m = Convert.ToInt32(txtM.Text);
            float li = 0.5f;
            int ls = 3;
            int iteraciones = 0;

            if (rb10.Checked) iteraciones = 20;
            else if (rb100.Checked) iteraciones = 200;
            else if (rb1000.Checked) iteraciones = 2000;
            else if (rb10000.Checked) iteraciones = 20000;

            float xn, un, du;
            xn = seed;
            un = xn / m;
            float[] arrXn = new float[iteraciones];
            arrXn[0] = xn;
            dgvData.Rows.Add(1, xn, un, (li + ((ls - li) * un)));
            chFrecuencia.Series[0].Points.Clear();

            // Histograma
            chRangos.Series.Clear();
            int numBins = (int)Math.Ceiling(1 + Math.Log(iteraciones, 2));
            Dictionary<double, int> histogramBins = new Dictionary<double, int>();

            for (int i = 1; i < iteraciones; i++)
            {
                arrXn[i] = (a * arrXn[i - 1] + b) % m;
                xn = arrXn[i];
                un = arrXn[i] / m;
                du = (li + ((ls - li) * un));

                dgvData.Rows.Add(i + 1, xn, un, du);
                chFrecuencia.Series[0].Points.AddXY(i, un);

                // Cálculo de intervalos para histograma manteniendo decimales
                double binWidth = 1.0 / numBins;
                double binValue = Math.Floor(un / binWidth) * binWidth;

                if (!histogramBins.ContainsKey(binValue))
                    histogramBins[binValue] = 0;
                histogramBins[binValue]++;
            }

            // Crear y agregar la serie al histograma
            Series serieHistograma = new Series("Histograma");
            serieHistograma.ChartType = SeriesChartType.Column;
            serieHistograma.Color = System.Drawing.Color.Blue;

            foreach (var bin in histogramBins)
            {
                serieHistograma.Points.AddXY(bin.Key, bin.Value);
            }

            chRangos.Series.Add(serieHistograma);
            chRangos.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
        }


        private void btnPgGrafica_Click(object sender, EventArgs e)
        {
            tabControlCongruencial.SelectedTab = pgGraficasCongruencial;
        }

        private void btnPgMontecarlo_Click(object sender, EventArgs e)
        {
            tabControlMontecarlo.SelectedTab = pgMontecarlo;
        }

        private void btnPgGraficaM_Click(object sender, EventArgs e)
        {
            tabControlMontecarlo1.SelectedTab = pgGraficaM;
        }


        //Bloquear en los text box que solo se ingresen numeros positivos
        //se bloquearon numeros negativos y letras
        private void txtA_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void txtB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; 
            }
        }

        private void txtM_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; 
            }
        }

        private void txtSeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true; 
            }
        }
    }
}
