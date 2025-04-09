using Jam.Scripts.Dialogue.Runtime.Enums;

namespace Jam.Scripts.Dialogue.Utils
{
    public static class UseStringEventModifier
    {
        public static bool ModifierBoolCheck(StringEventModifierType eventModifier)
        {
            return eventModifier switch
            {
                StringEventModifierType.SetTrue => true,
                StringEventModifierType.SetFalse => false,
                _ => false
            };
        }
        
        public static float ModifierFloatCheck(float inputValue, StringEventModifierType eventModifier)
        {
            return eventModifier switch
            {
                StringEventModifierType.Add => inputValue,
                StringEventModifierType.Remove => -inputValue,
                _ => 0
            };
        }
    }
}