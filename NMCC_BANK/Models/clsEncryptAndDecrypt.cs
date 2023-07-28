using System;
using System.Text;

namespace INDO_FIN_NET.Models
{
    public class clsEncryptAndDecrypt
    {
        //Encryption Key
        private byte[] EncryptionKey { get; set; }
        // The Initialization Vector for the DES encryption routine
        private byte[] IV { get; set; }

        /// Constructor for TripleDESImplementation class
        /// </summary>
        /// <param name="encryptionKey">The 24-byte encryption key (24 character ASCII)</param>
        /// <param name="IV">The 8-byte DES encryption initialization vector (8 characters ASCII)</param>
        //public clsEncryptAndDecrypt()
        //{
        //    string encryptionKey = "jANBgkqhkiG9w0BAQEFAAOCA";
        //    string IV = "27c54a8f";

        //    if (string.IsNullOrEmpty(encryptionKey))
        //    {
        //        throw new ArgumentNullException("'encryptionKey' parameter cannot be null.", "encryptionKey");
        //    }
        //    if (string.IsNullOrEmpty(IV))
        //    {
        //        throw new ArgumentException("'IV' parameter cannot be null or empty.", "IV");
        //    }
        //    EncryptionKey = Encoding.ASCII.GetBytes(encryptionKey);
        //    // Ensures length of 24 for encryption key
        //    //  Trace.Assert(EncryptionKey.Length == 24, "Encryption key must be exactly 24 characters of ASCII text (24 bytes)");
        //    this.IV = Encoding.ASCII.GetBytes(IV);
        //    // Ensures length of 8 for init. vector
        //    // Trace.Assert(IV.Length == 8, "Init. vector must be exactly 8 characters of ASCII text (8 bytes)");
        //}

        ///// Encrypts a text block
        public string Encrypt(string textToEncrypt)
        {
            //if (textToEncrypt != null && textToEncrypt != "")
            //{
            //    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //    tdes.Key = EncryptionKey;
            //    tdes.IV = IV;
            //    byte[] buffer = Encoding.ASCII.GetBytes(textToEncrypt);
            //    return Convert.ToBase64String(tdes.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length));
            //}
            //else
            //{
            //    return textToEncrypt;
            //}
            return textToEncrypt;
        }

        ///// Decrypts an encrypted text block

        //public string Decrypt(string textToDecrypt)
        //{
        //    if (textToDecrypt != null && textToDecrypt != "")
        //    {
        //        try
        //        {
        //            byte[] buffer = Convert.FromBase64String(textToDecrypt);
        //            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        //            des.Key = EncryptionKey;
        //            des.IV = IV;
        //            return Encoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        //        }
        //        catch (Exception e)
        //        {
        //            return "Bad Data";
        //        }
        //    }
        //    else
        //    {
        //        return textToDecrypt;
        //    }
        //}

        //public string Decrypt1(string textToDecrypt)
        //{
        //    textToDecrypt = textToDecrypt.Replace(" ", "+");
        //    if (textToDecrypt != null && textToDecrypt != "")
        //    {
        //        try
        //        {
        //            byte[] buffer = Convert.FromBase64String(textToDecrypt);
        //            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        //            des.Key = EncryptionKey;
        //            des.IV = IV;
        //            return Encoding.ASCII.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
        //        }
        //        catch (Exception e)
        //        {
        //            return "Bad Data";
        //        }
        //    }
        //    else
        //    {
        //        return textToDecrypt;
        //    }
        //}
    }
}
