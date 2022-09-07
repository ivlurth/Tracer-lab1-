using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracer
{
    public class Tracer : ITracer
    {
        private TraceResult result = new();
        private Dictionary<TracingMethod, long> timeStart = new();

        public void StartTrace()
        {
            var currentThread = new TracingThread();
            if (!result.threads.Contains(currentThread))
            {
                lock (result)
                {
                    result.threads.Add(currentThread);
                }
            }
            currentThread = GetCurrentThread();

            var currentMethod = new TracingMethod(GetCurrentMethodName(), GetCurrentClassName());
            var baseMethod = new TracingMethod(GetBaseMethodName(), GetBaseClassName());
            baseMethod = currentThread.GetMethod(baseMethod);

            if (baseMethod != null)
            {
                baseMethod.methods.Add(currentMethod);
            }
            else
            {
                currentThread.methods.Add(currentMethod);
            }

            lock (timeStart)
            {
                timeStart[currentMethod] = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
        }

        public void StopTrace()
        {
            var currentThread = GetCurrentThread();
            var currentMethod = new TracingMethod(GetCurrentMethodName(), GetCurrentClassName());

            currentMethod = currentThread.GetStartedMethod(currentMethod);

            if (!timeStart.ContainsKey(currentMethod))
            {
                throw new Exception("StopTace был вызван для метода, для которого не был вызван StartStart.");
            }

            long timeFinish = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            currentMethod.time = timeFinish - timeStart[currentMethod];

            lock (timeStart)
            {
                timeStart.Remove(currentMethod);
            }

            var baseMethod = new TracingMethod(GetBaseMethodName(), GetBaseClassName());
            baseMethod = currentThread.GetMethod(baseMethod);
            if (baseMethod == null)
            {
                foreach (var method in currentThread.methods)
                {
                    currentThread.time += method.time;
                }
            }
        }


        public TraceResult GetTraceResult()
        {
            return result;
        }

        private TracingThread GetCurrentThread()
        {
            return result.threads.Find((arg) => arg.id == Thread.CurrentThread.ManagedThreadId);
        }

       
        private string GetCurrentMethodName()
        {
            return (new StackTrace()).GetFrame(2).GetMethod().ToString();
        }

        private string GetCurrentClassName()
        {
            return (new StackTrace()).GetFrame(2).GetMethod().ReflectedType.Name.ToString();
        }

        private string GetBaseMethodName()
        {
            return (new StackTrace()).GetFrame(3)?.GetMethod().ToString();
        }

        private string GetBaseClassName()
        {
            return (new StackTrace()).GetFrame(3)?.GetMethod().ReflectedType.Name.ToString();
        }
    }

}

