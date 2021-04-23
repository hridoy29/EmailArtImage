using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Net.Mail;
using System.IO;

using System.Globalization;
using System.Windows.Forms;
using System.Net.Mime;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data.SqlClient;
using IronOcr;

namespace Art
{
    public class FarewellMail_Setup
    {

        #region <VARIABLE & INSTANCES>
        //SmtpClient iSmtpClient = new SmtpClient("mail.dhakaregency.com");

        Attachment iAttachment = null;
        // MailAddress iMailAddress = new MailAddress("MIS<mis@dhakaregency.com>");
        MailMessage iMailMessage = new MailMessage();

        public string glbErrorMessage = "";
        #endregion

        #region <CUSTOME METHODS>
        /// <summary>
        /// use for attaching any type of documnet,image and html file.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="emailId"></param>
        public void autoWelcomeMail(string userName, string emailId, int position, string subject,string imageAddress,string attachFile)
        {
            try
            {
                //------Email Information-----
                iMailMessage.IsBodyHtml = true;
                string SmtpClintName = string.Empty;
                string Body = string.Empty;
                string EmailId = string.Empty;
                string Password = string.Empty;
                string contentMailAddress = string.Empty;
                string contentSubject = string.Empty;
                iMailMessage.CC.Clear();
                iMailMessage.To.Clear();
                iMailMessage.Attachments.Clear();
                iMailMessage.To.Add(emailId);
                iMailMessage.Bcc.Add("dhakaregencymarketing@gmail.com".Trim());
                
                SmtpClintName = "mail.test.com".Trim();
                EmailId = "jubair@test.com".Trim();
                Password = "123456";
                contentMailAddress = "DHAKA REGENCY HOTEL & RESORT<marketing@dhakaregency.com>".Trim();
                contentSubject = "Feel Proud To Be Parent of STAR ARTIST!";//subject.Trim();


                MailAddress iMailAddress = new MailAddress(contentMailAddress);
                SmtpClient iSmtpClient = new SmtpClient(SmtpClintName);

                iSmtpClient.Host = "169.60.229.215".Trim();


                iSmtpClient.UseDefaultCredentials = false;
                iSmtpClient.Port = 25;
                iSmtpClient.Credentials = new System.Net.NetworkCredential(EmailId, Password);

                //-----For Reading Subject From The text File-----.
                iMailMessage.From = iMailAddress;
                iMailMessage.Subject = contentSubject;
                iMailMessage.Body = " ";

                
                //imPath = Application.StartupPath + @"\JPEG\";
                //DirectoryInfo directoryInfo = new DirectoryInfo(imageAddress);
                //FileInfo[] fileInfo = directoryInfo.GetFiles();
                //ArrayList arrayList = new ArrayList();
                //Attachment attachment = new Attachment(attachFile);
                //arrayList.Add(fileInfo[position]);
                //AlternateView view = AlternateView.CreateAlternateViewFromString("Test", null, MediaTypeNames.Image.Jpeg);
                
                try
                {
                    //-----Add view to the Email Message-----
                    //iMailMessage.AlternateViews.Add(view);
                    
                    Attachment attachment = new Attachment(attachFile);
                    iMailMessage.Attachments.Add(attachment);
                    iMailMessage.IsBodyHtml = false;
                    iSmtpClient.Send(iMailMessage);
                
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                   // File.Delete(attachFile);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        #endregion
        public bool IsValidMailAddress(string emailId)
        {
            try
            {
                var addr = new MailAddress(emailId);
                return addr.Address == emailId;
            }
            catch
            {
                return false;
            }
        }

    }
}
