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
            if (!(label_result.Text[label_result.Text.Length-1] == '+' || label_result.Text[label_result.Text.Length - 1] == '-'))
            {
                label_result.Text += "+";
            }
        }

        private void minus_btn_Click(object sender, EventArgs e)
        {
            if (!(label_result.Text[label_result.Text.Length - 1] == '+' || label_result.Text[label_result.Text.Length - 1] == '-'))
            {
                label_result.Text += "-";
            }
        }
        private void result_btn_Click(object sender, EventArgs e)
        {
            int first_num = 0, semi_result = 0, index = 0, calculating_result = 0;
            char operator_sign;
            if (!(label_result.Text.Length == 1 && (label_result.Text[label_result.Text.Length - 1] == '+' || label_result.Text[label_result.Text.Length - 1] == '-')))
            {
                label_result.Text += '$'; // A special sign to find the end of the string
                while (!(label_result.Text[index] == '+' || label_result.Text[index] == '-'))
                {
                    if (label_result.Text[index] == '$')
                    {
                        break;
                    }
                    first_num = first_num * 10 + ((int)(label_result.Text[index])-48);
                    index++;
                }
                calculating_result+=first_num;
                while (!(label_result.Text[index] == '$'))
                {
                    first_num = 0;
                    operator_sign = label_result.Text[index];
                    index++;
                    while (!(label_result.Text[index] == '+' || label_result.Text[index] == '-'))
                    {
                        if (label_result.Text[index] == '$')
                        {
                            break;
                        }
                        first_num = first_num * 10 + ((int)(label_result.Text[index]) - 48);
                        index++;
                    }
                    if (operator_sign == '+')
                    {
                        calculating_result += first_num;
                    }
                    else
                    {
                        calculating_result -= first_num;
                    }
                }
            }
            label_result.Text = calculating_result.ToString();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            label_result.Text = "0";
        }
    }
}
