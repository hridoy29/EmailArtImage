using Aspose.PSD.FileFormats.Psd;
using Aspose.PSD.FileFormats.Psd.Layers;
using IronOcr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Art
{
    public partial class Art : Form
    {
        string mailAddress = string.Empty;
        string userName = string.Empty;
        string slNo = string.Empty;
        string fileLocation = string.Empty;
        int i = 0;
        FarewellMail_Setup _FarewellMail_Setup = new FarewellMail_Setup();
        //SmtpClient iSmtpClient = new SmtpClient("mail.dhakaregency.com");
        //Attachment iAttachment = null;
        //MailAddress iMailAddress = new MailAddress("MIS<mis@dhakaregency.com>");
        //MailMessage iMailMessage = new MailMessage();
        public Art()
        {


            InitializeComponent();
        }


        private void convertPsdToGif()
        {
            // Load PSD file
            using (PsdImage image =(PsdImage) PsdImage.Load (@"C:\Users\administrator\Desktop\Certificate Kids art.psd"))
            {
                // Find Layer using layer's name
                var layerToUpdateText = (TextLayer)FindLayer("Name", image);
                // Simple way to update text
                layerToUpdateText.UpdateText("John Doe");
                // Save the updated PSD file
                image.Save("updated-psd.psd");
            }
            //-------------------FindLayer()-------------
            
        }
        public static Layer FindLayer(string layerName, PsdImage image)
        {
            // Get aa layers in PSD file
            var layers = image.Layers;
            // Find desired layer
            foreach (var layer in layers)
            {
                // Match layer's name
                if (string.Equals(layer.DisplayName, layerName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return layer;
                }
            }
            return null;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {

            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "nameSelect";
            checkColumn.HeaderText = "Select";
            checkColumn.Width = 50;
            checkColumn.ReadOnly = false;
            checkColumn.FillWeight = 10; //if the datagridview is resized (on form resize) the checkbox won't take up too much; value is relative to the other columns' fill values
            dataGridView1.Columns.Add(checkColumn);
            String name = "Sheet1";

            String constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + txtXlFile.Text+
                            //"C:\\Users\\administrator\\Desktop\\EventRegistration.xlsx" +
                            ";Extended Properties='Excel 12.0 XML;HDR=YES;';";

            OleDbConnection con = new OleDbConnection(constr);
            OleDbCommand oconn = new OleDbCommand("Select * From [" + name + "$]", con);
            con.Open();

            OleDbDataAdapter sda = new OleDbDataAdapter(oconn);
            DataTable data = new DataTable();
            sda.Fill(data);

            dataGridView1.DataSource = data;
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[0];
                chk.Value = !(chk.Value == null ? false : (bool)chk.Value);
            }
        }

      

        private void button2_Click(object sender, EventArgs e)
        {
           
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)item.Cells[0];
                if (chk.Value != null)
                {
                    if ((bool)chk.Value == true)
                    {
                        this.slNo = item.Cells[1].Value.ToString();
                        this.mailAddress = item.Cells[3].Value.ToString();
                        this.userName= item.Cells[2].Value.ToString();
                        DirectoryInfo info = new DirectoryInfo(txtDirectoryPath.Text);
                        
                         string[] filesDirectory = Directory.GetFiles(txtDirectoryPath.Text, "*.jpg", SearchOption.AllDirectories);

                        FileInfo[] files = info.GetFiles().OrderBy(p => p.Name).ToArray();
                        int imageExtentionIndex = files[i].Name.IndexOf(".");
                        string imageName = files[i].Name.Substring(0, imageExtentionIndex);
                        //  foreach (string afile in files)
                        {
                            //string myFile = txtDirectoryPath.Text + "\\"+  afile.Trim();
                            // var Result = new IronTesseract().Read(afile);
                            // if(Result.Text.Contains(userName))
                            if (imageName==slNo)
                            {
                                {

                                    /*--------------Auto Welcome Mail Code For Customer (Jubair)-----------*/

                                    bool Isconnected = false;
                                    bool IsValidEmail = false;
                                    try
                                    {
                                        using (var client = new WebClient())

                                        using (client.OpenRead("http://clients3.google.com/generate_204"))
                                        {
                                            Isconnected = true;

                                        }
                                        if (mailAddress != null && Isconnected == true)
                                        {
                                            IsValidEmail = _FarewellMail_Setup.IsValidMailAddress(mailAddress.Trim());
                                            if (IsValidEmail)
                                            {
                                               
                                                _FarewellMail_Setup.autoWelcomeMail(userName, mailAddress, i, txtSubject.Text, txtDirectoryPath.Text, filesDirectory[i]);
                                                chk.Value = false;

                                                 
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        
                                        MessageBox.Show(ex.Message);
                                        break;
                                    }
                                }
                                
                            }
                        }
                    }
                }
                i++;

            }
             MessageBox.Show("Task Complited");
        }

        private void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.jpg;*.gif;*.bmp;*.png;*.jpeg)|*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName.ToString();
               
                txtDirectoryPath.Text =Path.GetDirectoryName(path) ;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "EXCEL Files(*.xlsx;*.xlsm;*.xlsb;*.xltx;*.xltm;*.xls;*.xlt)|*.xlsx;*.xlsm;*.xlsb;*.xltx;*.xltm;*.xls;*.xlt|Image Files(*.jpg;*.gif;*.bmp;*.png;*.jpeg)|*.jpg;*.gif;*.bmp;*.png;*.jpeg|All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string path = ofd.FileName.ToString();

                txtXlFile.Text = path;
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            convertPsdToGif();
            //RenameFiles();
        }
        private void RenameFiles()
        {
            string[] files =
                   Directory.GetFiles(txtDirectoryPath.Text, "*.jpg", SearchOption.AllDirectories);


            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)item.Cells[0];
                if (chk.Value != null)
                {
                    if ((bool)chk.Value == true)
                    {
                        this.slNo = item.Cells[1].Value.ToString();
                        this.mailAddress = item.Cells[3].Value.ToString();
                        this.userName = item.Cells[2].Value.ToString();
                        DirectoryInfo info = new DirectoryInfo(txtDirectoryPath.Text);
                        //FileInfo[] files = info.GetFiles().OrderBy(p => p.Name).ToArray();
                        //int imageExtentionIndex = files[i].Name.IndexOf(".");
                        //string imageName = files[i].Name.Substring(0, imageExtentionIndex);
                      

                        foreach (string afile in files)
                        {
                            string myFile = txtDirectoryPath.Text + "\\" + afile.Trim();
                            var Result = new IronTesseract().Read(afile);
                            if (Result.Text.Contains(userName))
                            {

                                System.IO.File.Move(afile, txtDirectoryPath.Text+ "\\Moved\\" + userName+".jpg");
                                return;
                            }

                        }

                    }
                }
                i++;

            }
            MessageBox.Show("Task Complited");
        }

       
    }
}
