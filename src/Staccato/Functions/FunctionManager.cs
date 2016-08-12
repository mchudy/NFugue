using System.Collections.Generic;

namespace Staccato.Functions
{
    public class FunctionManager
    {
        private readonly IDictionary<string, IPreprocessorFunction> preprocessorFunctions = new Dictionary<string, IPreprocessorFunction>();
        private readonly IDictionary<string, ISubparserFunction> subparserFunctions = new Dictionary<string, ISubparserFunction>();

        public void addPreprocessorFunction(IPreprocessorFunction function)
        {
            foreach (string name in function.GetNames())
            {
                preprocessorFunctions[name.ToUpper()] = function;
            }
        }

        public void removePreprocessorFunction(IPreprocessorFunction function)
        {
            foreach (string name in function.GetNames())
            {
                preprocessorFunctions.Remove(name.ToUpper());
            }
        }

        public IPreprocessorFunction GetPreprocessorFunction(string name)
        {
            return preprocessorFunctions[name.ToUpper()];
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