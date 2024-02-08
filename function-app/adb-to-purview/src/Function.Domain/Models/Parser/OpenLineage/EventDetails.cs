using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Function.Domain.Models.Parser.OpenLineage
{
    public class EventDetails(string runId, string jobName)
    {
        public string RunId { get; init; } = runId;
        public string JobName { get; init; } = jobName;
    }
}