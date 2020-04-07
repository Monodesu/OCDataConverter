using System;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;


namespace OCDataConverter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query, RDBServer, RDBPort, RDBUser, RDBPassword, RDBDatabase;

            RDBServer = "Your Host";
            RDBPort = "Your Host Port";
            RDBUser = "Your Username";
            RDBPassword = "Your Password";
            RDBDatabase = "Your Database";

            int recordcount = 0;
            label2.Text = "record count: " + recordcount.ToString();
            listBox1.Items.Clear();
            listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Working.");
            listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
            button1.Enabled = false;

            MySqlConnection RemoteDB = new MySqlConnection("Host=" + RDBServer + ";port=" + RDBPort +
            ";User ID=" + RDBUser + ";password=" + RDBPassword + ";database=" + RDBDatabase + "");

            listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Connecting to OC remote database...");
            listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
            try
            {
                RemoteDB.Open();
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Successfully connected to OC remote database.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                DirectoryInfo OCUserPath = new DirectoryInfo(pathLabel.Text);
                foreach (FileInfo NextFile in OCUserPath.GetFiles())
                {
                    listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Reading File:" + NextFile.Name);
                    listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                    string UID = INIHelper.INIHelper.Read("record", "uid", "notlinked", pathLabel.Text + "\\" + NextFile.Name);
                    string MainMode = INIHelper.INIHelper.Read("record", "mainmode", "0", pathLabel.Text + "\\" + NextFile.Name);
                    string UserName = INIHelper.INIHelper.Read("record", "username", "notlinked", pathLabel.Text + "\\" + NextFile.Name);
                    string QQ = NextFile.Name.Substring(0, NextFile.Name.Length - 4);
                    listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() +
                        "] UID:" + UID + "  Username:" + UserName + "  QQ:" + QQ + "  MainMode:" + MainMode);
                    listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                    listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Pushing User " + UID + " ...");
                    listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                    query = "INSERT INTO info (uid, qq, mainmode) VALUES (" + UID + ", " + QQ + ", " + MainMode + ")";
                    MySqlCommand CMD = new MySqlCommand(query, RemoteDB);
                    CMD.ExecuteNonQuery();
                    listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Done..");
                    listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                    recordcount = recordcount + 1;
                    label2.Text = "record count: " + recordcount.ToString();
                    listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] " + recordcount.ToString() + " records have been exported.");
                    listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                }
                RemoteDB.Close();
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Your request has successfully submitted.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] The database connection has closed.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                MessageBox.Show("Your request has successfully submitted.", "successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button1.Enabled = true;
            }
            catch (MySqlException err)
            {
                RemoteDB.Close();
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] The database connection has closed.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] An error has occurred during your session.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Details: " + err.Message);
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                MessageBox.Show("An error has occurred during your session.\r\nSee the log for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
            }
            catch (Exception err)
            {
                RemoteDB.Close();
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] The database connection has closed.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] An error has occurred during your session.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Details: " + err.Message);
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                MessageBox.Show("An error has occurred during your session.\r\nSee the log for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "Choose a folder.";
            if (dilog.ShowDialog() == DialogResult.OK || dilog.ShowDialog() == DialogResult.Yes)
            {
                pathLabel.Text = dilog.SelectedPath;
                DirectoryInfo OCUserPath = new DirectoryInfo(pathLabel.Text);
                listBox1.Items.Clear();
                listBox1.Items.Add("List of this folder:");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("*********************");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                foreach (FileInfo NextFile in OCUserPath.GetFiles())
                {
                    listBox1.Items.Add(NextFile.Name);
                }
                listBox1.Items.Add(" ");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("Subfolder of this folder:");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("****************************");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                foreach (DirectoryInfo NextFolder in OCUserPath.GetDirectories())
                {
                    listBox1.Items.Add(NextFolder.Name);
                    listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                }
                listBox1.Items.Add(" ");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] If this is OK then start convert.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int recordcount = 0;
            label2.Text = "record count: " + recordcount.ToString();
            listBox1.Items.Clear();
            listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Working.");
            listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
            button3.Enabled = false;
            try
            {
                DirectoryInfo OCUserPath = new DirectoryInfo(pathLabel.Text);
                foreach (FileInfo NextFile in OCUserPath.GetFiles())
                {
                    listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Reading File:" + NextFile.Name);
                    listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                    string UID = INIHelper.INIHelper.Read("record", "uid", "notlinked", pathLabel.Text + "\\" + NextFile.Name);
                    string MainMode = INIHelper.INIHelper.Read("record", "mainmode", "0", pathLabel.Text + "\\" + NextFile.Name);
                    string UserName = INIHelper.INIHelper.Read("record", "username", "notlinked", pathLabel.Text + "\\" + NextFile.Name);
                    string QQ = NextFile.Name.Substring(0, NextFile.Name.Length - 4);
                    listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() +
                        "] UID:" + UID + "  Username:" + UserName + "  QQ:" + QQ + "  MainMode:" + MainMode);
                    listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                    recordcount = recordcount + 1;
                    label2.Text = "record count: " + recordcount.ToString();
                    listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] " + recordcount.ToString() + " records have been exported.");
                    listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                }
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Your request has successfully submitted.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                MessageBox.Show("Your request has successfully submitted.", "successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button3.Enabled = true;
            }
            catch (MySqlException err)
            {
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] The database connection has closed.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] An error has occurred during your session.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Details: " + err.Message);
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                MessageBox.Show("An error has occurred during your session.\r\nSee the log for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
            }
            catch (Exception err)
            {
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] The database connection has closed.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] An error has occurred during your session.");
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                listBox1.Items.Add("[" + DateTime.Now.ToLongTimeString().ToString() + "] Details: " + err.Message);
                listBox1.TopIndex = listBox1.Items.Count - (listBox1.Height / listBox1.ItemHeight);
                MessageBox.Show("An error has occurred during your session.\r\nSee the log for more details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Enabled = true;
            }
        }
    }
}
