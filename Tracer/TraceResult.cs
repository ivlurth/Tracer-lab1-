using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tracer
{

    [Serializable]
    public class TraceResult
    {
        public List<TracingThread> threads { get; set; } 

        public TraceResult()
        {
            threads = new();
        }
    }


    [Serializable]
    public class TracingThread
    {

        [XmlAttribute]
        public int id { get; set; }


        [XmlAttribute]
        public long time { get; set; }

        public List<TracingMethod> methods { get; set; }

        public TracingThread()
        {
            this.id = Thread.CurrentThread.ManagedThreadId;
            this.time = 0;
            this.methods = new();            
        }


        public override bool Equals(object obj)
        {
            try
            {
                if (((TracingThread)obj).id == this.id) return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            return false;
        }

        public TracingMethod GetMethod(TracingMethod method)
        { // ищет метод в списке методов потока
            foreach (var m in this.methods)
            {
                if (method.Equals(m)) return m;
                if (m.GetMethod(method) != null) return m.GetMethod(method);
            }
            return null;
        }

        public TracingMethod GetStartedMethod(TracingMethod method)
        { // ищет метод в списке методов потока
            foreach (var m in this.methods)
            {
                if (method.Equals(m) && (m.time == 0)) return m;
                if (m.GetMethod(method) != null) return m.GetStartedMethod(method);
            }
            return null;
        }

    }


    [Serializable]
    public class TracingMethod
    {

        [XmlAttribute]
        public string name { get; set; }

        [XmlAttribute]
        public string className { get; set; }

        [XmlAttribute]
        public long time { get; set; }

        public List<TracingMethod> methods { get; set; }

        public TracingMethod(string name, string className)
        {
            methods = new();
            this.name = name;
            this.className = className;
            this.time = 0;
        }

        public TracingMethod()
        {
            methods = new();
        }

        public override bool Equals(object obj)
        {
            try
            {
                if ((((TracingMethod)obj).name == this.name) &&
                    (((TracingMethod)obj).className == this.className)) return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            return false;
        }

        public TracingMethod GetMethod(TracingMethod method)
        { // ищет метод в списке методов метода 
            foreach (var m in this.methods)
            {
                if (method.Equals(m)) return m;
                if (m.GetMethod(method) != null) return m.GetMethod(method);
            }
            return null;
        }

        public TracingMethod GetStartedMethod(TracingMethod method)
        { // ищет метод в списке методов метода
            foreach (var m in this.methods)
            {
                if (method.Equals(m) && (m.time == 0)) return m;
                if (m.GetMethod(method) != null) return m.GetStartedMethod(method);
            }
            return null;
        }
    }
}
