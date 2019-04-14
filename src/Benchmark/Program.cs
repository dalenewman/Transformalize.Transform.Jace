using Autofac;
using Transformalize.Contracts;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Transformalize.Containers.Autofac;
using Transformalize.Logging;
using Transformalize.Providers.Bogus.Autofac;
using Transformalize.Transforms.Jace.Autofac;
using Transformalize.Configuration;

namespace Benchmark {


   [LegacyJitX64Job]
   public class Benchmarks {

      [Benchmark(Baseline = true, Description = "5000 rows")]
      public void TestRows() {
         var logger = new NullLogger();

         using (var outer = new ConfigurationContainer(new BogusModule(), new JaceModule()).CreateScope(@"files\bogus.xml?Size=5000", logger)) {
            var process = outer.Resolve<Process>();
            using (var inner = new Container(new BogusModule(), new JaceModule()).CreateScope(process, logger)) {
               var controller = inner.Resolve<IProcessController>();
               controller.Execute();
            }
         }
      }

      [Benchmark(Baseline = false, Description = "5000 rows 1 jace")]
      public void CSharpRows() {
         var logger = new NullLogger();
         using (var outer = new ConfigurationContainer(new BogusModule(), new JaceModule()).CreateScope(@"files\bogus-lambda-parser.xml?Size=5000", logger)) {
            var process = outer.Resolve<Process>();
            using (var inner = new Container(new BogusModule(), new JaceModule()).CreateScope(process, logger)) {
               var controller = inner.Resolve<IProcessController>();
               controller.Execute();
            }
         }
      }

   }

   public class Program {
      private static void Main(string[] args) {
         BenchmarkRunner.Run<Benchmarks>();
      }
   }
}
