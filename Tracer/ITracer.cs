using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public interface ITracer
    {
        void StartTrace(); // вызывается в начале замеряемого метода
        void StopTrace(); // вызывается в конце замеряемого метода
        TraceResult GetTraceResult(); // получить результаты измерений
    }
}
