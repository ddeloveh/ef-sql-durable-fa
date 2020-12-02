using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace deloveh.david.EfSqlDurableFa
{
    public static class AddEmployeeFunc
    {
        [FunctionName(nameof(AddEmployee_HttpStart))]
        public static async Task<HttpResponseMessage> AddEmployee_HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            //Get the employee object from the HTTP body
            var content = req.Content.ReadAsStringAsync().Result;
            var employee = JsonConvert.DeserializeObject<Employee>(content);

            string instanceId = await starter.StartNewAsync(nameof(AddEmployee_Orchestrate), employee);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }

        [FunctionName(nameof(AddEmployee_Orchestrate))]
        public static async Task<List<Guid>> AddEmployee_Orchestrate(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var employee = context.GetInput<Employee>();
            var outputs = new List<Guid>();

            //Add to commit the new row
            outputs.Add(await context.CallActivityAsync<Guid>(nameof(AddToDB_Activity), employee));

            //The second add should return a Guid.Empty because it is a duplicate. Just simulates re-execution of activity.
            outputs.Add(await context.CallActivityAsync<Guid>(nameof(AddToDB_Activity), employee));

            //The outputs can be seen with the statusQueryGetUri returned by the HttpStart trigger
            return outputs;
        }

        [FunctionName(nameof(AddToDB_Activity))]
        public static Guid AddToDB_Activity([ActivityTrigger] Employee employee, ILogger log)
        {
            using (var db = new EmployeeContext())
            {
                if (db.Employees.Find(employee.EmployeeID) == null)
                {
                    db.Employees.Add(employee);
                    db.SaveChanges();
                    return employee.EmployeeID;
                }
                else
                {
                    return Guid.Empty;
                }
            }
        }
    }
}