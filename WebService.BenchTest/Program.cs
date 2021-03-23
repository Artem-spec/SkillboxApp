using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;

using Google.Apis.AnalyticsReporting.v4;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.AnalyticsReporting.v4.Data;

using System.Security.Cryptography.X509Certificates;

namespace WebService.BenchTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
        }


        public static void Test()
        {

            //var service = AuthenticateServiceAccount(
            //    "pain-949@rb-openapi-wso2.iam.gserviceaccount.com", "rb-openapi-wso2-b6f74f5ff512.json");

            //var request = new GetReportsRequest
            //{
            //    ReportRequests = new[] {
            //            new ReportRequest{
            //                DateRanges = new[] { new DateRange{ StartDate = "2019-01-01", EndDate = "2019-01-31" }},
            //                Dimensions = new[] { new Dimension{ Name = "ga:date" }},
            //                Metrics = new[] { new Metric{ Expression = "ga:sessions", Alias = "Sessions"}},
            //                ViewId = "208179485"
            //            }
            //        }
            //};
           // var report = BatchGet(service, request);

            ConnectToGoogle();

            Console.ReadKey();


        }

        public static async Task ConnectToGoogle()
        {

            #region firebase

            // using firebase
            // nuGet FireBaseAdmin
            //try
            //{
            //    // Initialize the default app
            //    var defaultApp = FirebaseApp.Create(new AppOptions()
            //    {
            //        Credential = GoogleCredential.FromFile("rb-openapi-wso2-ff69339b80da.json")
            //    });
            //    Console.WriteLine(defaultApp.Name); // "[DEFAULT]"

            //    // Retrieve services by passing the defaultApp variable...
            //    var defaultAuth = FirebaseAuth.GetAuth(defaultApp);

            //    // ... or use the equivalent shorthand notation
            //    defaultAuth = FirebaseAuth.DefaultInstance;


            //    // FirebaseMessaging.DefaultInstance
            //}
            //catch (Exception e)
            //{

            //}

            #endregion

            try
            {
                // UserCredential credential;


                var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromFile("rb-openapi-wso2-3363554821c5.json")
               .CreateScoped(new[] { Google.Apis.AnalyticsReporting.v4.AnalyticsReportingService.Scope.AnalyticsReadonly });

                using (var analytics = new Google.Apis.AnalyticsReporting.v4.AnalyticsReportingService(new Google.Apis.Services.BaseClientService.Initializer
                {
                    HttpClientInitializer = credential
                }))
                {
                    var request = analytics.Reports.BatchGet(new GetReportsRequest
                    {
                        ReportRequests = new[] {
                        new ReportRequest{
                            DateRanges = new[] { new DateRange{ StartDate = "30daysAgo", EndDate = "yesterday" } },
                            Dimensions = new[] { new Dimension{ Name = "ga:browser" } },
                            Metrics = new[] { new Metric{ Expression = "ga:sessions", Alias = "Sessions"}},
                            ViewId = "208179485"
                        }
                    }
                    });
                    var responses = request.Execute();
                    foreach (var row in responses.Reports[0].Data.Rows)
                    {
                        Console.Write(string.Join(",", row.Dimensions) + ": ");
                        foreach (var metric in row.Metrics) Console.WriteLine(string.Join(",", metric.Values));
                    }
                }
            }
            catch (Exception e)
            {

            }

            try
            {
                // using var stream = new FileStream("testfortest-app-9c18923f3f17.json", FileMode.Open, FileAccess.Read);
                using var stream = new StreamReader("rb-openapi-wso2-3363554821c5.json");
                var json = stream.ReadToEnd();
                var cr = JsonConvert.DeserializeObject<PersonalServiceAccountCred>(json);

                var xCred = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(cr.client_email)
                {
                    Scopes = new[] {
                    AnalyticsReportingService.Scope.Analytics
                }
                }
                .FromPrivateKey(cr.private_key));

                using var svc = new AnalyticsReportingService(
                    new BaseClientService.Initializer
                    {
                        HttpClientInitializer = xCred,
                        ApplicationName = "UA-15885208-7"
                    });

                // Create the DateRange object.
                DateRange dateRange = new DateRange() { StartDate = "2017-05-01", EndDate = "2017-05-31" };

                // Create the Metrics object.
                Metric sessions = new Metric { Expression = "ga:sessions", Alias = "Sessions" };

                //Create the Dimensions object.
                Dimension browser = new Dimension { Name = "ga:browser" };

                // Create the ReportRequest object.
                ReportRequest reportRequest = new ReportRequest()
                {
                    ViewId = "208179485",
                    DateRanges = new List<DateRange> { dateRange },
                    Dimensions = new List<Dimension>() { browser },
                    Metrics = new List<Metric>() { sessions }
                };

                List<ReportRequest> requests = new List<ReportRequest>();
                requests.Add(reportRequest);

                // Create the GetReportsRequest object.
                GetReportsRequest getReport = new GetReportsRequest() { ReportRequests = requests };

                // Call the batchGet method.
                GetReportsResponse response = svc.Reports.BatchGet(getReport).Execute();
            }
            catch (Exception e)
            {

            }
        }

        /// <summary>
        /// Authenticating to Google using a Service account
        /// Documentation: https://developers.google.com/accounts/docs/OAuth2#serviceaccount
        /// </summary>
        /// <param name="serviceAccountEmail">From Google Developer console https://console.developers.google.com</param>
        /// <param name="serviceAccountCredentialFilePath">Location of the .p12 or Json Service account key file downloaded from Google Developer console https://console.developers.google.com</param>
        /// <returns>AnalyticsService used to make requests against the Analytics API</returns>
        public static AnalyticsReportingService AuthenticateServiceAccount(string serviceAccountEmail, string serviceAccountCredentialFilePath)
        {
            try
            {
                if (string.IsNullOrEmpty(serviceAccountCredentialFilePath))
                    throw new Exception("Path to the service account credentials file is required.");
                if (!File.Exists(serviceAccountCredentialFilePath))
                    throw new Exception("The service account credentials file does not exist at: " + serviceAccountCredentialFilePath);
                if (string.IsNullOrEmpty(serviceAccountEmail))
                    throw new Exception("ServiceAccountEmail is required.");

                // These are the scopes of permissions you need. It is best to request only what you need and not all of them
                string[] scopes = new string[] { AnalyticsReportingService.Scope.Analytics };             // View your Google Analytics data

                // For Json file
                if (Path.GetExtension(serviceAccountCredentialFilePath).ToLower() == ".json")
                {
                    GoogleCredential credential;
                    using (var stream = new FileStream(serviceAccountCredentialFilePath, FileMode.Open, FileAccess.Read))
                    {
                        credential = GoogleCredential.FromStream(stream)
                             .CreateScoped(scopes);
                    }

                    // Create the  Analytics service.
                    return new AnalyticsReportingService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "AnalyticsReporting Service account Authentication Sample",
                    });
                }
                else if (Path.GetExtension(serviceAccountCredentialFilePath).ToLower() == ".p12")
                {   // If its a P12 file

                    var certificate = new X509Certificate2(serviceAccountCredentialFilePath, "notasecret", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
                    var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
                    {
                        Scopes = scopes
                    }.FromCertificate(certificate));

                    // Create the  AnalyticsReporting service.
                    return new AnalyticsReportingService(new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credential,
                        ApplicationName = "AnalyticsReporting Authentication Sample",
                    });
                }
                else
                {
                    throw new Exception("Unsupported Service accounts credentials.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Create service account AnalyticsReportingService failed" + ex.Message);
                throw new Exception("CreateServiceAccountAnalyticsReportingFailed", ex);
            }
        }

        /// <summary>
        /// Returns the Analytics data. 
        /// Documentation https://developers.google.com/analyticsreporting/v4/reference/reports/batchGet
        /// Generation Note: This does not always build corectly.  Google needs to standardise things I need to figuer out which ones are wrong.
        /// </summary>
        /// <param name="service">Authenticated AnalyticsReporting service.</param>  
        /// <param name="body">A valid AnalyticsReporting v4 body.</param>
        /// <returns>GetReportsResponseResponse</returns>
        public static GetReportsResponse BatchGet(AnalyticsReportingService service, GetReportsRequest body)
        {
            try
            {
                // Initial validation.
                if (service == null)
                    throw new ArgumentNullException("service");
                if (body == null)
                    throw new ArgumentNullException("body");

                // Make the request.
                return service.Reports.BatchGet(body).Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Request Reports.BatchGet failed.", ex);
            }
        }


    }

    public static class SampleHelpers
    {

        /// <summary>
        /// Using reflection to apply optional parameters to the request.  
        /// 
        /// If the optonal parameters are null then we will just return the request as is.
        /// </summary>
        /// <param name="request">The request. </param>
        /// <param name="optional">The optional parameters. </param>
        /// <returns></returns>
        public static object ApplyOptionalParms(object request, object optional)
        {
            if (optional == null)
                return request;

            System.Reflection.PropertyInfo[] optionalProperties = (optional.GetType()).GetProperties();

            foreach (System.Reflection.PropertyInfo property in optionalProperties)
            {
                // Copy value from optional parms to the request.  They should have the same names and datatypes.
                System.Reflection.PropertyInfo piShared = (request.GetType()).GetProperty(property.Name);
                if (property.GetValue(optional, null) != null) // TODO Test that we do not add values for items that are null
                    piShared.SetValue(request, property.GetValue(optional, null), null);
            }

            return request;
        }

        public static void StartProcess()
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = false;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            cmd.Start();

            cmd.StandardInput.WriteLine("ng --version");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }
    }
}
