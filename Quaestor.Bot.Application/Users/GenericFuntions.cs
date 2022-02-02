using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Quaestor.Bot.Users
{
    public static class GenericFuntions
    {
        private static readonly byte[] salt = Encoding.ASCII.GetBytes("Ent3r your oWn S@lt v@lu# h#r3");
        private static readonly string encryptionPassword = "Qu@est0rC0reBOTUI#2019#";

        static string UserToken = "QBUser";

        //public static string MainUrlpath = string.Format("{0}://{1}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Headers["host"]);
        public static string MainUrlpath = "http://quaestor-bot-ui.azurewebsites.net";

        public static readonly string CoinbaseQuaestorAuth = "Q_u_a_e_s_t_o_r_2018";

        public static string Encrypt(string textToEncrypt)
        {
            var algorithm = GetAlgorithm(encryptionPassword);

            byte[] encryptedBytes;
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV))
            {
                byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);
                encryptedBytes = InMemoryCrypt(bytesToEncrypt, encryptor);
            }
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string encryptedText)
        {
            var algorithm = GetAlgorithm(encryptionPassword);

            byte[] descryptedBytes;
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV))
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                descryptedBytes = InMemoryCrypt(encryptedBytes, decryptor);
            }
            return Encoding.UTF8.GetString(descryptedBytes);
        }

        // Performs an in-memory encrypt/decrypt transformation on a byte array.
        private static byte[] InMemoryCrypt(byte[] data, ICryptoTransform transform)
        {
            MemoryStream memory = new MemoryStream();
            using (Stream stream = new CryptoStream(memory, transform, CryptoStreamMode.Write))
            {
                stream.Write(data, 0, data.Length);
            }
            return memory.ToArray();
        }        

        private static RijndaelManaged GetAlgorithm(string encryptionPassword)
        {
            // Create an encryption key from the encryptionPassword and salt.
            var key = new Rfc2898DeriveBytes(encryptionPassword, salt);

            // Declare that we are going to use the Rijndael algorithm with the key that we've just got.
            var algorithm = new RijndaelManaged();
            int bytesForKey = algorithm.KeySize / 8;
            int bytesForIV = algorithm.BlockSize / 8;
            algorithm.Key = key.GetBytes(bytesForKey);
            algorithm.IV = key.GetBytes(bytesForIV);
            return algorithm;
        }

        public static string ReadHtmlPage(string PageName, string MainURL)
        {
            string sHTML = "";
            try
            {
                string inputPath = MainURL + @"\Html\" + PageName + ".html";           
                sHTML = System.IO.File.ReadAllText(inputPath);                
            }
            catch (Exception ex)
            {
            }
            return sHTML;
        }


        public static Task<bool> SendEmail(string subject, string body, string to)
        {
            var tcs = new TaskCompletionSource<bool>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("accounts@quaestor.io");
            mail.To.Add(new MailAddress(to));
            SmtpClient client = new SmtpClient();
            //client.Host = "smtp.gmail.com";
            client.Host = "asmtp.unoeuro.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("accounts@quaestor.io", "Quaestor2019");
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            try
            {
                client.Send(mail);
                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                return tcs.Task;
            }
            return tcs.Task;
        }
        public static Task<bool> SendEmailWithCC(string subject, string body, List<string> to)
        {
            var tcs = new TaskCompletionSource<bool>();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("accounts@quaestor.io");
            for (int tomail = 0; tomail < to.Count; tomail++)
            {
                if (tomail == 0)
                {
                    mail.To.Add(new MailAddress(to[tomail]));
                }
                else
                {
                    mail.CC.Add(new MailAddress(to[tomail]));
                }
            }
            SmtpClient client = new SmtpClient();
            //client.Host = "smtp.gmail.com";
            client.Host = "asmtp.unoeuro.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new System.Net.NetworkCredential("accounts@quaestor.io", "Quaestor2019");
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            try
            {
                client.Send(mail);
                tcs.SetResult(true);
            }
            catch (Exception ex)
            {
                return tcs.Task;
            }
            return tcs.Task;
        }

    }
}
