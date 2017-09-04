using SendGrid;
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
       //     var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
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
            //            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bestfluence.com", "Bestfluence"),
                Subject = "Velkommen til Bestfluence",
                HtmlContent =

                "<p><b>Hej " + name + ",</b></p>" +
                "<p>Tak fordi du har oprettet dig som bruger hos Bestfluence.</p>" +
                "<p>Du er nu en del af Bestfluence og vores fælles mål om at stoppe utroværdighed, mobning og løgne på sociale medier og det er vi glade for. Derfor beder vi dig om at sprede budskabet til andre følgere og influencere, således at vi sammen én gang for alle kan gøre de sociale medier til et trygt sted at være.</p>" +
                "<p>Du er også meget velkommen til at gå på opdagelse i vores univers af følgere, influencere og alt deres konstruktive feedback. Vores database af influencere bliver større og større, men vi har ikke fundet dem alle og vil derfor bede dig om at give en hånd med. Hvis du kender en derude, som er gået vores næse forbi, kan du oprette vedkommende <a href=" + "https://goo.gl/8Z89ke" + ">her</a>.</p>" +
                "<p>For at forbedre din oplevelse af Bestfluence og gøre vores univers endnu bedre, har vi lavet en 3 minutters spørgeundersøgelse, hvor du kan give os feedback og sætte dit præg på hjemmesiden. Du finder spørgeskemaet <a href=" + "https://goo.gl/forms/W5nGkU8hDRdDk6oZ2" + ">her</a>.</p>" +
                "<p>Har du spørgsmål eller kommentarer, er du altid velkommen til at sende os en mail på <a href="+"mailto:support@bestfluence.com?Subject="+">support@bestfluence.com</a>. </p>"+
                "<p>Tak fordi du støtter op om et trygt online fællesskab!</p>" +
                "<p>Med venlig hilsen<p>" +
                "<p><b>Bestfluence</b><br>" +
                "<b><a href=" + "https://goo.gl/YSp8uj" + ">bestfluence.dk</a></b></p>"
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
            //var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bestfluence.com", "Bestfluence"),
                Subject = "Din influencerprofil blev godkendt",
                HtmlContent =
                "<p><b>Hej "+name+",</b></p>"+
                "<p>Tillykke, din influencerprofil "+alias+" blev godkendt hos Bestfluence og du er nu klar til at blive fundet og modtage konstruktiv feedback fra dine følgere. Vi har gjort det nemt og overskueligt for dig at holde styr på og besvare din feedback under <a href="+"https://goo.gl/ameKC1"+">Min feedback</a>.</p>"+
                "<p>Har du spørgsmål eller kommentarer, er du altid velkommen til at sende os en mail på <a href="+"mailto:support@bestfluence.com?Subject="+">support@bestfluence.com</a>.</p>"+
                "<p>Tak fordi du støtter op om et trygt online fællesskab!</p>"+
                "<p>Med venlig hilsen<p>" +
                "<p><b>Bestfluence</b><br>" +
                "<b><a href=" + "https://goo.gl/YSp8uj" + ">bestfluence.dk</a></b></p>"
            };

            msg.AddTo(new EmailAddress(email, name));
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendInfluencerDisapprovedEmailAsync(string name, string email)
        {
            //var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bestfluence.com", "Bestfluence"),
                Subject = "Din influencerprofil blev ikke godkendt",
                HtmlContent =
                "<p><b>Hej "+name+",</b></p>"+
                "<p>Din influencerprofil blev desværre ikke godkendt hos Bestfluence, da den ikke lever op til vores <a href=" + " https://goo.gl/cvsGDk" + ">retningslinjer</a></p>"+
                "<p>Har du problemer med at opsætte din influencerprofil, vælge kategorier og linke til dine sociale medier, er du altid velkommen til at kontakte os på <a href=" + "mailto:support@bestfluence.com?Subject=" + ">support@bestfluence.com</a><p>" +
                "<p>Du er velkommen til at oprettet en influencerprofil igen ved at klikke på Bliv <a href=" + "https://goo.gl/BUkPKu" + ">influencer</a></p>"+
                "<p>Tak fordi du støtter op om et trygt online fællesskab!</p>"+
                "<p>Med venlig hilsen<p>" +
                "<p><b>Bestfluence</b><br>" +
                "<b><a href=" + "https://goo.gl/YSp8uj" + ">bestfluence.dk</a></b></p>"
            };

            msg.AddTo(new EmailAddress(email, name));
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendInfluencerFeedbackUpdateEmailAsync(string nameInfluencer, string email, string name)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bestfluence.com", "Bestfluence"),
                Subject = "Du har fået feedback!",
                HtmlContent =
                "<p><b>Hej " + nameInfluencer + ",</b></p>"+
                "<p>Du har modtaget ny feedback fra "+name+ " som du finder under <a href=" + " https://goo.gl/V6hDLu" + ">Min feedback</a>.</p>" +
                "<p>For at forbedre din oplevelse af Bestfluence og gøre vores univers endnu bedre, har vi lavet en 3 minutters spørgeundersøgelse, "+
                "hvor du kan give os feedback og sætte dit præg på hjemmesiden. Du finder spørgeskemaet <a href=" + "https://goo.gl/forms/W5nGkU8hDRdDk6oZ2" + ">her</a>.</p>"+
                "<p>Tak fordi du støtter op om et trygt online fællesskab!</p>"+
                "<p>Med venlig hilsen<p>" +
                "<p><b>Bestfluence</b><br>" +
                "<b><a href=" + "https://goo.gl/YSp8uj" + ">bestfluence.dk</a></b></p>"
            };

            msg.AddTo(new EmailAddress(email, name));
            var response = await client.SendEmailAsync(msg);
        }

        public async Task SendUserFeedbackUpdateEmailAsync(string alias, string email, string name)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("noreply@bestfluence.com", "Bestfluence"),
                Subject = "Du har fået svar på din feedback!",
                HtmlContent =
                "<p><b>Hej " + name + ",</b></p>" +
                "<p>Du har modtaget svar på din feedback til "+alias+ " som du finder under <a href=" + "https://goo.gl/vGWztJ" + ">Min feedback</a></b>.</p>" +
                "<p>For at forbedre din oplevelse af Bestfluence og gøre vores univers endnu bedre, har vi lavet en 3 minutters spørgeundersøgelse, hvor du kan give os feedback og sætte dit præg på hjemmesiden. Du finder spørgeskemaet <a href=" + "https://goo.gl/forms/W5nGkU8hDRdDk6oZ2" + ">her</a>.</p>" +
                "<p>Tak fordi du støtter op om et trygt online fællesskab!</p>"+
                "<p>Med venlig hilsen<p>" +
                "<p><b>Bestfluence</b><br>" +
                "<b><a href=" + "https://goo.gl/YSp8uj" + ">bestfluence.dk</a></b></p>"
            };

            msg.AddTo(new EmailAddress(email, name));
            var response = await client.SendEmailAsync(msg);
        }



    }
}
