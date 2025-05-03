using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Jam.Scripts.UI
{
    internal class ButtonDeselector : MonoBehaviour
    {
        private Button button;

        void Awake()
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(DeselectAfterClick);
            }
        }

        private void DeselectAfterClick() =>
            EventSystem.current.SetSelectedGameObject(null);
    }
}