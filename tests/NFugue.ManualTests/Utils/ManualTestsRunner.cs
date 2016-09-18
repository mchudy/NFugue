using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NFugue.ManualTests.Utils
{
    public class ManualTestsRunner
    {
        private const ConsoleColor successColor = ConsoleColor.DarkGreen;
        private const ConsoleColor errorColor = ConsoleColor.Red;
        private const ConsoleColor titleColor = ConsoleColor.Yellow;

        private List<MethodInfo> testMethods;
        private int testsFailed = 0;
        private int testsPassed = 0;
        private int testsRun = 0;

        public ManualTestsRunner()
        {
            LoadTestClasses();
        }

        private void LoadTestClasses()
        {
            testMethods = typeof(ManualTestsRunner).Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttributes(typeof(ManualTestAttribute), false).Any()
                            && !m.GetParameters().Any())
                .ToList();
        }

        public void RunTests()
        {
            for (int i = 0; i < testMethods.Count; i++)
            {
                var method = testMethods[i];
                RunTest(method, i + 1);
            }
            PrintSummary();
            Console.ReadLine();
        }

        private void RunTest(MethodInfo method, int testNumber)
        {
            var type = method.DeclaringType;
            if (type != null)
            {
                var typeInstance = Activator.CreateInstance(type);
                var attribute = method.GetCustomAttribute<ManualTestAttribute>();

                PrintBeforeTest(attribute, testNumber);

                method.Invoke(typeInstance, null);

                GetTestResult();
                testsRun++;
            }
        }

        private void GetTestResult()
        {
            do
            {
                string prompt = "Did it pass? (Y/n) ";
                Console.Write(prompt);

                string answer = Console.ReadLine() ?? "";
                Console.SetCursorPosition(prompt.Length + answer.Length + 1, Console.CursorTop - 1);

                if (string.IsNullOrWhiteSpace(answer) || answer.Equals("y", StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleEx.WriteLine("Test passed", successColor);
                    testsPassed++;
                    break;
                }
                if (answer.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleEx.WriteLine("Test failed", errorColor);
                    testsFailed++;
                    break;
                }
            } while (true);
            Console.WriteLine("------------------------------------------------------\n");
        }

        private static void PrintBeforeTest(ManualTestAttribute attribute, int testNumber)
        {
            Console.Write($"Test #{testNumber} - ");
            ConsoleEx.WriteLine(attribute.Title, titleColor);
            if (!string.IsNullOrWhiteSpace(attribute.Description))
            {
                Console.WriteLine(attribute.Description);
            }
            Console.WriteLine();
        }

        private void PrintSummary()
        {
            Console.Write($"\nSUMMARY\n{testsRun} tests run in total, ");
            ConsoleEx.Write($"{testsPassed} passed, ", successColor);
            ConsoleEx.WriteLine($"{testsFailed} failed", errorColor);
        }
    }
}
