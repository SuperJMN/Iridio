using System;
using System.Text;

namespace SimpleScript.Zafiro
{
    public class StringAssistant : IStringAssistant
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();
        private int indentLevel;

        private string Indent => GetIndent(indentLevel);

        public void IncreaseIndent()
        {
            Check(indentLevel++);
        }

        private void Check(int i)
        {
            if (i < 0) throw new InvalidOperationException("Indent level cannot be decreased");
        }

        public void DecreaseIndent()
        {
            Check(indentLevel--);
        }

        private static string GetIndent(int i)
        {
            var result = "";

            for (var j = 0; j < i; j++) result += "\t";

            return result;
        }

        public override string ToString()
        {
            return stringBuilder.ToString();
        }

        public void Print(AppendableString str)
        {
            stringBuilder.Append(str);
        }

        public void TabPrint(AppendableString str)
        {
            stringBuilder.Append($"{Indent}{str}");
        }

        public void Indentate(Action action)
        {
            IncreaseIndent();
            action();
            DecreaseIndent();
        }

        public void NewLine()
        {
            stringBuilder.AppendLine();
        }
    }
}