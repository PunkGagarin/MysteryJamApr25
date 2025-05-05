using UnityEngine;

namespace Jam.Scripts.Manual.Pages
{
    public class ToolsPage : MonoBehaviour
    {
        [SerializeField] private GameObject _mirrorContainer;
        [SerializeField] private GameObject _magnifierContainer;

        public void InitPage(bool isMirrorUnlocked, bool isMagnifierUnlocked)
        {
            _mirrorContainer.gameObject.SetActive(isMirrorUnlocked);
            _magnifierContainer.gameObject.SetActive(isMagnifierUnlocked);
        }
    }
}