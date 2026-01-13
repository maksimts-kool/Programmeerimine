using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using Tund11.Auth.Models;

namespace Tund11.Auth.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet]
    public IActionResult Ankeet()
    {
        return RedirectToAction(actionName: "Index", controllerName: "Ankeet");
    }

    public static async Task SendEmailAsync(IConfiguration configuration, string toEmail, string subject, string body)
    {
        var emailSettings = configuration.GetSection("EmailSettings");
        var smtpServer = emailSettings["SmtpServer"];
        var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
        var senderEmail = emailSettings["SenderEmail"];
        var senderName = emailSettings["SenderName"];
        var username = emailSettings["Username"];
        var password = emailSettings["Password"];

        using var smtpClient = new SmtpClient(smtpServer, smtpPort)
        {
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderEmail!, senderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public static async Task<bool> ValidateEmailDomain(string email)
    {

        try
        {
            var domain = email.Split('@')[1];

            // Check if domain can be resolved
            var hostEntry = await Dns.GetHostEntryAsync(domain);
            return hostEntry != null && hostEntry.AddressList.Length > 0;
        }
        catch (SocketException)
        {
            // Domain doesn't exist or can't be resolved
            return false;
        }
        catch
        {
            // Any other error, consider invalid
            return false;
        }
    }
}
