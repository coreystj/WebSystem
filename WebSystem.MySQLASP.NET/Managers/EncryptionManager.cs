using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebSystem.MySQLASP.NET.Managers
{
    public static class EncryptionManager
    {
        private const string ENCRYPTION_KEY = "AK45Sjdll3AsveJkopRT6";

        public static bool ValidatePassword(string password, string salt, string hash)
        {
            return (PasswordEncrypt(password + salt) == hash) ? true : false;
        }

        public static void ValidateApiKey(this HttpRequestMessage request, out int userId)
        {
            try
            {
                string token = request.Get<string>("API_KEY");
                bool validated = ValidateToken(token, out userId);
                if(!validated)
                    throw new Exception();
            }
            catch(Exception)
            {
                throw new Exception("Your api key is invalid.");
            }
        }

        public static bool ValidateToken(string token, out int userId)
        {
            try
            {
                string[] arguments = Decrypt(token).Split(':');
                userId = int.Parse(arguments[0]);
                long ticks = long.Parse(arguments[1]);
                DateTime time = new DateTime(ticks);

                TimeSpan elapsed = DateTime.Now - time;
                if (elapsed.TotalHours < 24)
                {
                    return true;
                }
                else
                {
                    userId = -1;
                    return false;
                }
            }
            catch (Exception) { }
            userId = -1;
            return false;
        }

        public static string GetToken(int userId)
        {
            return Encrypt(userId + ":" + DateTime.Now.Ticks);
        }

        public static string PasswordEncrypt(this string passwordKey)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(passwordKey));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();
            // Get the key from config file

            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(ENCRYPTION_KEY));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            System.Configuration.AppSettingsReader settingsReader =
                                                new AppSettingsReader();

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(ENCRYPTION_KEY));
            //release any resource held by the MD5CryptoServiceProvider

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}