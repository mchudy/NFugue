using NFugue.Patterns;
using Staccato.Extensions;
using Staccato.Functions;
using Staccato.Preprocessors;
using System;
using System.Text;

namespace Staccato.Subparsers
{
    public class FunctionSubparser : ISubparser
    {
        public const char FunctionChar = ':';


        public bool Matches(string music)
        {
            return music[0] == ':';
        }

        public TokenType GetTokenType(string tokenString)
        {
            if (tokenString[0] == FunctionChar)
            {
                return TokenType.Function;
            }
            return TokenType.UnknownToken; ;
        }

        public int Parse(string music, StaccatoParserContext context)
        {
            if (music[0] != FunctionChar) return 0;
            int posOpenParen = music.FindNextOrEnd('(');
            int posCloseParen = music.FindNextOrEnd(')', posOpenParen);
            string functionName = music.Substring(1, posOpenParen - 1);
            string parameters = music.Substring(posOpenParen + 1, posCloseParen - posOpenParen - 1);
            parameters = ParenSpacesPreprocessor.Unprocess(parameters);
            var function = FunctionManager.Instance.GetSubparserFunction(functionName);
            if (function != null)
            {
                context.Parser.OnFunctionParsed(functionName, parameters);
                function.Apply(parameters, context);
            }
            return Math.Min(posCloseParen + 1, music.Length);
        }

        public static string GenerateFunctionCall(string functionName, object val)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(FunctionChar);
            sb.Append(functionName);
            sb.Append("(");
            AppendList(sb, val.ToString());
            sb.Append(")");
            return sb.ToString();
        }

        public static string GenerateFunctionCall(string functionName, params sbyte[] vals)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(FunctionChar);
            sb.Append(functionName);
            sb.Append("(");
            AppendList(sb, GetStringForPossibleArray(vals));
            sb.Append(")");
            return sb.ToString();
        }

        private static string GetStringForPossibleArray(params sbyte[] vals)
        {
            if (vals.Length == 0)
            {
                return "";
            }
            StringBuilder sb = new StringBuilder();
            foreach (var val in vals)
            {
                sb.Append(val.ToString());
                sb.Append(",");
            }
            return sb.ToString().Substring(0, sb.Length - 1);
        }

        public static string GenerateParenParamIfNecessary(string functionId, string value)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(functionId);
            if (value.IndexOf(' ') + value.IndexOf('\'') == -2)
            {
                sb.Append(value);
            }
            else
            {
                sb.Append("(");
                sb.Append(value);
                sb.Append(")");
            }
            return sb.ToString();
        }

        private static void AppendList(StringBuilder sb, params object[] vals)
        {
            for (int i = 0; i < vals.Length - 1; i++)
            {
                sb.Append(vals[i]);
                sb.Append(",");
            }
            sb.Append(vals[vals.Length - 1]);
        }

    }
}