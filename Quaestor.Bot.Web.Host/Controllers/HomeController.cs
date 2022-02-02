using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Timing;
using Quaestor.Bot.Controllers;
using CoinbaseCommerce;
using CoinbaseCommerce.Utils;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Quaestor.Bot.Products.Dto;
using Abp.Dependency;
using Quaestor.Bot.Products;
using Castle.Core.Logging;
using Quaestor.Bot.JobManagement.EmailTemplate;
using Microsoft.AspNetCore.Hosting;

namespace Quaestor.Bot.Web.Host.Controllers
{
    public class HomeController : BotControllerBase
    {
        private readonly INotificationPublisher _notificationPublisher;
        public ILogger Logger { get; set; }
        private IHostingEnvironment _env;
        public HomeController(INotificationPublisher notificationPublisher, IHostingEnvironment env)
        {
            _notificationPublisher = notificationPublisher;
            Logger = NullLogger.Instance;
            _env = env;
        }

        public IActionResult Index()
        {
            Logger.Info("Swagger started");           
            return Redirect("/swagger");
        }

        [HttpPost]
        public async Task<ActionResult> PaymentRepsonse()
        {
            Logger.Info("enter in PaymentRepsonse   =");
            foreach (var header in Request.Headers)
            {
                Logger.Info("Key  =" + header.Key +"  Value" +header.Value);               
            }
            var headerValue = Request.Headers[HeaderNames.WebhookSignature];
            string webhookHeaderValue = string.Empty;
            string bodyResponse = string.Empty;
            using (var reader = new StreamReader(Request.Body))
            {
                 bodyResponse = reader.ReadToEnd();               
            }
            Logger.Info("Json Response  ="+bodyResponse);
            if (headerValue.Any() == true)
            {
                webhookHeaderValue = headerValue.ToString();
            }         
            if (WebhookHelper.IsValid(ShareData.WebhookSecret, webhookHeaderValue, bodyResponse))
            {
                try
                {

                Logger.Info("Web Hook is valid " );

                JObject Mainobj = JObject.Parse(bodyResponse);
                JObject Mainobj_event = JObject.Parse(Mainobj["event"].ToString());
                JObject even_data = JObject.Parse(Mainobj_event["data"].ToString());
                JObject data_pricing = JObject.Parse(even_data["pricing"].ToString());
                JObject data_metadata = JObject.Parse(even_data["metadata"].ToString());
                string[] key = new string[2];
                int count = 0;
                foreach (var x in data_pricing)
                {
                    key[count] = x.Key.ToString();
                    count++;
                    if (count > 1)
                        break;
                }
                JObject pricing_local = JObject.Parse(data_pricing[key[0]].ToString());
                JObject pricing_Destination = JObject.Parse(data_pricing[key[1]].ToString());
                JObject data_addresses = JObject.Parse(even_data["addresses"].ToString());
                    //addresses
                Logger.Info("Main Object passed ");
                CoinbaseCommerce.Models.CallBackResponse resp = new CoinbaseCommerce.Models.CallBackResponse();
                resp.Code = even_data["code"].ToString();
                resp.Username = even_data["name"].ToString();
                resp.UserId = Convert.ToInt64(data_metadata["CustomerId"]);
                resp.ProductId = Convert.ToInt64(data_metadata["ItemId"]);
                resp.UserProductId = Convert.ToInt32(data_metadata["UserProductId"]);
                resp.SrcAmount = Convert.ToDouble(pricing_local["amount"]);
                resp.SrcCurrency = pricing_local["currency"].ToString();
                resp.DestAmount = Convert.ToDouble(pricing_Destination["amount"]);
                resp.DestCurrency = pricing_Destination["currency"].ToString();
                Logger.Info("Dest Currency ="+ resp.DestCurrency);
                    string addresskey = "";
                foreach (var x in data_addresses)
                {
                    addresskey = x.Key.ToString();
                    break;
                }

                resp.Address = data_addresses[addresskey].ToString();
                resp.CreatedAt = Convert.ToDateTime(even_data["created_at"]);
                resp.ExpiredAt = Convert.ToDateTime(even_data["expires_at"]);
                resp.HostedUrl = even_data["hosted_url"].ToString();
                resp.Status = Mainobj_event["type"].ToString();
                 Logger.Info("Type =" + resp.Status);
                    if (resp.Status == "charge:created")
                    {
                        resp.Status = "created";
                        CreateUserProductsPaymentRecordsDto createUserProductsPaymentRecordsDto = new CreateUserProductsPaymentRecordsDto
                        {
                            CreatorUserId = Convert.ToInt32(resp.UserId),
                            ProductId = Convert.ToInt32(resp.ProductId),
                            UserProductId = resp.UserProductId,
                            Code = resp.Code,
                            SourceAmount = Convert.ToDecimal(resp.SrcAmount),
                            SourceCurrency = resp.SrcCurrency,
                            DestinationCurrency = resp.DestCurrency,
                            DestinationAmount = Convert.ToDecimal(resp.DestAmount),
                            Address = resp.Address,
                            CreatedAt = resp.CreatedAt,
                            ExpiresAt = resp.ExpiredAt,
                            Type = resp.Status
                        };
                       await IocManager.Instance.Resolve<IUserProductsPaymentRecordsDomainService>().CreateUserProudctPayment(createUserProductsPaymentRecordsDto);
                    }
                    else if (resp.Status == "charge:confirmed")
                    {
                        resp.ConfirmedAt = Convert.ToDateTime(even_data["confirmed_at"]);
                        resp.Status = "confirmed";
                        UserProductsPaymentRecordSearch userProductsPaymentRecordSearch = new UserProductsPaymentRecordSearch {Code=resp.Code };
                        var paymentInfo = IocManager.Instance.Resolve<IUserProductsPaymentRecordsDomainService>().GetPaymentByCode(userProductsPaymentRecordSearch);
                        if (paymentInfo != null)
                        {
                            paymentInfo.Type = resp.Status;
                            paymentInfo.ConfirmedAt = resp.ConfirmedAt;
                            await IocManager.Instance.Resolve<IUserProductsPaymentRecordsDomainService>().UpdateUserProudctPayment(paymentInfo);
                            var user= await IocManager.Instance.Resolve<Users.UserAppService>().GetUserByEmailAsync(resp.Username);
                            if (user!=null)
                            {
                                string tempaltePackageBuy = GenericFuntions.ReadHtmlPage("PackageBuy", _env.WebRootPath);
                               GenericFuntions.SendEmail("Monthly subscription successful", string.Format(tempaltePackageBuy, user.UserName), user.EmailAddress);

                            }

                        }

                    }
                    else if (resp.Status == "charge:failed")
                    {
                        resp.Status = "failed";
                        dynamic payments_data = JsonConvert.DeserializeObject(even_data["payments"].ToString());                       
                        foreach (var item in payments_data)
                        {
                            var mainval = JObject.Parse(item["value"].ToString());
                            JObject cryptoval = JObject.Parse(mainval["crypto"].ToString());
                            var amountPaid = cryptoval["amount"].ToString();
                            var user = await IocManager.Instance.Resolve<Users.UserAppService>().GetUserByEmailAsync(resp.Username);
                            if (user != null && Convert.ToDouble(amountPaid) <resp.DestAmount)
                            {
                                string tempaltePackageBuy = GenericFuntions.ReadHtmlPage("PaymentHeld", _env.WebRootPath);
                                GenericFuntions.SendEmail("Payment Held", string.Format(tempaltePackageBuy, user.UserName,resp.Code,resp.DestAmount, amountPaid), user.EmailAddress);
                            }

                        }
                        UserProductsPaymentRecordSearch userProductsPaymentRecordSearch = new UserProductsPaymentRecordSearch { Code = resp.Code };
                        var paymentInfo = IocManager.Instance.Resolve<IUserProductsPaymentRecordsDomainService>().GetPaymentByCode(userProductsPaymentRecordSearch);
                        if (paymentInfo != null && paymentInfo.Type!= "confirmed")
                        {
                            paymentInfo.Type = resp.Status;
                            await IocManager.Instance.Resolve<IUserProductsPaymentRecordsDomainService>().UpdateUserProudctPayment(paymentInfo);
                        }

                    }
                }
                catch (Exception ex)
                {

                    Logger.Error(ex.Message,ex);
                }

            }
            else
            {
                Logger.Info("Web Hook not Valid");
                // Some hackery going on. The Webhook message validation failed.
                // Someone is trying to spoof payment events!
                // Log the requesting IP address and HTTP body. 
            }
            return Ok();
        }
        /// <summary>
        /// This is a demo code to demonstrate sending notification to default tenant admin and host admin uers.
        /// Don't use this code in production !!!
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> TestNotification(string message = "")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            var defaultTenantAdmin = new UserIdentifier(1, 2);
            var hostAdmin = new UserIdentifier(null, 1);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { defaultTenantAdmin, hostAdmin }
            );

            return Content("Sent notification: " + message);
        }
    }
}
