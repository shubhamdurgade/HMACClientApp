namespace HMACClientApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var clientId = "DesktopClient";
            var secretKey = "m1n2b3v4c5x6z7i8k9j0";
            var baseUrl = "https://localhost:7063";

            var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(5)
            };

            try
            {
                var employee = new
                {
                    Name = "Pranaya Rout",
                    Position = "Software Engineer",
                    Salary = 60000
                };
                // Create a new Employee (POST Request)
                var response = await HMACHelper.SendRequestAsync(
                    client,
                    HttpMethod.Post,
                    baseUrl,
                    "/api/employees",
                    clientId,
                    secretKey,
                    employee);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Employee created successfully:");
                    Console.WriteLine(responseData);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    var errorData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorData);
                }


                //Get All Employees
                response = await HMACHelper.SendRequestAsync(
                    client,
                    HttpMethod.Get,
                    baseUrl,
                    "/api/employees",
                    clientId,
                    secretKey);

                if (response.IsSuccessStatusCode)
                {
                     var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Employees retrieved successfully:");
                    Console.WriteLine(responseData);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    var errorData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorData);
                }


                //Get Employee by Id (GET Request)
                var employeeId = 1; // Assuming the employee with ID 1 exists
                response = await HMACHelper.SendRequestAsync(
                    client,
                    HttpMethod.Get,
                    baseUrl,
                    $"/api/employees/{employeeId}",
                    clientId,
                    secretKey);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Employee retrieved successfully:");
                    Console.WriteLine(responseData);
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Employee with ID {employeeId} not found.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    var errorData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorData);
                }


                // Delete Employee by Id (DELETE Request)
                response = await HMACHelper.SendRequestAsync(
                    client,
                    HttpMethod.Delete,
                    baseUrl,
                    $"/api/employees/{employeeId}",
                    clientId,
                    secretKey);

                if( response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Employee with ID {employeeId} deleted successfully.");
                }
                else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"Employee with ID {employeeId} not found.");
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    var errorData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorData);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred:");
                Console.WriteLine(ex.Message);
            }
        }
    }
}