using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tracer
{
    static class XmlTraceResultSerialization
    {
        public static string Serialize(TraceResult result)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(TraceResult));
            FileStream fs = new FileStream("./temp.xml", FileMode.OpenOrCreate);
            formatter.Serialize(fs, result);
            fs.Close();
            string res = File.ReadAllText("./temp.xml");
            File.Delete("./temp.xml");
            return res;
        }
    }
}
