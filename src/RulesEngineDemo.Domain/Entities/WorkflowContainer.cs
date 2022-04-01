using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngineDemo.Domain.Entities
{
    public class WorkflowContainer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Workflow Workflow { get; set; }
    }
}
