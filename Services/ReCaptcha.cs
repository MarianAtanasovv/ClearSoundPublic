using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClearSoundCompany.Services
{
    public class ReCaptcha
    {
        private readonly HttpClient _captchaClient;

        public ReCaptcha(HttpClient captchaClient)
        {
            _captchaClient = captchaClient;
        }

        public async Task<bool> IsValid(string captcha)
        {
            try
            {
                var postTask = await _captchaClient
                    .PostAsync($"", new StringContent(""));
                var result = await postTask.Content.ReadAsStringAsync();
                var resultObject = JObject.Parse(result);
                dynamic success = resultObject["success"];
                return (bool)success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}