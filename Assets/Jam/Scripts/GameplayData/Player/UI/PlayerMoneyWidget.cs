using TMPro;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.GameplayData.Player.UI
{
    public class PlayerMoneyWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amount;

        [Inject] private PlayerStatsPresenter _playerStatsPresenter;

        private void Awake()
        {
            _playerStatsPresenter.OnMoneyChanged += UpdateWidget;
            UpdateWidget(_playerStatsPresenter.Money);
        }

        private void UpdateWidget(int newValue)
        {
            _amount.text = newValue.ToString();
        }

        private void OnDestroy()
        {
            _playerStatsPresenter.OnMoneyChanged -= UpdateWidget;
        }
    }
}