using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jam.Scripts.Dialogue.UI
{
    public class AnswerButtonHistory : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        public void SetText(string text) => _text.text = text;
    }
}