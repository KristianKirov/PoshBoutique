using PoshBoutique.Data;
using PoshBoutique.Data.Providers;
using PoshBoutique.Identity;
using PoshBoutique.MailSending;
using PoshBoutique.Models;
using PoshBoutique.Templates;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using PoshBoutique.Polyfills;
using PoshBoutique.Data.Models;

namespace PoshBoutique.Facades
{
    public class MailSendingFacade
    {
        private IMailSender mailSender;
        private TemplateEngine templateEngine;

        public MailSendingFacade()
            : this(new SystemMailSender(), TemplateEngineFactory.GetDefault())
        {
        }

        public MailSendingFacade(IMailSender mailSender, TemplateEngine templateEngine)
        {
            this.mailSender = mailSender;
            this.templateEngine = templateEngine;
        }

        public void SendForgottenPasswordMail(string baseUrl, string resetPasswordUrl, string toEmailAddress)
        {
            HostingEnvironmentPolyfills.QueueBackgroundWorkItemPf(async ct =>
            {
                string emailContent = templateEngine.RenderTemplate("ForgottenPassword",
                    new
                    {
                        BaseUrl = baseUrl,
                        ResetPasswordUrl = resetPasswordUrl
                    });

                await this.mailSender.SendEmail(toEmailAddress, "Забравена парола", emailContent);
            });
        }

        public void SendContactUsMail(FeedbackModel feedback)
        {
            HostingEnvironmentPolyfills.QueueBackgroundWorkItemPf(async ct =>
            {
                string contactUsContent = templateEngine.RenderTemplate("ContactUs", feedback);

                await this.mailSender.SendEmail(ConfigurationManager.AppSettings["MailSending.DefaultSender"], "Попълнена бе формата за контакти", contactUsContent);
            });
        }

        public void SendNewOrderMail(int orderId)
        {
            HostingEnvironmentPolyfills.QueueBackgroundWorkItemPf(async ct =>
            {
                NewOrderEmailModel emailModel = new NewOrderEmailModel();

                OrdersProvider ordersProvider = new OrdersProvider();
                Order order = await ordersProvider.GetFullOrderData(orderId);

                string toEmailAddress;
                using (ApplicationDbContext appContext = new ApplicationDbContext())
                {
                    string userId = order.UserId.ToString();
                    Address userAddress = await appContext.AddressInfos.FirstOrDefaultAsync(a => a.UserId == userId);

                    emailModel.Country = userAddress.Country;
                    emailModel.City = userAddress.City;
                    emailModel.District = userAddress.District;
                    emailModel.PostCode = userAddress.PostCode;
                    emailModel.Commune = userAddress.Commune;
                    emailModel.AddresDetails = userAddress.AddresDetails;
                    emailModel.FullName = string.Format("{0} {1} {2}", userAddress.FirstName, userAddress.MiddleName, userAddress.LastName);
                    emailModel.Phone = userAddress.Phone;
                    toEmailAddress = userAddress.Email;
                }

                emailModel.BaseUrl = ConfigurationManager.AppSettings["Site.BaseUrl"];
                emailModel.ContactUsFormUrl = string.Concat(emailModel.BaseUrl, ConfigurationManager.AppSettings["Site.ContactUsFormUrl"].TrimStart('/'));
                emailModel.OrdersPageUrl = string.Concat(emailModel.BaseUrl, ConfigurationManager.AppSettings["Site.OrdersPageUrl"].TrimStart('/'));

                emailModel.OrderNumber = order.Id;
                emailModel.CommissionPercents = order.HasCommission ? order.CommissionPercents : 0;
                emailModel.DeliveryMerchant = order.DeliveryMerchant;
                emailModel.ShippingPrice = order.ShippingPrice.ToString("N2") + "лв";
                emailModel.TotalPrice = order.TotalPrice.ToString("N2") + "лв";
                emailModel.Items = order.OrderDetails.Select(od => new Item()
                {
                    Title = od.Article.Title,
                    Quantity = od.Quantity,
                    Price = od.ItemPrice.ToString("N2") + "лв"
                }).ToList();

                string emailContent = templateEngine.RenderTemplate("NewOrder", emailModel);

                await this.mailSender.SendEmail(toEmailAddress, string.Format("{0}: Поръчка #{1}", emailModel.BaseUrl, emailModel.OrderNumber), emailContent);
            });
        }

        public void SendOrderStatusChangedMail(int orderId, int oldStatusId, int newStatusId)
        {
            if (oldStatusId == newStatusId)
            {
                return;
            }

            HostingEnvironmentPolyfills.QueueBackgroundWorkItemPf(async ct =>
            {
                OrdersProvider ordersProvider = new OrdersProvider();
                Task<OrderStatusModel> getNewStatusTask = ordersProvider.GetStatus(newStatusId);
                Task<OrderStatusModel> getOldStatusTask = ordersProvider.GetStatus(oldStatusId);

                OrderStatusChangedEmailModel emailModel = new OrderStatusChangedEmailModel();
                emailModel.OrderNumber = orderId;

                SimpleOrderModel orderModel = await ordersProvider.GetOrder(orderId);

                string toEmailAddress;
                using (ApplicationDbContext appContext = new ApplicationDbContext())
                {
                    string userId = orderModel.UserId.ToString();
                    Address userAddress = await appContext.AddressInfos.FirstOrDefaultAsync(a => a.UserId == userId);
                    emailModel.FullName = string.Format("{0} {1} {2}", userAddress.FirstName, userAddress.MiddleName, userAddress.LastName);

                    toEmailAddress = userAddress.Email;
                }

                emailModel.BaseUrl = ConfigurationManager.AppSettings["Site.BaseUrl"];

                OrderStatusModel newOrderStatus = await getNewStatusTask;
                OrderStatusModel oldOrderStatus = await getOldStatusTask;

                emailModel.NewOrderStatus = newOrderStatus.Name;
                emailModel.OldOrderStatus = oldOrderStatus.Name;

                string emailContent = templateEngine.RenderTemplate("OrderStatusChanged", emailModel);

                await this.mailSender.SendEmail(toEmailAddress, string.Format("{0}: Статусът на поръчка #{1} беше променен", emailModel.BaseUrl, orderId), emailContent);
            });
        }

        public void SendNewUserRegisteredMail(string userEmail, string userFirstName, string userLastName)
        {
            HostingEnvironmentPolyfills.QueueBackgroundWorkItemPf(async ct =>
            {
                NewUserEmailModel emailModel = new NewUserEmailModel();
                emailModel.BaseUrl = ConfigurationManager.AppSettings["Site.BaseUrl"];
                emailModel.FirstName = userFirstName;
                emailModel.LastName = userLastName;

                string emailContent = templateEngine.RenderTemplate("NewUserRegistered", emailModel);

                await this.mailSender.SendEmail(userEmail, string.Format("{0}: Добре дошнли в бутик Posh", emailModel.BaseUrl), emailContent);
            });
        }

        public async Task SendLoyalCustomerMail(Guid userId, int orderId)
        {
            LoyalCustomerEmailModel emailModel = new LoyalCustomerEmailModel();
            emailModel.BaseUrl = ConfigurationManager.AppSettings["Site.BaseUrl"];
            emailModel.OrderNumber = orderId;

            string toEmailAddress;
            using (ApplicationDbContext appContext = new ApplicationDbContext())
            {
                string userIdString = userId.ToString();
                Address userAddress = await appContext.AddressInfos.FirstOrDefaultAsync(a => a.UserId == userIdString);
                emailModel.FullName = string.Format("{0} {1} {2}", userAddress.FirstName, userAddress.MiddleName, userAddress.LastName);

                toEmailAddress = userAddress.Email;
            }

            string emailContent = templateEngine.RenderTemplate("LoyalCustomer", emailModel);

            await this.mailSender.SendEmail(toEmailAddress, string.Format("{0}: Вписани бяхте като лоялен клиент", emailModel.BaseUrl), emailContent);
        }
    }
}