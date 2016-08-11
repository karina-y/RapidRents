using RapidRents.Web.Models.Requests.Messaging;
using System.Data.SqlClient;
using System.Net.Mail;
using SendGrid;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web;
using RapidRents.Web.Services.Templates;
using RapidRents.Web.Domain;
using System.Collections.Generic;

namespace RapidRents.Web.Services.Messaging
{
    public class MessagingService : BaseService, IMessagingService
    {
        private readonly ITemplateService _templateService;
        private readonly IMaintenanceRequestServices _MRqstService;

        public MessagingService(ITemplateService templateService, IMaintenanceRequestServices MRqstService)
        {
            _MRqstService = MRqstService;
            _templateService = templateService;
        }

        private static string sendGridKey = ConfigurationManager.AppSettings["SendGridKey"];

        public int Insert(MessagingAddRequests model, string userId)
        {
            int id = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Messaging_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)

               {
                   paramCollection.AddWithValue("@UserId", userId);
                   paramCollection.AddWithValue("@TypeId", model.TypeId);
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Email", model.Email);
                   paramCollection.AddWithValue("@Message", model.Message);

                   SqlParameter p = new SqlParameter("@Id", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)

               {
                   int.TryParse(param["@id"].Value.ToString(), out id);
               }
               );
            return id;
        }

        public int SendMessage(MessagingAddRequests model, string userId)
        {
            RapidRentRqst message = new RapidRentRqst();
            int messageId = Insert(model, userId);
            MailDefinition md = new MailDefinition();
            string templateName = "sendmessage.html";
            message.From = model.Name + model.Email;
            message.Subject = "Inquiry Type: " + model.TypeId;
            message.To = model.Email;
            string templateContent = _templateService.GetTemplateContents(templateName);
            string replaceHtml = templateContent.Replace("model.Email", model.Email).Replace("model.Name", model.Name).Replace("model.Message", model.Message);
            message.Html = replaceHtml;
            createMessage(message);
            return messageId;
        }

        public void SendEmailConfirmation(string Email, string callbackUrl)
        {
            RapidRentRqst message = new RapidRentRqst();
            string templateName = "sendemailconfirmation.html";
            message.From = "support@RapidRents.dev";
            message.Subject = "Confirm your Account";
            message.To = Email;
            string templateContent = _templateService.GetTemplateContents(templateName);
            string replaceConfirm = templateContent.Replace("callBackURL", callbackUrl);
            message.Html = replaceConfirm;
            createMessage(message);
        }

        public void PasswordResetToken(string Email, string callbackUrl)
        {
            RapidRentRqst message = new RapidRentRqst();
            string templateName = "passwordresettoken.html";
            message.From = "noreply@RapidRents.dev";
            message.Subject = "Password Reset";
            message.To = Email;
            string templateContent = _templateService.GetTemplateContents(templateName);
            string replacePswrdToken = templateContent.Replace("callBackUrl", callbackUrl);
            message.Html = replacePswrdToken;
            createMessage(message);
        }


        private void createMessage(RapidRentRqst message)
        {
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.From = new MailAddress(message.From);
            myMessage.AddTo(message.To);
            myMessage.Subject = message.Subject;
            myMessage.Html = message.Html;
            var transportWeb = new SendGrid.Web(sendGridKey);
            transportWeb.DeliverAsync(myMessage);
        }

        public void createMtRqstMessage(int AddressId, MessagingAddRequests model)
        {
            string replaceHtml = string.Empty;
            RapidRentRqst message = new RapidRentRqst();
            List<MaintenanceRequest> mntncRqMessage = _MRqstService.GetMaintenanceRqstByAddId(AddressId);
            message.From = model.Name + model.Email;
            message.Subject = "Maintenance Request";
            message.To = model.Email;

            foreach (var item in mntncRqMessage)
            {
                string templateContent = _templateService.GetTemplateContents("sendmessage.html");
                string newContent = templateContent.Replace("model.Email", model.Email).Replace("model.Name", model.Name).Replace("model.Message", "<div>" + "<p>Name: " + item.Name + "</p>" + "<p>Status: " + item.Status + "</p>" + "<p>User Id: " + item.UserId + "<p>" + "<p>Address Id: " + item.AddressId + "</p>" + "<p>UrgencyId: " + item.UrgencyId + "</p>" + "<p>Subject: " + item.Subject + "</p>" + "<p>Description: " + item.Description + "</p>" + "</div>");
                replaceHtml = replaceHtml + newContent;
            }

            message.Html = replaceHtml;
            createMessage(message);
        }


        public void SendEmailStatusChange(string email, string MaintStatus, string callbackURL)
        {
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.To = new MailAddress[] { new MailAddress(email) };
            myMessage.From = new MailAddress("noreply@rapidrents.dev");
            myMessage.Subject = "Maintenance Request Status Update";
            string templateName = HttpContext.Current.Server.MapPath("~/Scripts/rapidRents/ContactUs/MaintStatusUpdate.html");
            string messageHtml = _templateService.GetTemplateContents(templateName);
            string replaceStatusUpdate = messageHtml.Replace("callBackURL", callbackURL);
            replaceStatusUpdate = replaceStatusUpdate.Replace("usersEmailHere", email);
            myMessage.Html = replaceStatusUpdate;
            var transportWeb = new SendGrid.Web(sendGridKey);
            transportWeb.DeliverAsync(myMessage);
        }


        public void SendEmailMaintComment(string email, string MaintComment, string callbackURL)
        {
            SendGridMessage myMessage = new SendGridMessage();
            myMessage.To = new MailAddress[] { new MailAddress(email) };
            myMessage.From = new MailAddress("noreply@rapidrents.dev");
            myMessage.Subject = "New Comment for Your Maintenance Request";
            string templateName = HttpContext.Current.Server.MapPath("~/Scripts/rapidRents/ContactUs/MaintStatusUpdateEmail.html");
            string messageHtml = _templateService.GetTemplateContents(templateName);
            string replaceMaintComment = messageHtml.Replace("callBackURL", callbackURL);
            myMessage.Html = replaceMaintComment;
            var transportWeb = new SendGrid.Web(sendGridKey);
            transportWeb.DeliverAsync(myMessage);
        }
    }
}
