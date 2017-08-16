﻿using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public async Task SendEmailAsync(string name, string email, string subject, string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bestfluence.com", "Bestfluence"),
                Subject = subject,
                HtmlContent = message,
            };
            msg.AddTo(new EmailAddress(email, name));

            var response = await client.SendEmailAsync(msg);
        }
      
        public async Task SendWelcomeMailAsync(string name, string email)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bestfluence.com", "Bestfluence"),
                Subject = "Velkommen til Bestfluence",
                HtmlContent =
                "<strong>Hej " + name + ", </strong> <br><br> " +
                "Tak fordi du har oprettet dig som bruger hos Bestfluence. <br><br> " +
                "Du er nu en del af Bestfluence og vores fælles mål om at stoppe utroværdighed, mobning og løgne på sociale medier og det er vi glade for. Derfor beder vi dig om at sprede budskabet til andre følgere og influencere, således at vi sammen én gang for alle kan gøre de sociale medier til et trygt sted at være.<br><br> " +
                "Du er også meget velkommen til at gå på udkig i vores univers af følgere, influencere og alt deres konstruktive feedback. Vores database af influencere bliver større og større, men vi har ikke fundet dem alle og vil derfor bede dig om at give en hånd med. Hvis du kender en derude, som er gået vores næse forbi, kan du oprette vedkommende her. <br><br>" +
                "For at forbedre din oplevelse af bestfluence.dk, har vi lavet en spørgeundersøgelse som vi håber du vil tage dig tid til at udfylde. Den tager ca. 3 minutter og kan findes <a href=" + "https://goo.gl/forms/fBAE9WggIfAORNLn2" + ">her</a>.<br><br>" +
                "Har du spørgsmål eller kommentarer, er du altid velkommen til at sende os en mail på support@bestfluence.com. <br><br>" +
                "Med venlig hilsen <br> <strong>Bestfluence</strong><br><br>"
            };

            msg.AddTo(new EmailAddress(email, name));
            var response = await client.SendEmailAsync(msg);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }

        public async Task SendInfluencerApprovedEmailAsync(string name, string email, string alias)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bestfluence.com", "Bestfluence"),
                Subject = "Din influencerprofil er godkendt",
                HtmlContent =
                "<strong>Hej " + name + ", </strong> <br><br> " +
                "Din influencerprofil " + alias + " er godkendt hos Bestfluence.<br><br>" +
                "Det betyder at du nu er klar til at blive fundet og modtage feedback fra dine følgere. Vi har gjort det nemt og overskueligt for dig at holde styr på og besvare din feedback, du skal bare klikke <a>her</a>. <br><br>" + 
                "For at forbedre din oplevelse af bestfluence.dk, har vi lavet en spørgeundersøgelse som vi håber du vil tage dig tid til at udfylde. Den tager ca. 3 minutter og kan findes <a href=" + "https://goo.gl/forms/fBAE9WggIfAORNLn2" + ">her</a>.<br><br>" +
                "Har du spørgsmål eller kommentarer, er du altid velkommen til at sende os en mail på support@bestfluence.com. <br><br>" +
                "Tak fordi du støtter op om et trygt online fællesskab!<br><br>" +
                "Med venlig hilsen <br> <strong>Bestfluence</strong><br><br>"
            };

            msg.AddTo(new EmailAddress(email, name));
            var response = await client.SendEmailAsync(msg);
        }
    }
}
