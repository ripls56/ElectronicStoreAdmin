﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ElectronicStoreAdmin
{
    public class Utils
    {
        static RegistryKey registry = Registry.CurrentUser;
        public static string? RegGet(string textload)
        {
            RegistryKey key = registry.CreateSubKey("Electro");
            return key.GetValue(textload)?.ToString();
        }
        public static void RegSet(string keyname, string textsave)
        {
            RegistryKey key = registry.CreateSubKey("Electro");
            key.SetValue(keyname, textsave);
        }
        public static string? RegGet(string textload, string encryptKey = "key123")
        {
            RegistryKey key = registry.CreateSubKey("Electro");
            textload = key.GetValue(textload)?.ToString();
            return Decrypt(textload, encryptKey);
        }
        public static void RegSet(string keyname, string textsave, string encryptKey = "key123")
        {
            RegistryKey key = registry.CreateSubKey("Electro");
            textsave = Encrypt(textsave, encryptKey);
            key.SetValue(keyname, textsave);
        }

        private static string Encrypt(string ishText, string pass, string sol = "doberman", string cryptographicAlgorithm = "SHA1", int passIter = 2, string initVec = "a8doSuDitOz1hZe#", int keySize = 256)
        {
            if (string.IsNullOrEmpty(ishText)) return "";

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] ishTextB = Encoding.UTF8.GetBytes(ishText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);
            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] cipherTextBytes = null;

            using (ICryptoTransform encryptor = symmK.CreateEncryptor(keyBytes, initVecB))
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(ishTextB, 0, ishTextB.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memStream.ToArray();
                        memStream.Close();
                        cryptoStream.Close();
                    }
                }
            }
            symmK.Clear();
            return Convert.ToBase64String(cipherTextBytes);
        }

        private static string? Decrypt(string ciphText, string pass, string sol = "doberman", string cryptographicAlgorithm = "SHA1", int passIter = 2, string initVec = "a8doSuDitOz1hZe#", int keySize = 256)
        {
            if (string.IsNullOrEmpty(ciphText)) return null;

            byte[] initVecB = Encoding.ASCII.GetBytes(initVec);
            byte[] solB = Encoding.ASCII.GetBytes(sol);
            byte[] cipherTextBytes = Convert.FromBase64String(ciphText);

            PasswordDeriveBytes derivPass = new PasswordDeriveBytes(pass, solB, cryptographicAlgorithm, passIter);
            byte[] keyBytes = derivPass.GetBytes(keySize / 8);

            RijndaelManaged symmK = new RijndaelManaged();
            symmK.Mode = CipherMode.CBC;

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int byteCount = 0;

            using (ICryptoTransform decryptor = symmK.CreateDecryptor(keyBytes, initVecB))
            {
                using (MemoryStream mSt = new MemoryStream(cipherTextBytes))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mSt, decryptor, CryptoStreamMode.Read))
                    {
                        byteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                        mSt.Close();
                        cryptoStream.Close();
                    }
                }
            }
            symmK.Clear();
            return Encoding.UTF8.GetString(plainTextBytes, 0, byteCount);
        }
    }
}
