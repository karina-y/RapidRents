using System.Collections.Generic;
using RapidRents.Web.Models.Requests.Messaging;

namespace RapidRents.Web.Services.Messaging
{
    public interface IMessagingService
    {
        int Insert(MessagingAddRequests model, string userId);
        int SendMessage(MessagingAddRequests model, string userId);
        void SendEmailConfirmation(string email, string callbackURL);
        void PasswordResetToken(string Email, string callbackURL);
        void createMtRqstMessage(int AddressId, MessagingAddRequests model);
    }
}
