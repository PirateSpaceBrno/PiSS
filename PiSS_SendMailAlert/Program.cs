using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;
using PiSS;

namespace PiSS_SendMailAlert
{
    class Program
    {
        private const string Sender = "piratsky.space@gmail.com";
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SmtpClient = "piratsky.space@gmail.com";
        private const string SmtpPassword = "Z29yaWxhNzI=";

        private static string receiversFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\receivers.txt";
        private static Logger _logger = new Logger("PiSS_SendMailAlert");

        static void Main(string[] args)
        {
            foreach(string arg in args)
            {
                Console.WriteLine("PiSS SendMailAlert");
                Console.WriteLine("------------------");

                // odstraň '/' a '-' z argumentů
                var argSwitch = arg.ToLowerInvariant().Replace("/", "").Replace("-", "");

                switch (argSwitch)
                {
                    case "alarm":
                        SendEmail("!! DETEKOVÁN POHYB !!", $"PSB chodba, detekován pohyb na kameře.{Environment.NewLine}Kontrola je možná na http://piratsky.space:8087/");
                        return;

                    case "test":
                        SendEmail("TESTOVACÍ ZPRÁVA", "Testovací zpráva kamerového systému PSB.");
                        return;

                    case "serveroff":
                        SendEmail("SERVER ODPOJEN", "Kamerový systém odpojen.");
                        return;

                    case "serveron":
                        SendEmail("SERVER PŘIPOJEN", "Kamerový systém připojen.");
                        return;

                    case "list":
                        // vypíše seznam adres na obrazovku
                        Console.WriteLine("E-mail addresses in list:");
                        _logger.Log("List of e-mail addresses shown", Logger.logLevel.INFO);

                        foreach (string address in GetReceivers())
                        {
                            Console.WriteLine($"\t{address}");
                        }

                        break;

                    case "help":
                        _logger.Log("Help shown", Logger.logLevel.INFO);

                        Console.WriteLine("/add={name@mail.tld} or -add={name@mail.tld} | Add specified e-mail address to list.");
                        Console.WriteLine("/alarm or -alarm | Send \"motion detected\" message to all e-mail addresses in list.");
                        Console.WriteLine("/help or -help | Show this help guide.");
                        Console.WriteLine("/list or -list | Show all e-mail addresses in list.");
                        Console.WriteLine("/test or -test | Send \"test\" message to all e-mail addresses in list.");
                        Console.WriteLine("/remove={name@mail.tld} or -remove={name@mail.tld} | Remove specified e-mail address from list.");
                        Console.WriteLine("/serverOff or -serverOff | Send \"Server is down\" message to all e-mail addresses in list.");
                        Console.WriteLine("/serverOn or -serverOn | Send \"Server is up\" message to all e-mail addresses in list.");
                        Console.WriteLine();
                        Console.WriteLine("Do not combine arguments.");
                        break;

                    default:    
                        // přidá adresu na seznam
                        if(arg.Contains("add="))
                        {
                            try
                            {
                                string address = arg.Replace("add=", "").Replace("/", "").Replace("-", "");

                                using (StreamWriter w = File.AppendText(receiversFilePath))
                                {
                                    w.WriteLine(address);
                                }

                                Console.WriteLine($"Address [{address}] successfully added.");
                                _logger.Log($"Address '{address}' added", Logger.logLevel.INFO);
                            }
                            catch (Exception ex)
                            {
                                _logger.Log("Can't add address...", Logger.logLevel.FAIL, ex);
                                Console.WriteLine("Can't add address...");
                            }
                        }

                        // odebere adresu ze seznamu
                        else if(arg.Contains("remove="))
                        {
                            try
                            {
                                string addressToRemove = arg.Replace("remove=", "").Replace("/", "").Replace("-", "");
                                var addresses = GetReceivers().Except(new string[] { addressToRemove });

                                File.WriteAllLines(receiversFilePath, addresses);

                                Console.WriteLine($"Address [{addressToRemove}] successfully removed.");
                                _logger.Log($"Address '{addressToRemove}' removed", Logger.logLevel.INFO);
                            }
                            catch (Exception ex)
                            {
                                _logger.Log("Can't remove address...", Logger.logLevel.FAIL, ex);
                                Console.WriteLine("Can't remove address...");
                            }
                        }

                        break;
                }
            }
        }

        // načtení seznamu mailových adres
        private static IEnumerable<String> GetReceivers()
        {
            try
            {
                return File.ReadAllLines(receiversFilePath);
            }
            catch (Exception ex)
            {
                _logger.Log("Can't find file 'receivers.txt'", Logger.logLevel.FATAL, ex);
                throw new NullReferenceException("Can't find file 'receivers.txt'.");
            }
        }

        public static bool SendEmail(string subject, string msgBody)
        {
            try
            {
                // přihlášení se k smtp od google gmail
                var client = new SmtpClient(SmtpServer, SmtpPort)
                {
                    Credentials = new NetworkCredential(SmtpClient, Encoding.UTF8.GetString(Convert.FromBase64String(SmtpPassword))),
                    EnableSsl = true
                };

                var receivers = GetReceivers();
                var now = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                var message = $"{msgBody}{Environment.NewLine}{Environment.NewLine}{now}";

                Console.WriteLine($"{subject} : {message}{Environment.NewLine}");

                // odeslání emailu (od koho, komu, předmět, zpráva)
                foreach (string receiver in receivers)
                {
                    Console.WriteLine($"Sending message to {receiver}");

                    client.Send(Sender, receiver, subject, message);

                    _logger.Log($"Message sent | SUBJ={{{subject}}} TEXT={{{message.Replace(Environment.NewLine, "")}}} TO={{{receiver}}}", Logger.logLevel.INFO);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                _logger.Log($"Error occured while sending message SUBJ={{{subject}}}", Logger.logLevel.FAIL, ex);

                return false;
            }
        }
    }
}
