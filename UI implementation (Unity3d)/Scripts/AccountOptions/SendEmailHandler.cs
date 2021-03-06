/**
Script used to send emails to a specific address.
 License type: cc-by-sa 
https://creativecommons.org/licenses/by-sa/2.5/
DIT029 H16 Project: Software Architecture for Distributed Systems
University of Gothenburg, Sweden 2016

This file is part of "Run Doris Run!" game. It has been modified according to
a file found on StackOverflow. All credits go to stackOverflow as the owner has not
assumed any specific authorship. According to StackOverflow, all files posted
before the 1st of Feb. 2016 own a CC-BY-SA license type, thus, this file 
suits under the terms of the CC-BY-SA license.

"Run Doris Run!" game is free software: you can redistribute it and/or modify
it under the terms of the CC-BY-SA license as published by
the Free Software Foundation.

"Run Doris Run!" game is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/


using System;
using System.Net;
using System.Net.Mail;
using UnityEngine;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


public class SendEmailHandler
{

	public static void sendEmail(string email, string tempPass)
		{
		try
		{
			MailMessage mail = new MailMessage();
			SmtpClient smtpC = new SmtpClient("smtp.gmail.com");

			//From address to send email
			mail.From = new MailAddress("rundorisrun.thegame@gmail.com");
			//To address to send email
			mail.To.Add(email);
			mail.Body = "You have recently requested to change your \"Run Doris Run!\" password. Please find the new password below. \n" + tempPass  + 
				         "\n  Best Regards, \n Run Doris, Run! Team";
			mail.Subject = "[no-reply] New Password";
			smtpC.Port = 587;
			//Credentials for From address
			smtpC.Credentials =(System.Net.ICredentialsByHost) new System.Net.NetworkCredential("rundorisrun.thegame@gmail.com", "potatoeseverywhere");
			smtpC.EnableSsl = true;
			ServicePointManager.ServerCertificateValidationCallback = 
				delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
			{ return true; };
			smtpC.Send(mail);
			//Change Console.Writeline to Debug.Log 
			Debug.Log ("Message sent successfully");
		}
		catch (Exception e)
		{
			Debug.Log(e.GetBaseException());
			//You don't need or use Console.ReadLine();
		}


	}
}
	

