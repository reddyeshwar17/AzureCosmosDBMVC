using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;

namespace KeyVaultClienttest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Authenticate().Result.AccessToken);           
            //without certificate AD keys are stored in app
            //solution  use certificate base authentication
            Program program = new Program();
            var res = program.Get();


            //CertificateHelper.GetCert();
            //var kv = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(Utils.GetAccessToken));
            Console.ReadLine();
        }
        public async Task<string> Get()
        {          

            //key vault secrets of azure
            var keyVaultClient = new KeyVaultClient(AuthenticateVault);
            var result = await keyVaultClient.GetSecretAsync("https://mcmiotest.vault.azure.net/secrets/MCIOTestSecret/58e2db7fcd144f72823568e6e220dfe7");
            Console.WriteLine(result.Value);
            Console.ReadKey();
            return result.Value;
        }
        private async Task<string> AuthenticateVault2(string authority, string resource, string scope)
        {
            //authenticate AD, app id and endpoint key value
            var clientId = new ClientCredential("6af2259a-367f-463c-bafe-411c3fdcfb09", "pVc+2Btuvjl1702XiXmnYU8BboJZHTGtqWBewhB0OOs=");
            //var credentials = new UserAssertion("girijasrtj4@gmail.com", "Mercury@2018");
            var authenticationContext = new AuthenticationContext(authority);
            var results = await authenticationContext.AcquireTokenAsync(resource, clientId);
            return results.AccessToken;
        }

        //no use for now
        private static async Task<AuthenticationResult> AuthenticateVault1()
        {
            string userName = "girijasrtj4@gmail.com";
            string password = "";
            string clientId = "6af2259a-367f-463c-bafe-411c3fdcfb09";
            var credentials = new UserPasswordCredential(userName, password);
            var authenticationContext = new AuthenticationContext("https://login.windows.net/common/oauth2/authorize/");
            var result = await authenticationContext.AcquireTokenAsync("https://mcmiotest.vault.azure.net/secrets/MCIOTestSecret/58e2db7fcd144f72823568e6e220dfe7", clientId, credentials);
            return result;
        }

        //without certificate
        private async Task<string> AuthenticateVault(string authority, string resource, string scope)
        {
            //THUMB PRINT VALUE
            var certificate = FindCertificateByThumbprint("3C6FB987E9BCA51E4AB02D8CF21691DD4057127A");
            //New client id from app registrations (keyvault Webapplication)
            var clientAssertionCert = new ClientAssertionCertificate("0c182bce-e608-413c-990c-6980f1ae18e0", certificate);            
            var authenticationContext = new AuthenticationContext(authority);
            var results = await authenticationContext.AcquireTokenAsync(resource, clientAssertionCert);
            return results.AccessToken;
        }

        //with certificate
        public static X509Certificate2 FindCertificateByThumbprint(string findValue)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint,
                    findValue, false); // Don't validate certs, since the test root isn't installed.
                if (col == null || col.Count == 0)
                    throw new Exception("Certificate not installed on store");
                return col[0];
            }
            finally
            {
                store.Close();
            }
        }
    }

    //from  MS Docs
    // https://docs.microsoft.com/en-us/azure/key-vault/key-vault-use-from-web-application
    public static class CertificateHelper
    {
        public static X509Certificate2 FindCertificateByThumbprint(string findValue)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection col = store.Certificates.Find(X509FindType.FindByThumbprint,
                    findValue, false); // Don't validate certs, since the test root isn't installed.
                if (col == null || col.Count == 0)
                    throw new Exception("Certificate not installed on store");
                return col[0];
            }
            finally
            {
                store.Close();
            }
        }

        public static ClientAssertionCertificate AssertionCert { get; set; }

        public static void GetCert()
        {
            var clientAssertionCertPfx = CertificateHelper.FindCertificateByThumbprint(ConfigurationManager.AppSettings["thumbprint"]);
            AssertionCert = new ClientAssertionCertificate(ConfigurationManager.AppSettings["clientid"], clientAssertionCertPfx);
        }

        public static async Task<string> GetAccessToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, AssertionCert);
            return result.AccessToken;
        }

    }

    
}
