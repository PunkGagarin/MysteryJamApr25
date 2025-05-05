using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.SceneManagement;
using Jam.Scripts.Utils.Coroutine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.End
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private GameObject _creditsContainer;
        [SerializeField] private GameObject _resultsContainer;
        [SerializeField] private TMP_Text _totalMoneyEarned;
        [SerializeField] private ToLocalize _result;

        [Inject] private SceneLoader _sceneLoader;
        [Inject] private CoroutineHelper _coroutineHelper;
        [Inject] private PlayerStatsPresenter _playerStatsPresenter;

        private const string LAST_QUEST_SUCCESS_KEY = "LAST_QUEST_SUCCESS", LAST_QUEST_FAILED = "LAST_QUEST_FAILED";

        public void Show()
        {
            _totalMoneyEarned.text = _playerStatsPresenter.TotalMoneyEarned.ToString();
            _result.SetKey(_playerStatsPresenter.IsLastQuestComplete ? LAST_QUEST_SUCCESS_KEY : LAST_QUEST_FAILED);
            gameObject.SetActive(true);
        }
        
        private void Awake()
        {
            _exitButton.onClick.AddListener(ToMainMenu);
            _creditsButton.onClick.AddListener(ShowCredits);
        }
        
        private void OnDestroy()
        {
            _exitButton.onClick.RemoveListener(ToMainMenu);
            _creditsButton.onClick.RemoveListener(ShowCredits);
        }

        private void ShowCredits()
        {
            _resultsContainer.SetActive(false);
            _creditsContainer.SetActive(true);
        }

        private void ToMainMenu() => 
            _coroutineHelper.StartCoroutine(_sceneLoader.LoadScene(SceneEnum.MainMenu));
    }
}