using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string name, string email, string subject, string message);

        Task SendWelcomeMailAsync(string name, string email);

        Task SendInfluencerApprovedEmailAsync(string name, string email, string alias);

        Task SendInfluencerDisapprovedEmailAsync(string name, string email);

        Task SendInfluencerFeedbackUpdateEmailAsync(string nameInfluencer, string email, string name);

        Task SendUserFeedbackUpdateEmailAsync(string alias, string email, string name);
    }
}
