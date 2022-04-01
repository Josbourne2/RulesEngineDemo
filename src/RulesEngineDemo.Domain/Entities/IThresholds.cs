using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineDemo.Domain.Entities
{
    public interface IThresholds
    {
        public Dictionary<string,int> IntThresholds { get; set; }
    }
}
