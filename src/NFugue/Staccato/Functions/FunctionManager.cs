using System.Collections.Generic;

namespace NFugue.Staccato.Functions
{
    public class FunctionManager
    {
        private static FunctionManager instance;

        private FunctionManager() { }

        public static FunctionManager Instance => instance ?? (instance = new FunctionManager());

        private readonly IDictionary<string, IPreprocessorFunction> preprocessorFunctions = new Dictionary<string, IPreprocessorFunction>();
        private readonly IDictionary<string, ISubparserFunction> subparserFunctions = new Dictionary<string, ISubparserFunction>();

        public void AddPreprocessorFunction(IPreprocessorFunction function)
        {
            foreach (string name in function.GetNames())
            {
                preprocessorFunctions[name.ToUpper()] = function;
            }
        }

        public void RemovePreprocessorFunction(IPreprocessorFunction function)
        {
            foreach (string name in function.GetNames())
            {
                preprocessorFunctions.Remove(name.ToUpper());
            }
        }

        public IPreprocessorFunction GetPreprocessorFunction(string name)
        {
            IPreprocessorFunction value;
            if (preprocessorFunctions.TryGetValue(name.ToUpper(), out value))
            {
                return value;
            }
            return null;
        }

        public void AddSubparserFunction(ISubparserFunction function)
        {
            foreach (string name in function.GetNames())
            {
                subparserFunctions[name.ToUpper()] = function;
            }
        }

        public void RemoveSubparserFunction(ISubparserFunction function)
        {
            foreach (string name in function.GetNames())
            {
                subparserFunctions.Remove(name.ToUpper());
            }
        }

        public ISubparserFunction GetSubparserFunction(string name)
        {
            return subparserFunctions[name.ToUpper()];
        }
    }
}