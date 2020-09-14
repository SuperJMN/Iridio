using System;
using System.Text;

namespace SimpleScript.Zafiro
{
    public class StringAssistant
    {
        private readonly StringBuilder stringBuilder = new StringBuilder();
        private int indentLevel;

        private string Indent => GetIndent(indentLevel);

        public void Append(AppendableString str)
        {
            stringBuilder.Append($"{Indent}{str}");
        }

        public void AppendLine(AppendableString str)
        {
            stringBuilder.AppendLine($"{Indent}{str}");
        }

        public void Add(AppendableString str)
        {
            stringBuilder.Append(str);
        }

        public void NewLineWith(AppendableString str)
        {
            stringBuilder.AppendLine();
            stringBuilder.Append($"{Indent}{str}");
        }

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
    }
}