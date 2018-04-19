using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Common.Logging;
using Jal.Finder.Atrribute;
using Jal.Finder.Impl;
using Jal.HttpClient.Impl.Fluent;
using Jal.HttpClient.Installer;
using Jal.HttpClient.Interface.Fluent;
using Jal.HttpClient.Model;
using Jal.Locator.CastleWindsor.Installer;
using Jal.Router.AzureServiceBus.Extensions;
using Jal.Router.AzureServiceBus.Impl;
using Jal.Router.AzureServiceBus.Installer;
using Jal.Router.AzureStorage.Extensions;
using Jal.Router.AzureStorage.Impl;
using Jal.Router.AzureStorage.Installer;
using Jal.Router.Impl;
using Jal.Router.Impl.Management;
using Jal.Router.Installer;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Logger.Installer;
using Jal.Router.Model;
using Jal.Router.Tests.Impl;
using Jal.Router.Tests.Model;
using Jal.Settings.Installer;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Component = Castle.MicroKernel.Registration.Component;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {



            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            AssemblyFinder.Current = AssemblyFinder.Builder.UsePath(AppDomain.CurrentDomain.BaseDirectory).Create;
            IWindsorContainer container = new WindsorContainer();
            var assemblies = AssemblyFinder.Current.GetAssembliesTagged<AssemblyTagAttribute>();
            container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));
            container.Install(new RouterInstaller(assemblies, "/"));
            container.Install(new ServiceLocatorInstaller());
            container.Install(new SettingsInstaller());
            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerHandler)).Named(typeof(TriggerHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend>)).ImplementedBy(typeof(RequestHandler)).Named(typeof(RequestHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowAHandler)).Named(typeof(TriggerFlowAHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend>)).ImplementedBy(typeof(RequestToSendAppAHandler)).Named(typeof(RequestToSendAppAHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(ResponseToSendAppBHandler)).Named(typeof(ResponseToSendAppBHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowBHandler)).Named(typeof(TriggerFlowBHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend>)).ImplementedBy(typeof(RequestToSendAppCHandler)).Named(typeof(RequestToSendAppCHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(ResponseToSendAppDHandler)).Named(typeof(ResponseToSendAppDHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowCHandler)).Named(typeof(TriggerFlowCHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend,Data>)).ImplementedBy(typeof(RequestToSendAppEHandler)).Named(typeof(RequestToSendAppEHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppFHandler)).Named(typeof(ResponseToSendAppFHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(RequestToSendAppXHandler)).Named(typeof(RequestToSendAppXHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppHHandler)).Named(typeof(ResponseToSendAppHHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<ResponseToSend>)).ImplementedBy(typeof(RequestToSendAppZHandler)).Named(typeof(RequestToSendAppZHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowDHandler)).Named(typeof(TriggerFlowDHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppGHandler)).Named(typeof(ResponseToSendAppGHandler).FullName).LifestyleSingleton());

            container.Register(Component.For(typeof(IRequestResponseHandler<Trigger>)).ImplementedBy(typeof(TriggerFlowEHandler)).Named(typeof(TriggerFlowEHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppJHandler)).Named(typeof(ResponseToSendAppJHandler).FullName).LifestyleSingleton());
            container.Register(Component.For(typeof(IRequestResponseHandler<RequestToSend, Data>)).ImplementedBy(typeof(ResponseToSendAppIHandler)).Named(typeof(ResponseToSendAppIHandler).FullName).LifestyleSingleton());
            //container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(OtherMessageHandler)).Named(typeof(OtherMessageHandler).FullName).LifestyleSingleton());


            //container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(SagaInput1HandlerMessageHandler)).Named(typeof(SagaInput1HandlerMessageHandler).FullName).LifestyleSingleton());
            //container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(SagaInputTopicHandlerMessageHandler)).Named(typeof(SagaInputTopicHandlerMessageHandler).FullName).LifestyleSingleton());
            //container.Register(Component.For(typeof(IMessageHandler<Message>)).ImplementedBy(typeof(SagaInputTopic2HandlerMessageHandler)).Named(typeof(SagaInputTopic2HandlerMessageHandler).FullName).LifestyleSingleton());

            container.Install(new AzureServiceBusRouterInstaller(8));
            container.Install(new AzureStorageRouterInstaller("DefaultEndpointsProtocol=https;AccountName=narwhalappssaeusprod001;AccountKey=O+wOMYBaz5ee8AlZYmVpgyFjaJO2RtLozXtI+XJXJRnuIeQLf4J0AUCoOEDP+rGkGvoCKIciUJFtMa4CHPv5BA==", "sagatests", "messagestests", DateTime.UtcNow.ToString("yyyyMMdd")));
            container.Install(new RouterLoggerInstaller());
            container.Install(new HttpClientInstaller(30000));
            container.Register(Component.For(typeof(ILog)).Instance(LogManager.GetLogger("Cignium.Enigma.App")));
            var client = container.Resolve<IHttpFluentHandler>();

            //config.UsingCommonLogging();

            var host = container.Resolve<IHost>();

            host.Configuration.UsingAzureServiceBus();

            host.Configuration.UsingAzureStorage();

            host.Configuration.ApplicationName = "App";

            host.Configuration.AddMonitoringTask<HeartBeatMonitor>(60000);

            host.Configuration.UsingShutdownWatcher<ShutdownFileWatcher>();



            var storage = container.Resolve<IStorage>(typeof(AzureTableStorage).FullName);

            var ids = new[]
            {"a4d45b6e-d758-4dfe-96ba-54fe3e0b5596"}
;


            var req = new SubmittedEnrollmentEvent()
            {
                ApplicationId = "a4d45b6e-d758-4dfe-96ba-54fe3e0b5596",
                ApplicationDate = new DateTime(2018, 03, 01),
                Attempt = 0,
                CuyLeadId = "3068932",
                EnrollmentId = "100742811",
                ErrorType = ErrorType.None,
                FirstName = "Judith",
                LastName = "Warrick",
                Response = "{\"errors\":null,\"enrollment_id\":\"100742811\",\"enrollment_response_status\":\"eligible\",\"ineligible_reasons\":[]}",
                FriendlyResponse = "",
                Request = "{\"agent\":{\"agent_email_address\":\"marquita.drayton@tzinsurance.com\",\"agent_name\":\"Marquita Drayton\",\"agent_san\":1655504,\"agent_ssn\":\"246-53-8966\",\"notify_when_digitally_signed\":false},\"applicant\":{\"chronic_conditions\":{\"cardiovascular_disease\":{\"circulation\":false,\"heart_problem\":false,\"leg_pain\":false,\"medications\":\"\"},\"chronic_heart_failure\":{\"fluid\":false,\"heart_failure\":false,\"leg_swelling\":false,\"medications\":\"\"},\"chronic_lung_disease\":{\"breathing_problem\":false,\"lung_problem\":false,\"medications\":\"\",\"using_inhaler\":false},\"diabetes\":{\"high_blood_sugar\":false,\"insulin_oral_medication\":false,\"medications\":\"\",\"monitor_blood_sugar\":false}},\"date_of_birth\":\"1952-03-31\",\"first_name\":\"Judith\",\"last_name\":\"Warrick\",\"email_address\":\"lwarrick51@comcast.net\",\"gender\":\"female\",\"hospital_insurance_parta\":\"2017-03-01\",\"medical_insurance_partb\":\"2018-04-01\",\"language_preference\":\"English\",\"permanent_address\":{\"address_line_one\":\"104 Joella Dr\",\"city\":\"Smyrna\",\"county\":\"Rutherford\",\"fips\":\"47149\",\"state\":\"TN\",\"zip_code\":\"37167\"},\"mailing_address\":{\"address_line_one\":\"104 Joella Dr\",\"city\":\"Smyrna\",\"county\":\"\",\"state\":\"TN\",\"zip_code\":\"37167\"},\"phone_number\":\"6155429057\",\"medicare_claim_number\":\"412866311A\",\"middle_initial\":\"E\"},\"application_date\":\"2018-03-01\",\"do_you_or_your_spouse_work\":true,\"end_stage_renal_disease\":{\"diagnosed_positive\":false},\"enrollment_type\":\"SEP\",\"long_term_care\":{\"currently_in_long_term_care\":false},\"marketing\":{\"campaign\":\"0310040752\"},\"medicaid\":{\"currently_on_medicaid\":false},\"ok_to_email\":true,\"osbs\":[],\"other_drug_coverage\":{\"other_rx_coverage\":false},\"other_group_coverage\":{\"medical_coverage\":false},\"partner_application_id\":\"1495099\",\"partner\":{\"id\":\"101\",\"name\":\"Trubridge Direct Sales\",\"ae_id\":\"TRBG\"},\"payments\":[{\"payment_amount\":0.0,\"payment_type\":\"coupon book\"}],\"plan\":{\"contract_number\":\"H4461\",\"pbp_number\":\"029\",\"primary_care_physician\":{\"do_you_wish_to_identify_a_pcp\":true,\"provider\":{\"established_patient\":true,\"provider_name\":\"Joshua McCollum\",\"provider_number\":\"142335\"}}},\"proposed_effective_date\":\"2018-04-01\",\"receive_plan_coverage_material_online\":false,\"sep_code\":\"OTH\",\"sep_date\":\"2018-03-01\",\"sep_reason_other\":\"OTH\",\"signature_type\":\"Client\"}",
                Phone = "6155429057",
                IsSuccess = true,
                StatusCode = "Ok",
                IsTheLastAttempt = false,
                TzApplicationTrackingId = "H44613068932"

            };
            //var first = storage.GetMessages(new DateTime(2018, 2, 26), new DateTime(2018, 2, 26, 23, 00, 00), "appintakesubmittedtenrollmenteventroute", "messages201802");

            //var firstms = first.Select(x => JsonConvert.DeserializeObject<SubmittedEnrollmentEvent>(x.Content));

            //var r = firstms.Where(x => ids.Contains(x.ApplicationId.ToLower())).ToArray();

            //foreach (var submittedEnrollmentEvent in r)
            //{
            //    UpdateEnrollment(client, submittedEnrollmentEvent);
            //    UpdateStatus(client, submittedEnrollmentEvent);
            //    UpdateLog(client, submittedEnrollmentEvent);
            //}


            UpdateEnrollment(client, req);
            UpdateStatus(client, req);
            UpdateLog(client, req);
            //var first = storage.GetMessages(new DateTime(2018, 2, 13, 22, 00, 00), new DateTime(2018, 2, 13, 22, 00, 00), "appintakesubmittedtenrollmenteventroute", "messages201802");

            //var seconds = ms.Where(x => ids.Contains(x.ApplicationId) && !string.IsNullOrWhiteSpace(x.EnrollmentId));



            //host.RunAndBlock();

            Console.ReadLine();

            //var bm = new BrokeredMessage(@"{""Name"":""Raul""}");
            //bm.Properties.Add("origin", "AB");


            //sagabrokered.Route<Message, BrokeredMessage>(bm);

            //var bm1 = new BrokeredMessage(@"{""Name1"":""Raul Naupari""}");
            //bm1.Properties.Add("origin", "ABC");
            ////{"partitionkey":"20170822_saga_f0dd186a-3043-4f32-b437-3d5d283b8f88","rowkey":"e5a5c8be-ce35-45e4-9a8c-3d8b539513dc"}
            ////{ "partitionkey":"20170821_944e53c5-b3eb-43f1-bc87-e5bd4260c21a","rowkey":"f274f66c-095e-48ce-8f3c-6fc7a2b0ef39"}
            ////{"partitionkey":"20170821_737d3a2e-b22c-4ec0-92d9-82474df6757f","rowkey":"bd09b4ed-6485-4293-b568-8e021ec8179c"}
            //bm1.Properties.Add("sagaid", "20171016_saga@b7250358-a128-4f6a-b4e8-027506cbbe7f@20171016");
            //sagabrokered.RouteToSaga<Message1, BrokeredMessage>(bm1, "saga");

            //var bm2 = new BrokeredMessage(@"{""Name1"":""Raul Naupari""}");
            //bm2.Properties.Add("origin", "xcd");
            ////{"partitionkey":"20170822_saga_f0dd186a-3043-4f32-b437-3d5d283b8f88","rowkey":"e5a5c8be-ce35-45e4-9a8c-3d8b539513dc"}
            ////{ "partitionkey":"20170821_944e53c5-b3eb-43f1-bc87-e5bd4260c21a","rowkey":"f274f66c-095e-48ce-8f3c-6fc7a2b0ef39"}
            ////{"partitionkey":"20170821_737d3a2e-b22c-4ec0-92d9-82474df6757f","rowkey":"bd09b4ed-6485-4293-b568-8e021ec8179c"}
            //bm2.Properties.Add("sagaid", "20170927_saga@69864a1a-defa-45c8-aa3c-c6b03b026b93@20170927");
            //sagabrokered.Route<Message1, BrokeredMessage>(bm2, "saga");
        }
        public static void UpdateLog(IHttpFluentHandler _client, SubmittedEnrollmentEvent message)
        {
            var result = message.IsSuccess ? "succeeded" : "failed";

            var url = new Uri(new Uri("https://lm.cignium.com"), "run/cignium/humana-ma/prod/create-worklog-middleware").ToString();

            var r = _client.Post(url).WithTimeout(30000).WithHeaders(x => x.AddLmAuthorizationHeader(url, "api-key-user-636258894521101042", "jl#WX_s^w)kFiX!dhZ0F!-!mi[Masi{T^Eck/6IypqitPOnT$!&2vCHZl6p5", "POST")).MultiPartFormData(
                x =>
                {
                    x.UrlEncoded(message.ApplicationId, "ApplicationId");
                    x.UrlEncoded($"Request {message.Attempt} to humana {result}", "UpdateType");
                    x.UrlEncoded(message.Request, "HumanaRawRequest");
                    x.UrlEncoded(message.StatusCode, "HumanaResponseCode");
                    x.UrlEncoded(message.Response, "HumanaRawResponse");
                    x.UrlEncoded(message.FriendlyResponse, "ValidationMessages");
                }).Send;
        }

        public static void UpdateStatus(IHttpFluentHandler _client, SubmittedEnrollmentEvent message)
        {
            var appstatus = message.IsSuccess ? "Enrollment Complete" : message.ErrorType == ErrorType.Transient ? "Pending Enrollment" : "Enrollment Failed";


            var url = new Uri(new Uri("https://lm.cignium.com"), "run/cignium/humana-ma/prod/update-application-status").ToString();

            var y = _client.Get(url).WithTimeout(30000).WithQueryParameters(x =>
            {
                x.Add("ApplicationId", message.ApplicationId);
                x.Add("ApplicationStatus", appstatus);
                x.Add("ApplicationSubStatus", appstatus);
            }).WithHeaders(x => x.AddLmAuthorizationHeader(url, "api-key-user-636258894521101042", "jl#WX_s^w)kFiX!dhZ0F!-!mi[Masi{T^Eck/6IypqitPOnT$!&2vCHZl6p5")).Send;
        }


        public static void UpdateEnrollment(IHttpFluentHandler _client, SubmittedEnrollmentEvent message)
        {
            if (message.IsSuccess)
            {
                var url = new Uri(new Uri("https://lm.cignium.com"), "run/cignium/humana-ma/prod/update-enrollmentid").ToString();

                var y = _client.Get(url).WithTimeout(30000).WithQueryParameters(x =>
                {
                    x.Add("ApplicationId", message.ApplicationId);
                    x.Add("EnrollmentId", message.EnrollmentId);
                    x.Add("LeadId", message.CuyLeadId);
                }).WithHeaders(x => x.AddLmAuthorizationHeader(url, "api-key-user-636258894521101042", "jl#WX_s^w)kFiX!dhZ0F!-!mi[Masi{T^Eck/6IypqitPOnT$!&2vCHZl6p5")).Send;
            }

        }
    }
    public static class RestHeaderDescriptorExtension
    {
        public static void AddLmAuthorizationHeader(this IHttpHeaderDescriptor descriptor, string url, string key,
            string secret, string protocol = "GET")
        {
            var requestUri = new Uri(url);

            var utcdate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

            var message = string.Join("\n", protocol, utcdate, requestUri.AbsolutePath.ToLower());

            var hash = Hash(secret, message);

            descriptor.Add("Authorization", $"APPLICATION-IDENTITY {key}:{hash}");

            descriptor.Add("Timestamp", utcdate);
        }

        private static void AddAuthorizationHeader(HttpRequest httpRequest, string key, string secret, string protocol)
        {
            var requestUri = new Uri(httpRequest.Url);

            var utcdate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

            var message = string.Join("\n", protocol, utcdate, requestUri.AbsolutePath.ToLower());

            var hash = Hash(secret, message);

            httpRequest.Headers.Add(new HttpHeader("Authorization", $"APPLICATION-IDENTITY {key}:{hash}"));

            httpRequest.Headers.Add(new HttpHeader("Timestamp", utcdate));
        }

        private static string Hash(string hashedPassword, string message)
        {
            var key = Encoding.UTF8.GetBytes(hashedPassword.ToUpper());

            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

                return Convert.ToBase64String(hash);
            }
        }


    }
    public class SubmittedEnrollmentEvent
    {
        public string EnrollmentId { get; set; }
        public string ApplicationId { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string CuyLeadId { get; set; }
        public bool IsSuccess { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string FriendlyResponse { get; set; }
        public string StatusCode { get; set; }
        public int Attempt { get; set; }
        public bool IsTheLastAttempt { get; set; }
        public string TzApplicationTrackingId { get; set; }
        public ErrorType ErrorType { get; set; }
        public SubmittedEnrollmentEvent()
        {
            EnrollmentId = string.Empty;
            ApplicationId = string.Empty;
            CuyLeadId = string.Empty;
            Request = string.Empty;
            Response = string.Empty;
            IsSuccess = true;
            ErrorType = ErrorType.None;
        }

    }
    public enum ErrorType
    {
        None,
        Transient,
        NonTransient
    }
}
