﻿using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace WebUI.Helpers
{
    public class Services
    {
        public static void SendMessage(string[] _param, string subject, string message, string messageTo)
        {
            //SmtpClient Smtp = new SmtpClient("smtp.yandex.ru", 25);
            //SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 587);
            SmtpClient Smtp = new SmtpClient(_param[3], Int32.Parse(_param[4]));
            //Smtp.Credentials = new NetworkCredential("aleksandr.st.novikov@gmail.com", "Hf,jnf_0035");
            Smtp.Credentials = new NetworkCredential(_param[0], _param[2]);
            Smtp.EnableSsl = _param[5] == "1" ? true : false;
            using (MailMessage Message = new MailMessage())
            {
                Message.From = new MailAddress(_param[0], _param[1]);

                string[] listTo = messageTo.Split(',');
                foreach (string lt in listTo)
                {
                    Message.To.Add(new MailAddress(lt.Trim()));
                }

                Message.Subject = subject;
                Message.IsBodyHtml = true;
                Message.Body = message;
                
                Smtp.Send(Message);
            }
        }
    }
}