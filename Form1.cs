using System;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace Montecarlo
{
    public partial class Form1: Form
    {
        DataTable dataSet = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnProcesar_Click(object sender, EventArgs e)
        {
            int seed = Convert.ToInt32(txtSeed.Text);
            int a = Convert.ToInt32(txtA.Text);
            int b = Convert.ToInt32(txtB.Text);
            int m = Convert.ToInt32(txtM.Text);
            int iterations = Convert.ToInt32(txtIteration.Text);
            //xn=(a*(xn-1)+b) mod m
            //un=xn/m
            float xn, un;
            xn = (a * seed + b) % m;
            un = xn / m;
            lblR2.Text = xn.ToString();
            lblR1.Text = un.ToString();
            float[] arrXn = new float[iterations];
            arrXn[0] = xn;
            dataSet.Rows.Add(0, seed, arrXn[0] / m);
            chart1.Series[0].Points.Clear();
            for (int i = 1; i < iterations; i++)
            {
                arrXn[i]= (a * arrXn[i-1] + b) % m;
                dataSet.Rows.Add(i, arrXn[i], arrXn[i]/m);
                chart1.Series[0].Points.AddXY(i, arrXn[i]/m);
            }
           
            dgvResults.DataSource = dataSet;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataSet.Columns.Add("Iteracion",typeof(int));
            dataSet.Columns.Add("Xn", typeof(float));
            dataSet.Columns.Add("Un", typeof(float));
        }

        private void button2_Click(object sender, EventArgs e)
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
        }

        
    }
}
