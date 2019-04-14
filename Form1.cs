using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using org.mariuszgromada.math.mxparser;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public double Xbegin, Y, f, hf, y1;
        /*[DllImport(@"C:\Users\Konstantin\Desktop\Курсовая работа\Methods\x64\Release\Methods.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern line EulerMethod(double h, double x, double y, double y1);*/
        double h, Xend;

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        uint n;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.Hide();
            textBox9.Text = "f(x,y)=";
            dataGridView1.Columns.Add("Column5", "Y+hF");
            dataGridView1.Columns.Add("Column6", "Y1-Y");
            toolTip1.SetToolTip(textBox9, "Арифметические операции:\n" +
                                          "\t+ , - , * , /" +
                                          "\n ^ - возведение в степень\n х! - факториал от x\n" +
                                          "Тригонометрические функции:\n" +
                                          "\tsin(x) cos(x) tg(x) ctg(x) и т.п.\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Function F = new Function(textBox9.Text);
            if (!F.checkSyntax())
            {
                MessageBox.Show("Дифференциальное уравнение неправильно введено! Введите заного\nОбратите внимание на инстрeкцию." + F.getErrorMessage());
                return;
            }
            try
            {
                Xbegin = double.Parse(textBox1.Text);
                Xend = double.Parse(textBox7.Text);
                if (Xbegin >= Xend)
                {
                    MessageBox.Show("Начало и/или конец интервала заданы неверно! Введите заного.");
                    return;
                }
                Y = double.Parse(textBox2.Text);
                if (double.Parse(textBox6.Text) <= 0.0)
                {
                    MessageBox.Show("Итерационный шаг задан неверно! Проверьте правильность введённых данных.");
                    return;
                }
                if (radioButton3.Checked)
                {
                    h = double.Parse(textBox6.Text);
                    n = Convert.ToUInt32((Xend - Xbegin) / h);
                }
                else
                {
                    n = uint.Parse(textBox6.Text);
                    h = Convert.ToDouble((Xend - Xbegin) / n);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка ввода данных! Введите данные согласно инструкции.");
                return;
            }
            dataGridView1.Rows.Clear();
            if (radioButton1.Checked)
                EulerMethod(F);
            else ImproveEulerMethod(F);
        }
        private void EulerMethod(Function F)
        {
            dataGridView1.Columns[0].HeaderText = "X";
            dataGridView1.Columns[1].HeaderText = "Y";
            dataGridView1.Columns[2].HeaderText = "F(X,Y)";
            dataGridView1.Columns[3].HeaderText = "h*F(X,Y)";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            for (uint i = 0; i < n + 1; ++i)
            {

                f = F.calculate(Xbegin, Y);
                hf = h * f;
                y1 = Y + hf;
                dataGridView1.Rows.Add((float)Xbegin, (float)Y, (float)f, (float)hf);
                Xbegin += h;
                Y = y1;
            }
            dataGridView1.Show();
        }
        private void ImproveEulerMethod(Function F)
        {
            dataGridView1.Columns[0].HeaderText = "X";
            dataGridView1.Columns[1].HeaderText = "Y";
            dataGridView1.Columns[2].HeaderText = "X+h/2";
            dataGridView1.Columns[3].HeaderText = "F(X,Y)";
            dataGridView1.Columns[4].Visible = true;
            dataGridView1.Columns[5].Visible = true;
            for (uint i = 0; i < n + 1; ++i)
            {
                f = F.calculate(Xbegin, Y);
                hf = h * 0.5 * f;
                y1 = Y + h*F.calculate(Xbegin+h*0.5,Y+hf);
                dataGridView1.Rows.Add((float)Xbegin, (float)Y, (float)(Xbegin+h*0.5), (float)f, (float)(Y+hf), (float)(y1-Y));
                Xbegin += h;
                Y = y1;
            }
            dataGridView1.Show();
        }
    }
}
