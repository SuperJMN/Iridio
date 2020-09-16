using System;

namespace SimpleScript.Zafiro
{
    public interface IStringAssistant
    {
        string ToString();
        void Print(FormatlessString str);
        void TabPrint(FormatlessString str);
        void Indentate(Action action);
        void NewLine();
        void IncreaseIndent();
        void DecreaseIndent();
    }
}