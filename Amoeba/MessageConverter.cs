﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Amoeba.Properties;
using Library.Net.Amoeba;
using Library.Security;
using System.Security.Cryptography;

namespace Amoeba
{
    static class MessageConverter
    {
        public static string ToKeywordsString(IEnumerable<Keyword> keywords)
        {
            return String.Join(", ", keywords.Select(n => n.Value));
        }

        public static string ToSignatureString(DigitalSignature digitalSignature)
        {
            if (digitalSignature == null || digitalSignature.PublicKey == null) return null;

            try
            {
                using (var sha512 = new SHA512Managed())
                {
                    return Convert.ToBase64String(sha512.ComputeHash(digitalSignature.PublicKey).ToArray());
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("ArgumentException", e);
            }
        }

        public static string ToSignatureString(Certificate certificate)
        {
            if (certificate == null || certificate.PublicKey == null) return null;

            try
            {
                using (var sha512 = new SHA512Managed())
                {
                    return Convert.ToBase64String(sha512.ComputeHash(certificate.PublicKey).ToArray());
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("ArgumentException", e);
            }
        }

        public static string ToInfoMessage(Seed seed)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                if (seed.Name != null) builder.AppendLine(string.Format("{0}: {1}", LanguagesManager.Instance.SearchControl_Name, seed.Name));
                builder.AppendLine(string.Format("{0}: {1}", LanguagesManager.Instance.SearchControl_Signature, MessageConverter.ToSignatureString(seed.Certificate)));
                builder.AppendLine(string.Format("{0}: {1:#,0}", LanguagesManager.Instance.SearchControl_Length, seed.Length));
                if (seed.Keywords.Count != 0) builder.AppendLine(string.Format("{0}: {1}", LanguagesManager.Instance.SearchControl_Keywords, String.Join(", ", seed.Keywords.Select(n => n.Value))));
                builder.AppendLine(string.Format("{0}: {1}", LanguagesManager.Instance.SearchControl_CreationTime, seed.CreationTime.ToString("yyyy/MM/dd HH:mm:ss")));
                if (seed.Comment != null) builder.AppendLine(string.Format("{0}: {1}", LanguagesManager.Instance.SearchControl_Comment, seed.Comment));

                if (builder.Length != 0) return builder.ToString().Remove(builder.Length - 2);
                else return null;
            }
            catch (Exception e)
            {
                throw new ArgumentException("ArgumentException", e);
            }
        }

        public static string ToInfoMessage(Box box)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                if (box.Name != null) builder.AppendLine(string.Format("{0}: {1}", LanguagesManager.Instance.SearchControl_Name, box.Name));
                builder.AppendLine(string.Format("{0}: {1}", LanguagesManager.Instance.SearchControl_Signature, MessageConverter.ToSignatureString(box.Certificate)));
                builder.AppendLine(string.Format("{0}: {1}", LanguagesManager.Instance.SearchControl_CreationTime, box.CreationTime.ToString("yyyy/MM/dd HH:mm:ss")));
                if (box.Comment != null) builder.AppendLine(string.Format("{0}: {1}", LanguagesManager.Instance.SearchControl_Comment, box.Comment));

                if (builder.Length != 0) return builder.ToString().Remove(builder.Length - 2);
                else return null;
            }
            catch (Exception e)
            {
                throw new ArgumentException("ArgumentException", e);
            }
        }
    }
}