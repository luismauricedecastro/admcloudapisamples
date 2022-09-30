using System.Net.Http.Headers;
using System.Text.Json;

namespace csharpsample
{
    internal class Program
    {
        //Instructions
        //Create an API integration in Adm Cloud
        //Fill this variables
        //Run!

        private const string EMAIL = "ENTER MAIL HERE";
        private const string PASSWORD = "ENTER PASSWORD HERE";
        private const string APPID = "ENTER API ID HERE";
        private const string DB = "ENTER DB NAME HERE";
        private const string ROLE = "ENTER USER ROLE NAME HERE";

        static async Task Main(string[] args)
        {
            try
            {
                var customer_list = await GetCustomerListAsync();

                foreach (var customer in customer_list)
                {
                    System.Console.WriteLine(customer.Name);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.GetBaseException().Message);
            }

            System.Console.ReadLine();
        }

        public async static Task<List<Customer>> GetCustomerListAsync()
        {
            //Url to call
            var url = $"https://api.admcloud.net/api/Customers?skip=0&appid={APPID}&company={DB}&role={ROLE}";

            //Concatenate user and password: (BASIC AUTHENTICATION)
            //https://en.wikipedia.org/wiki/Basic_access_authentication
            var auth_string = $"{EMAIL}:{PASSWORD}";

            //Get Bytes
            var base_64_string = System.Text.ASCIIEncoding.ASCII.GetBytes(auth_string);

            //Convert to Base 64 string
            var header = Convert.ToBase64String(base_64_string);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", header);

            //Call Service
            var response = await httpClient.GetAsync(url);

            //Check Status Code
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new ApplicationException(message: $"Error calling service. Status Code: {response.StatusCode}");

            //Get Response Content
            var apiResponseString = await response.Content.ReadAsStringAsync();

            //Deserialize Json
            var api_returned_msg = JsonSerializer.Deserialize<ApiResponse>(apiResponseString);

            if (api_returned_msg == null)
                throw new ApplicationException(message: "Deserialization error");

            //Check for errors
            if (!api_returned_msg.success)
                throw new ApplicationException(api_returned_msg.message);

            //Deserialize Data
            string data_json_string = api_returned_msg.data.ToString();
            var customer_list = JsonSerializer.Deserialize<List<Customer>>(data_json_string);

            return customer_list;
        }
    }
}