using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Manual.Pages
{
    public class ArtPage : MonoBehaviour
    {
        [SerializeField] private Image _art;

        public void InitPage(bool artUnlocked) => 
            _art.enabled = artUnlocked;
    }
}