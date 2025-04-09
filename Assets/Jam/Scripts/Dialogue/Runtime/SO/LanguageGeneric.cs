using System;
using Jam.Scripts.Dialogue.Runtime.Enums;

namespace Jam.Scripts.Dialogue.Runtime.SO
{
    [Serializable]
    public class LanguageGeneric<T>
    {
        public LanguageType LanguageType;
        public T LanguageGenericType;
    }
}