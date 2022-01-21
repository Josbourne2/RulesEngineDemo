using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RulesEngineDemo.Domain.Entities
{
    public class RuleData : Rule
    {
        /// <summary>
        /// Reserved for Database / Entity Framework implementations
        /// </summary>
        [JsonIgnore]
        public int? Id { get; set; }
        public new List<RuleData> Rules { get; set; }
        public new List<ScopedParamData> LocalParams { get; set; }
        [JsonIgnore]
        public bool? IsSuccess { get; set; }
        [JsonIgnore]
        public string ExceptionMessage { get; set; }
        [JsonIgnore]
        public int Seq { get; set; }
    }
}
