using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Dialogue.UI
{
    public class ButtonController : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text ButtonText { get; private set; }

        public void SetText(string text) => 
            ButtonText.text = text;
    }
}