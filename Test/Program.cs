namespace TestProject
{
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
            Thread.Sleep(1000);
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