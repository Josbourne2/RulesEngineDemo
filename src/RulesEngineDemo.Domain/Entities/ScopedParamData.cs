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
    /// ScopedParamData - inherited class to continue naming convention / reserve future functionality
    /// </summary>
    public class ScopedParamData : ScopedParam
    {
        /// <summary>
        /// Reserved for Database / Entity Framework implementations
        /// </summary>
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int Seq { get; set; }
    }
}
