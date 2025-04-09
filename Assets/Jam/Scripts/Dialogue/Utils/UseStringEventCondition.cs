using Jam.Scripts.Dialogue.Runtime.Enums;
using UnityEngine;

namespace Jam.Scripts.Dialogue.Utils
{
    public static class UseStringEventCondition
    {
        public static bool ConditionFloatCheck(float currentValue, float checkValue, StringEventConditionType conditionType)
        {
            return conditionType switch
            {
                StringEventConditionType.Equals => ValueEquals(currentValue, checkValue),
                StringEventConditionType.EqualsOrBigger => ValueEqualsOrBigger(currentValue, checkValue),
                StringEventConditionType.EqualsOrSmaller => ValueEqualsOrSmaller(currentValue, checkValue),
                StringEventConditionType.Bigger => ValueBigger(currentValue, checkValue),
                StringEventConditionType.Smaller => ValueSmaller(currentValue, checkValue),
                _ => false
            };
        }
        
        public static bool ConditionBoolCheck(bool currentValue, StringEventConditionType conditionType)
        {
            return conditionType switch
            {
                StringEventConditionType.True => ValueBool(currentValue, true),
                StringEventConditionType.False => ValueBool(currentValue, false),
                _ => false
            };
        }

        private static bool ValueBool(bool currentValue, bool checkValue) => currentValue == checkValue;
        private static bool ValueEquals(float currentValue, float checkValue) => Mathf.Approximately(currentValue, checkValue);
        private static bool ValueEqualsOrBigger(float currentValue, float checkValue) => currentValue >= checkValue;
        private static bool ValueEqualsOrSmaller(float currentValue, float checkValue) => currentValue <= checkValue;
        private static bool ValueBigger(float currentValue, float checkValue) => currentValue > checkValue;
        private static bool ValueSmaller(float currentValue, float checkValue) => currentValue < checkValue;
    }
}