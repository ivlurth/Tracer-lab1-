using System.Diagnostics;
using Tracer;

namespace TestProjectTracer
{

    [TestClass]
    public class UnitTest1

    {
         private static void Check<T>(T t, T u, string hint)
        {
            if (!t.Equals(u))
            {
                Console.Error.Write("Testing failed: {0} != {1} ", t, u);
                if (hint.Length > 0)
                {
                    Console.Error.WriteLine(" hint: " + hint);
                }
                throw new SystemException(hint);
            }
        }

        private static ITracer tracer;

        [TestMethod]
        static void TestMultipleThreads()
        {
            tracer = new Tracer.Tracer();

            Task task1 = new Task(() => (new Foo(tracer)).MethodFoo());
            task1.Start();

            Task task2 = new Task(() => (new Bar(tracer)).MethodBar());
            task2.Start();

            task1.Wait();
            task2.Wait();

            Check(tracer.GetTraceResult().threads.Count, 2, "Number of threads is 3");
        }


        [TestMethod]
        static void TestThreadTime()
        {
            tracer = new Tracer.Tracer();

            Task task1 = new Task(() => (new Foo(tracer)).MethodFoo());
            task1.Start();

            Task task2 = new Task(() => (new Foo(tracer)).MethodFoo());
            task2.Start();

            task1.Wait();
            task2.Wait();

            long time1 = tracer.GetTraceResult().threads[0].time;
            Check(time1 >= 800, time1 <= 1200, "Task1 is expected to last 1000ms");

            long time2 = tracer.GetTraceResult().threads[1].time;
            Check(time2 >= 800, time2 <= 1200, "Task1 is expected to last 1000ms");
        }


        [TestMethod]
        static void TestManyThreadsAtOnce()
        {
            tracer = new Tracer.Tracer();

            Task[] tasks = new Task[5];
            for (int i = 0; i < 5; ++i)
            {
                tasks[i] = new Task(() => (new Foo(tracer)).MethodFoo());
                tasks[i].Start();
            }
            Task.WaitAll(tasks);

            TraceResult tr = tracer.GetTraceResult();
            Check(tr.threads.Count, 5, "5 threads are expected");
        }

        [TestMethod]
        static void TestMethodNames()
        {
            tracer = new Tracer.Tracer();

            Task task1 = new Task(() => {
                (new Foo(tracer)).MethodFoo();
                (new Bar(tracer)).MethodBar();
            });
            task1.Start();
            task1.Wait();

            TraceResult tr = tracer.GetTraceResult();
            Check(tr.threads[0].methods[0].name, "Void MethodFoo()", "Void MethodFoo() is expected");
            Check(tr.threads[0].methods[1].name, "Void MethodBar()", "Void MethodFoo() is expected");
        }


        [TestMethod]
        static void TestMethodClasses()
        {
            tracer = new Tracer.Tracer();

            Task task1 = new Task(() => {
                (new Foo(tracer)).MethodFoo();
                (new Bar(tracer)).MethodBar();
            });
            task1.Start();
            task1.Wait();

            TraceResult tr = tracer.GetTraceResult();
            Check(tr.threads[0].methods[0].className, "Foo", "Foo is expected");
            Check(tr.threads[0].methods[1].className, "Bar", "Foo is expected");
        }




    }
}