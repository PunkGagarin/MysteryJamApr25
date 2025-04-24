using Jam.Scripts.MainMenuPopups;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayOverlayUI : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [Inject] private PopupManager _popupManager;
    
    private void Awake() => _pauseButton.onClick.AddListener(OnPauseClick);

    private void OnDestroy() => _pauseButton.onClick.RemoveListener(OnPauseClick);

    private void OnPauseClick() => _popupManager.OpenPopup<PausePopup>(withPause: true);
}