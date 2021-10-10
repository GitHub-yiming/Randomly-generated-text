using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SQLite;
using 随机生成文字数据库版.Properties;
using System.IO;
using System.Text.RegularExpressions;

namespace 随机生成文字数据库版
{
    public partial class Form1 : Form
    {
        Connects connects = new Connects();
        public Form1()
        {
            InitializeComponent();
        }

        //错误错误单词
        int mistake = 0;
        //全部查询
        string sSQL = "SELECT WORD,TRANSLATION_1,TRANSLATION_2,TRANSLATION_3 FROM A_20200910";
        //数据库路径
        //string textBox1.Text = @"db\A_20200910.db";

        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = Convert.ToString(connects.GetDataTable(sSQL, "Data Source=" + textBox1.Text).Rows.Count) + "个单词";
            DataTable dbt = connects.GetDataTable(sSQL, "Data Source=" + textBox1.Text);
            this.dataGridView1.DataSource = dbt;

            checkBox2_CheckedChanged(null, null);
        }

        //行数绘制
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewStyle.DgvRowPostPaint(this.dataGridView1, e);
        }
        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewStyle.DgvRowPostPaint(this.dataGridView2, e);
        }

        //生成单词
        private void button2_Click(object sender, EventArgs e)
        {
            //正则表达式判断是否为数字
            Regex regex = new Regex("^(-?[0-9]*[.]*[0-9]{0,3})$");

            bool b = regex.IsMatch(textBox2.Text);

            if (b == true)

            {
                if (checkBox1.Checked)
                {
                    //随机查询
                    string random = string.Format("select WORD,TRANSLATION_1,TRANSLATION_2,TRANSLATION_3 from A_20200910 order by random() limit {0}", textBox2.Text);
                    DataTable dbt = connects.GetDataTable(random, "Data Source=" + textBox1.Text);
                    this.dataGridView1.DataSource = dbt;
                }
                else
                {
                    DataTable dbt = connects.GetDataTable(sSQL, "Data Source=" + textBox1.Text);
                    this.dataGridView1.DataSource = dbt;
                }
            }

            else

            {
                MessageBox.Show("输入的格式不正确！", "系统信息", MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

        }

        //数据库测试连接
        private void button1_Click(object sender, EventArgs e)
        {
            if (connects.ConnectTest("Data Source=" + textBox1.Text))
            {
                MessageBox.Show("连接正常！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("连接失败！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            label3.Text = Convert.ToString(connects.GetDataTable(sSQL, "Data Source=" + textBox1.Text).Rows.Count) + "个单词";
            DataTable dbt = connects.GetDataTable(sSQL, "Data Source=" + textBox1.Text);
            this.dataGridView1.DataSource = dbt;
        }

        //关闭程序时保存设置
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.Save();
        }

        //隐藏翻译
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                dataGridView1.Columns[0].Visible = true;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                checkBox3.Checked = false;
            }
            else
            {
                dataGridView1.Columns[0].Visible = true;
                dataGridView1.Columns[1].Visible = true;
                dataGridView1.Columns[2].Visible = true;
                dataGridView1.Columns[3].Visible = true;
            }
        }


        //隐藏单词
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = true;
                dataGridView1.Columns[2].Visible = true;
                dataGridView1.Columns[3].Visible = true;
                checkBox2.Checked = false;
            }
            else
            {
                dataGridView1.Columns[0].Visible = true;
                dataGridView1.Columns[1].Visible = true;
                dataGridView1.Columns[2].Visible = true;
                dataGridView1.Columns[3].Visible = true;
            }
        }

        //自动添加或者删除dataGridView2
        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (dataGridView1.Rows.Count > dataGridView2.Rows.Count)
            {
                dataGridView2.Rows.Add(dataGridView1.Rows.Count - dataGridView2.Rows.Count);
            }
            if (dataGridView2.Rows.Count > dataGridView1.Rows.Count)
            {
                for (int i = dataGridView2.Rows.Count; i > dataGridView1.Rows.Count; i--)
                {
                    dataGridView2.Rows.RemoveAt(i - 1);
                }

            }
        }

        //校对
        private void button3_Click(object sender, EventArgs e)
        {
            mistake = 0;
            int d = 1;
            if (checkBox3.Checked)
            {
                d = 0;
            }
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView2.Rows[i].Cells[0].Value != null)
                {

                    if (dataGridView1.Rows[i].Cells[d].Value.ToString().Trim() != dataGridView2.Rows[i].Cells[0].Value.ToString().Trim())
                    {
                        DataGridViewRow dgr = dataGridView2.Rows[i];
                        dgr.DefaultCellStyle.BackColor = Color.Red;
                        mistake++;
                    }
                    else
                    {
                        DataGridViewRow dgr = dataGridView2.Rows[i];
                        dgr.DefaultCellStyle.BackColor = Color.White;
                    }
                }
                else
                {
                    DataGridViewRow dgr = dataGridView2.Rows[i];
                    dgr.DefaultCellStyle.BackColor = Color.White;
                }

            }
            label2.Text = Convert.ToString(mistake) + "个错误";
        }

        //F5校验
        private void dataGridView2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                button3_Click(null, null);
            if (e.KeyCode == Keys.F4)
                button7_Click_1(null, null);
            if (e.KeyCode == Keys.F6)
                button8_Click(null, null);
        }

        //.txt路径
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                if (textBox3.Text == "")
                {
                    openFile.InitialDirectory = @"c:\";
                }
                else
                {
                    openFile.InitialDirectory = textBox3.Text;
                }
                openFile.Filter = "*.txt|*.txt";
                openFile.ShowDialog();
                textBox3.Text = System.IO.Path.GetDirectoryName(openFile.FileName);
                textBox3.Text = textBox3.Text + "\\" + openFile.SafeFileName;
            }
            catch (Exception)
            {
                //未知错误！
                //MessageBox.Show(ex.ToString(), "Message");
            }
        }

        //.db路径
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog();
                if (textBox1.Text == "")
                {
                    openFile.InitialDirectory = @"c:\";
                }
                else
                {
                    openFile.InitialDirectory = textBox1.Text;
                }
                openFile.Filter = "*.db|*.db";
                openFile.ShowDialog();
                textBox1.Text = System.IO.Path.GetDirectoryName(openFile.FileName);
                textBox1.Text = textBox1.Text + "\\" + openFile.SafeFileName;
            }
            catch (Exception)
            {
                //未知错误
                //MessageBox.Show(ex.ToString(), "Message");
            }
        }

        //.txt导入.db
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                StreamReader rd = new StreamReader(textBox3.Text, Encoding.UTF8);
                List<string> db1 = new List<string>();
                string db2;
                while ((db2 = rd.ReadLine()) != null)
                {
                    db1.Add(db2);
                }
                rd.Dispose();

                for (int i = 0; i < db1.Count; i++)
                {
                    //获取；的总数
                    int[] vs = new int[db1.ToArray().Length];
                    vs = GetSubStrCountInStr(db1.ToArray()[i], "；", 0);

                    int num1 = db1.ToArray()[i].IndexOf(",");
                    if (SubstringCount(db1.ToArray()[i], "；") == 0)
                    {
                        string word = db1.ToArray()[i].Substring(0, num1);
                        string translation_1 = db1.ToArray()[i].Substring(num1 + 1);
                        string sql = string.Format("INSERT INTO A_20200910 (WORD,TRANSLATION_1)  VALUES('{0}','{1}') ", word, translation_1);
                        connects.SetDataTable(sql, "Data Source=" + textBox1.Text);
                    }
                    if (SubstringCount(db1.ToArray()[i], "；") == 1)
                    {
                        string word = db1.ToArray()[i].Substring(0, num1);
                        string translation_1 = db1.ToArray()[i].Substring(num1 + 1, vs[0] - (num1 + 1));
                        string translation_2 = db1.ToArray()[i].Substring(vs[0] + 1);
                        string sql = string.Format("INSERT INTO A_20200910 (WORD,TRANSLATION_1,TRANSLATION_2)  VALUES('{0}','{1}','{2}') ", word, translation_1, translation_2);
                        connects.SetDataTable(sql, "Data Source=" + textBox1.Text);
                    }
                    if (SubstringCount(db1.ToArray()[i], "；") == 2)
                    {
                        string word = db1.ToArray()[i].Substring(0, num1);
                        string translation_1 = db1.ToArray()[i].Substring(num1 + 1, vs[0] - (num1 + 1));
                        string translation_2 = db1.ToArray()[i].Substring(vs[0] + 1, vs[1] - (vs[0] + 1));
                        string translation_3 = db1.ToArray()[i].Substring(vs[1] + 1);
                        string sql = string.Format("INSERT INTO A_20200910 (WORD,TRANSLATION_1,TRANSLATION_2,TRANSLATION_3)  VALUES('{0}','{1}','{2}','{3}') ", word, translation_1, translation_2, translation_3);
                        connects.SetDataTable(sql, "Data Source=" + textBox1.Text);
                    }
                    progressBar1.Maximum = db1.Count;
                    progressBar1.Value++;
                }
                progressBar1.Value = 0;
                MessageBox.Show("导入成功！", "系统信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(".txt地址不存在！", "系统信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //在一个字符串中返回指定字符重复的个数
        public int[] GetSubStrCountInStr(String str, String substr, int StartPos)
        {
            int foundPos = -1;
            int count = 0;
            List<int> foundItems = new List<int>();

            do
            {
                foundPos = str.IndexOf(substr, StartPos);
                if (foundPos > -1)
                {
                    StartPos = foundPos + 1;
                    count++;
                    foundItems.Add(foundPos);
                }
            } while (foundPos > -1 && StartPos < str.Length);

            return ((int[])foundItems.ToArray());
        }

        //在str中搜寻substring重复的值，并返回重复的数值
        static int SubstringCount(string str, string substring)
        {
            if (str.Contains(substring))
            {
                string strReplaced = str.Replace(substring, "");
                return (str.Length - strReplaced.Length) / substring.Length;
            }

            return 0;
        }

        //窗口置顶
        private void button7_Click_1(object sender, EventArgs e)
        {
            switch (button7.Text)
            {
                case "窗口置顶":
                    this.TopMost = true;
                    button7.Text = "取消置顶";
                    button7.BackColor = Color.Green;
                    break;
                case "取消置顶":
                    this.TopMost = false;
                    button7.Text = "窗口置顶";
                    button7.BackColor = SystemColors.Control;
                    break;
            }
        }

        //串口键盘响应事件
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
                button7_Click_1(null, null);
            if (e.KeyCode == Keys.F5)
                button3_Click(null, null);
            if (e.KeyCode == Keys.F6)
                button8_Click(null, null);
            if (e.KeyCode == Keys.F3)
                button9_Click(null, null);

        }

        //清除测试
        private void button8_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                dataGridView2.Rows[i].Cells[0].Value = null;
            }

        }

        //跟随
        private void button9_Click(object sender, EventArgs e)
        {
            switch (button9.BackColor.R)
            {
                case 240:
                    button9.BackColor = Color.Green;
                    break;
                case 0:
                    button9.BackColor = SystemColors.Control;
                    break;

            }
        }

        //测试表格滚动
        private void dataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
            if (button9.BackColor == Color.Green)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView2.FirstDisplayedScrollingRowIndex;
                dataGridView1.HorizontalScrollingOffset = dataGridView2.HorizontalScrollingOffset;
            }
        }

        //单词表格滚动
        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (button9.BackColor == Color.Green)
            {
                dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView1.FirstDisplayedScrollingRowIndex;
                dataGridView2.HorizontalScrollingOffset = dataGridView1.HorizontalScrollingOffset;
            }
        }

        //回车生成
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2_Click(null, null);
                e.SuppressKeyPress = true;
            }
        }

    }
}
