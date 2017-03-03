using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EntityCodeGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string str = txtConnectionStr.Text.Trim();
                Match m = Regex.Match(str, @"\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}");//检查是否字符串中有IP

                if (!m.Success)
                {
                    MessageBox.Show("连接字符串缺少IP");
                    return;
                }
                DBHelper.ConnectionString = str;
                int Port = 1433;
                if (Regex.IsMatch(str, @"\,\d+"))
                {
                    Port =Convert.ToInt32(Regex.Match(str, @"\,(\d+)").Groups[1].Value);
                }
                if (DBHelper.TryConnect(m.ToString(), Port))
                {
                
                    btnConnect.Enabled = false;
                    btnDisconect.Enabled = true;
                    return;
                }
                MessageBox.Show("连接数据库失败.");
            }
            catch (Exception ex)
            {
                btnConnect.Enabled = true;
                btnDisconect.Enabled = false;
                MessageBox.Show(ex.Message);
            }
        }



        private void btnDisconect_Click(object sender, EventArgs e)
        {
            DBHelper.ConnectionString = string.Empty;
            btnConnect.Enabled = true;
            btnDisconect.Enabled = false;
            txtNameSpace.Text = string.Empty;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtNameSpace.Text.Trim()))
            {
                MessageBox.Show("命名空间不能为空.");
                return;
            }
            string connectionString=DBHelper.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                connectionString = txtConnectionStr.Text.Trim();
            }
            EngineerCodeFirstHandler codeEngineer = new EngineerCodeFirstHandler();
            codeEngineer.CodeGenerator(connectionString, txtNameSpace.Text.Trim(), ShowMsg);
        }


        private void ShowMsg(string msgContent)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(delegate { MessageBox.Show(msgContent); }));
            }
            else
            {
                MessageBox.Show(msgContent);
            }
        }

        public void GenerateFile(string path, string fileName, string content)
        {
            if (Directory.Exists(path))
            {
                string fullName = Path.Combine(path, fileName);
                using (StreamWriter writer = new StreamWriter(fullName, false))
                {
                    writer.WriteLine(content);
                    writer.Flush();
                }
            }
            else
            {
                MessageBox.Show("文件夹不存在");
            }
        }
    }
}
