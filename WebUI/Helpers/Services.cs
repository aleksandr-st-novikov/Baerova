﻿using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace WebUI.Helpers
{
    public class Services
    {
        public static void SendMessage(string[] _param, string subject, string message, string messageTo, string messageCC = "")
        {
            try
            {
                System.Threading.Thread.Sleep(3000);
                //Task.Delay(3000);

                //SmtpClient Smtp = new SmtpClient("smtp.yandex.ru", 25);
                //SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 587);
                SmtpClient Smtp = new SmtpClient(_param[3], Int32.Parse(_param[4]));
                Smtp.UseDefaultCredentials = false;
                Smtp.Credentials = new NetworkCredential(_param[0], _param[2]);
                //Smtp.Credentials = _param[2] == "12345" ? new NetworkCredential(_param[0], "Hf,jnf_0035") : new NetworkCredential(_param[0], _param[2]);
                Smtp.EnableSsl = _param[5] == "1" ? true : false;
                
                using (MailMessage Message = new MailMessage())
                {
                    Message.From = new MailAddress(_param[0], _param[1]);

                    string[] listTo = messageTo.Split(',');
                    foreach (string lt in listTo)
                    {
                        Message.To.Add(new MailAddress(lt.Trim()));
                    }

                    if (messageCC != "")
                    {
                        string[] listCC = messageCC.Split(',');
                        foreach (string lcc in listCC)
                        {
                            Message.CC.Add(new MailAddress(lcc.Trim()));
                        }
                    }

                    Message.Subject = subject;
                    Message.IsBodyHtml = true;
                    Message.Body = message;

                    Smtp.Send(Message);
                }
            }
            catch(Exception ex)
            { }
        }


        /// <summary>
        /// Converts a datetime value to w3c format
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ConvertDateToW3CTime(DateTime date)
        {
            //Get the utc offset from the date value
            var utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(date);
            string w3CTime = date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
            //append the offset e.g. z=0, add 1 hour is +01:00
            w3CTime += utcOffset == TimeSpan.Zero ? "Z" :
                String.Format("{0}{1:00}:{2:00}", (utcOffset > TimeSpan.Zero ? "+" : "-")
                , utcOffset.Hours, utcOffset.Minutes);

            return w3CTime;
        }
    }
}