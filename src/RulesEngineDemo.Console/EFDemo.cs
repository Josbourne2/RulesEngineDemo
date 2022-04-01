// Copyright (c) Microsoft Corporation.
//  Licensed under the MIT License.


using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RulesEngineDemo.Domain;
using System;
using System.Collections.Generic;
using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using static RulesEngine.Extensions.ListofRuleResultTreeExtension;
//using Microsoft.EntityFrameworkCore;
using RulesEngineDemo.Domain.Entities;
using RulesEngineDemo.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RulesEngine.Models;
using RulesEngine.Actions;

namespace RulesEngineDemo.Console
{
    public class EFDemo
    {
        public void Run()
        {

            System.Console.WriteLine($"Running {nameof(EFDemo)}....");
            var basicInfo = "{\"name\": \"hello\",\"email\": \"abcy@xyz.com\",\"creditHistory\": \"good\",\"country\": \"canada\",\"loyaltyFactor\": 3,\"totalPurchasesToDate\": 10000}";
            var orderInfo = "{\"totalOrders\": 5,\"recurringItems\": 2}";
            var telemetryInfo = "{\"noOfVisitsPerMonth\": 10,\"percentageOfBuyingToVisit\": 15}";

            var converter = new ExpandoObjectConverter();

            dynamic input1 = JsonConvert.DeserializeObject<ExpandoObject>(basicInfo, converter);
            dynamic input2 = JsonConvert.DeserializeObject<ExpandoObject>(orderInfo, converter);
            dynamic input3 = JsonConvert.DeserializeObject<ExpandoObject>(telemetryInfo, converter);

            var inputs = new dynamic[]
                {
                    input1,
                    input2,
                    input3
                };

            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "Discount.json", SearchOption.AllDirectories);
            if (files == null || files.Length == 0)
                throw new Exception("Rules not found.");

            var fileData = File.ReadAllText(files[0]);
            var workflows = JsonConvert.DeserializeObject<List<Workflow>>(fileData);

            var workflowContainers = new List<WorkflowContainer>();
            foreach(var workflow in workflows)
            {
                workflowContainers.Add(new WorkflowContainer() { Name=workflow.WorkflowName, Workflow=workflow });
            }

            var options = new DbContextOptionsBuilder<RulesEngineDbContext>()
                .UseSqlServer(@"Server=localhost;Database=RulesEngineDemoDB;Trusted_Connection=True")
                .Options;

            var factory = new PooledDbContextFactory<RulesEngineDbContext>(options);

            var db = factory.CreateDbContext();

            if(!db.Workflows.Any())
            {
                db.Workflows.AddRange(workflowContainers);
                db.SaveChanges();
            }

            var wvcFromDb = db.Workflows.ToArray();

            Workflow[] workflowsToProcess = new Workflow[wvcFromDb.Length];
           for(int i = 0; i < wvcFromDb.Length; i++)
            {
                workflowsToProcess[i]=wvcFromDb[i].Workflow;
            }

            var reSettings = new ReSettings
            {
                CustomActions = new Dictionary<string, Func<ActionBase>>{
                                          {"SendEmail", () => new SendEmailAction() }
                                      }
            };

            var bre = new RulesEngine.RulesEngine(workflowsToProcess, null, reSettings);

            string discountOffered = "No discount offered.";

            List<RuleResultTree> resultList = bre.ExecuteAllRulesAsync("Discount", inputs).Result;

            //foreach (var ruleResult in resultList)
            //{
            //    if (ruleResult.ActionResult != null)
            //    {
            //        System.Console.WriteLine(ruleResult.ActionResult.Output); //ActionResult.Output contains the evaluated value of the action
            //    }
            //}




            resultList.OnSuccess((eventName) =>
            {
                discountOffered = $"Discount offered is {eventName} % over MRP.";
            });

            resultList.OnFail(() =>
            {
                discountOffered = "The user is not eligible for any discount.";
            });

            System.Console.WriteLine(discountOffered);
        }
    }
}
