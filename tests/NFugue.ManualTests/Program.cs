using NFugue.ManualTests.Utils;

namespace NFugue.ManualTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var testRunner = new ManualTestsRunner();
            testRunner.RunTests();
        }
    }
}
