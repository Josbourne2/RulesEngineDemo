using RulesEngine.Actions;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineDemo.Domain.Entities
{
    public class SendEmailAction : ActionBase
    {
        public SendEmailAction()
        {

        }

        public override ValueTask<object> Run(ActionContext context, RuleParameter[] ruleParameters)
        {
            var customInput = context.GetContext<string>("customContextInput");
            //Add your custom logic here and return a ValueTask
            Console.WriteLine(customInput);
            return new ValueTask<object>();
        }
    }
}
