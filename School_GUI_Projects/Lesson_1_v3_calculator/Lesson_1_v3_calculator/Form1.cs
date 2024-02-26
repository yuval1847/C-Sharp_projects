using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lesson_1_v3_calculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string EvaluateExpression(string expression)
        {
            // Using DataTable.Compute method to evaluate the expression
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '/' && expression[i + 1] == '0')
                {
                    return "Error!";
                }
            }
            DataTable table = new DataTable();
            DataColumn column = new DataColumn("expression", typeof(double), expression);
            table.Columns.Add(column);
            table.Rows.Add(0);
            double result = (double)(table.Rows[0]["expression"]);
            return result.ToString();
        }

        private void btn_0_Click(object sender, EventArgs e)
        {
            if (label_result.Text != "0")
            {
                label_result.Text += "0";
            }
        }

        private void btn_1_Click(object sender, EventArgs e)
        {
            if (label_result.Text == "0")
            {
                label_result.Text = "1";
            }
            else
            {
                label_result.Text += "1";
            }
        }
        private void btn_2_Click(object sender, EventArgs e)
        {
            if (label_result.Text == "0")
            {
                label_result.Text = "2";
            }
            else
            {
                label_result.Text += "2";
            }
        }
        private void btn_3_Click(object sender, EventArgs e)
        {
            if (label_result.Text == "0")
            {
                label_result.Text = "3";
            }
            else
            {
                label_result.Text += "3";
            }
        }
        private void btn_4_Click(object sender, EventArgs e)
        {
            if (label_result.Text == "0")
            {
                label_result.Text = "4";
            }
            else
            {
                label_result.Text += "4";
            }
        }
        private void btn_5_Click(object sender, EventArgs e)
        {
            if (label_result.Text == "0")
            {
                label_result.Text = "5";
            }
            else
            {
                label_result.Text += "5";
            }
        }
        private void btn_6_Click(object sender, EventArgs e)
        {
            if (label_result.Text == "0")
            {
                label_result.Text = "6";
            }
            else
            {
                label_result.Text += "6";
            }
        }
        private void btn_7_Click(object sender, EventArgs e)
        {
            if (label_result.Text == "0")
            {
                label_result.Text = "7";
            }
            else
            {
                label_result.Text += "7";
            }
        }
        private void btn_8_Click(object sender, EventArgs e)
        {
            if (label_result.Text == "0")
            {
                label_result.Text = "8";
            }
            else
            {
                label_result.Text += "8";
            }
        }
        private void btn_9_Click(object sender, EventArgs e)
        {
            if (label_result.Text == "0")
            {
                label_result.Text = "9";
            }
            else
            {
                label_result.Text += "9";
            }
        }

        private void plus_btn_Click(object sender, EventArgs e)
        {
            if (!(label_result.Text[label_result.Text.Length - 1] == '+' || label_result.Text[label_result.Text.Length - 1] == '-' || label_result.Text[label_result.Text.Length - 1] == '*' || label_result.Text[label_result.Text.Length - 1] == '/'))
            {
                label_result.Text += "+";
            }
        }

        private void minus_btn_Click(object sender, EventArgs e)
        {
            if (!(label_result.Text[label_result.Text.Length - 1] == '+' || label_result.Text[label_result.Text.Length - 1] == '-' || label_result.Text[label_result.Text.Length - 1] == '*' || label_result.Text[label_result.Text.Length - 1] == '/'))
            {
                label_result.Text += "-";
            }
        }

        private void multi_btn_Click(object sender, EventArgs e)
        {
            if (!(label_result.Text[label_result.Text.Length - 1] == '+' || label_result.Text[label_result.Text.Length - 1] == '-' || label_result.Text[label_result.Text.Length - 1] == '*' || label_result.Text[label_result.Text.Length - 1] == '/'))
            {
                label_result.Text += "*";
            }
        }

        private void Divide_btn_Click(object sender, EventArgs e)
        {
            if (!(label_result.Text[label_result.Text.Length - 1] == '+' || label_result.Text[label_result.Text.Length - 1] == '-' || label_result.Text[label_result.Text.Length - 1] == '*' || label_result.Text[label_result.Text.Length - 1] == '/'))
            {
                label_result.Text += "/";
            }
        }

        private void result_btn_Click(object sender, EventArgs e)
        {
            label_result.Text = this.EvaluateExpression(label_result.Text);
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            label_result.Text = "0";
        }

        private void label_result_Click(object sender, EventArgs e)
        {

        }

        
    }
}
