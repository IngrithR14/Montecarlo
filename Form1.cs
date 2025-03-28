using System;
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
            int seed = Convert.ToInt32(txtSeed.Text);
            int a = Convert.ToInt32(txtA.Text);
            int b = Convert.ToInt32(txtB.Text);
            int m = Convert.ToInt32(txtM.Text);
            float li = 0.5f;
            int ls = 3;
            int iteraciones = 0;
            if (rb10.Checked == true)
                iteraciones = 20;
            else if (rb100.Checked == true)
                iteraciones = 200;
            else if (rb1000.Checked == true)
                iteraciones = 2000;
            else if (rb10000.Checked == true)
                iteraciones = 20000;
            
            float xn, un,du;
            xn = seed;
            un = xn / m;
            float[] arrXn = new float[iteraciones];
            arrXn[0] = xn;
            dgvData.Rows.Add(1,xn, un , (li + ((ls - li) * un)));
            chFrecuencia.Series[0].Points.Clear();
            for (int i = 1; i < iteraciones; i++)
            {
                arrXn[i] = (a * arrXn[i - 1] + b) % m;
                xn = arrXn[i];
                un = arrXn[i] / m;
                du = (li+((ls-li)*un));
                dgvData.Rows.Add(i + 1,xn,un,du);
                chFrecuencia.Series[0].Points.AddXY(i, un);
            }
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
    }
}
