using System.Collections.Generic;
using DG.Tweening;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Utils.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Jam.Scripts.Shop
{
    public class ShopView : Popup
    {
        [SerializeField] private ShopItem _shopItemPrefab; 
        [SerializeField] private Transform _reagentsContainer;
        [SerializeField] private Transform _toolsContainer;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_Text _goldAmount;
        
        [Inject] private Localization _localization;
        
        private const string GOLD_AMOUNT_KEY = "GOLD_AMOUNT_KEY";

        public void SetItems(List<ShopItem> reagents, List<ShopItem> tools)
        {
            UpdateReagentsList(reagents);
            UpdateToolsList(tools);
        }
        
        public void UpdateMoney(int amount) => 
            _goldAmount.text = $"{GOLD_AMOUNT_KEY} : {amount.ToString()}";

        public void ShowCantBuyAnimation()
        {
            _goldAmount.transform.DOKill();
            _goldAmount.transform.DOShakePosition(.1f);
        }

        private void UpdateToolsList(List<ShopItem> tools)
        {
            if (tools == null)
                return;
            
            foreach (var tool in tools) 
                tool.transform.SetParent(_toolsContainer, false);
        }

        private void UpdateReagentsList(List<ShopItem> reagents)
        {
            foreach (var reagent in reagents) 
                reagent.transform.SetParent(_reagentsContainer, false);
        }

        private void Awake()
        {
            _exitButton.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            _exitButton.onClick.RemoveListener(Close);
        }
    }
}