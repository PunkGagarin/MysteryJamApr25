using UnityEngine;

namespace Jam.Scripts.Dialogue.Runtime.Events.SO
{
    public abstract class DialogueEventSO : ScriptableObject
    {
        public virtual void RunEvent()
        {
            Debug.Log("Event fired");
        }
    }
}