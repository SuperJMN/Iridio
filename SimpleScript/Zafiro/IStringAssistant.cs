using System;

namespace SimpleScript.Zafiro
{
    public interface IStringAssistant
    {
        string ToString();
        void Print(AppendableString str);
        void TabPrint(AppendableString str);
        void Indentate(Action action);
        void NewLine();
        void IncreaseIndent();
        void DecreaseIndent();
    }
}