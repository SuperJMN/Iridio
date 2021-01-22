using System;

namespace Iridio.Common.Utils
{
    public interface IStringAssistant
    {
        string ToString();
        void Print(FormatlessString str);
        void TabPrint(FormatlessString str);
        void Indent(Action action);
        void NewLine();
        void IncreaseIndent();
        void DecreaseIndent();
    }
}