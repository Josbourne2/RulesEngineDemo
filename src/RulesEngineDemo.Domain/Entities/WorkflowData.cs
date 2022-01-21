using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RulesEngineDemo.Domain.Entities
{
    /// <summary>
    /// WorkflowData has convenience methods (e.g. Lists) for RE Workflows
    /// </summary>
    public class WorkflowData : Workflow
    {
        /// <summary>
        /// Reserved for Database / Entity Framework implementations
        /// </summary>
        [JsonIgnore]
        public int? Id { get; set; }
        public new List<RuleData> Rules { get; set; }
        public new List<ScopedParamData> GlobalParams { get; set; }
        public new string WorkflowName { get; set; }
        [JsonIgnore]
        public int Seq { get; set; }
    }
}
