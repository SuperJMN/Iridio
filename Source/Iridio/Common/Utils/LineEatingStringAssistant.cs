using System;

namespace Iridio.Common.Utils
{
    public class LineEatingStringAssistant : IStringAssistant
    {
        private readonly IStringAssistant inner;
        private bool pendingNewline;
        private bool isEmpty = true;

        public LineEatingStringAssistant(IStringAssistant inner)
        {
            this.inner = inner;
        }

        public void Print(FormatlessString str)
        {
            Do(() => inner.Print(str));
        }

        public void TabPrint(FormatlessString str)
        {
            Do(() => inner.TabPrint(str));
        }

        private void Do(Action action)
        {
            if (pendingNewline && !isEmpty)
            {
                inner.NewLine();
            }

            pendingNewline = false;
            isEmpty = false;

            action();
        }

        public void Indent(Action action)
        {
            Do(() => inner.Indent(action));
        }

        public void NewLine()
        {
            pendingNewline = true;
        }

        public void IncreaseIndent()
        {
            inner.IncreaseIndent();
        }

        public void DecreaseIndent()
        {
            inner.DecreaseIndent();
        }

        public override string ToString()
        {
            return inner.ToString();
        }
    }
}