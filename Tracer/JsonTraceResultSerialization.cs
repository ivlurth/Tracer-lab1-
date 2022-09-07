using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tracer
{
    public class JsonTraceResultSerialization
    {
        public static string Serialize(TraceResult result)
        {
            string res = JsonSerializer.Serialize<TraceResult>(result);
            return res;
        }
    }
}
