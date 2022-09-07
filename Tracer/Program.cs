namespace Tracer
{
    class Program
    {
        static void Main()
        {

            var tracer = new Tracer();
            tracer.StartTrace();

            Thread.Sleep(1000);
            var foo = new Foo(tracer);
            var bar = new Bar(tracer);

            Thread thread = new Thread(foo.MethodFoo);
            thread.Start();

            Thread thread2 = new Thread(bar.MethodBar);
            thread2.Start();

            foo.MethodFoo();
            bar.MethodBar();
            bar.MethodSleep(10);

            tracer.StopTrace();

            var result = tracer.GetTraceResult();

            XmlTraceResultSerialization.Serialize(result);
            Console.WriteLine(JsonTraceResultSerialization.Serialize(result));

            return;
        }



        class Foo
        {
            private ITracer tracer;

            public Foo(ITracer tracer)
            {
                this.tracer = tracer;
            }

            public void MethodFoo()
            {
                tracer.StartTrace();
                Thread.Sleep(100);
                tracer.StopTrace();
            }
        }

        class Bar
        {
            private ITracer tracer;

            public Bar(ITracer tracer)
            {
                this.tracer = tracer;
            }

            public void MethodBar()
            {
                tracer.StartTrace();
                Thread.Sleep(200);
                tracer.StopTrace();
            }

            public void MethodSleep(int time)
            {
                tracer.StartTrace();
                Thread.Sleep(time / 2);
                tracer.StopTrace();

                tracer.StartTrace();
                Thread.Sleep(time / 2);
                tracer.StopTrace();
            }
        }

    }
}