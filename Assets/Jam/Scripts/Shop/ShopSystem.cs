using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.DayTime;
using Jam.Scripts.GameplayData.Player;
using Jam.Scripts.Ritual.Inventory;
using Jam.Scripts.Ritual.Inventory.Reagents;
using Jam.Scripts.Ritual.Tools;
using Jam.Scripts.Utils.UI;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Jam.Scripts.Shop
{
    public class ShopSystem : MonoBehaviour
    {
        [SerializeField] private ShopItem _shopItemPrefab;
        [Inject] private PopupManager _popupManager;
        [Inject] private ReagentRepository _reagentRepository;
        [Inject] private ShopConfig _shopConfig;
        [Inject] private DayController _dayController;
        [Inject] private PlayerStatsPresenter _playerStats;
        [Inject] private InventorySystem _inventorySystem;

        private ShopView _shopView;
        private List<ShopItem> _reagentsShopItems = new();
        private List<ShopItem> _toolsShopItems = new();

        public event Action<int> ItemAppear;

        public void OpenShop(Action closeEvent = null)
        {
            _shopView = _popupManager.OpenPopup<ShopView>(closeEvent);
            PopulateShop();
            UpdateMoney(_playerStats.Money, 0);
        }

        private void PopulateShop()
        {
            SetupReagentsShopItems();
            SetupToolsShopItems();

            _shopView.SetItems(_reagentsShopItems, _toolsShopItems);
        }

        private void SetupToolsShopItems()
        {
            var toolsInShopPool = new List<ToolDefinition>();
            PopulatePoolWithTools(toolsInShopPool);
            SetUpToolsShopItem(toolsInShopPool);
        }

        private void PopulatePoolWithTools(List<ToolDefinition> toolsInShopPool)
        {
            toolsInShopPool.Clear();
            toolsInShopPool.AddRange(_inventorySystem.GetUnlockedTools());
            UpdateContainerRooms(toolsInShopPool.Count, _toolsShopItems);
        }

        private void SetUpToolsShopItem(List<ToolDefinition> toolsInShopPool)
        {
            for (int i = 0; i < toolsInShopPool.Count; i++)
            {
                ToolDefinition toolInSlot = toolsInShopPool[i];
                ShopItem shopItem = _toolsShopItems[i];
                shopItem.SetTool(toolInSlot);
                shopItem.gameObject.SetActive(true);
                shopItem.BuyButton.onClick.RemoveAllListeners();

                shopItem.BuyButton.onClick.AddListener(BuyTool);
                continue;

                void BuyTool()
                {
                    if (_playerStats.CanSpend(toolInSlot.Cost))
                    {
                        _inventorySystem.BuyTool(toolInSlot);
                        shopItem.gameObject.SetActive(false);
                    }
                    else
                    {
                        _shopView.ShowCantBuyAnimation();
                    }
                }
            }
        }

        private void SetupReagentsShopItems()
        {
            var reagentsInShopPool = new List<ReagentDefinition>();
            PopulatePoolWithSexReagents(reagentsInShopPool);
            PopulatePoolWithNonSexReagents(reagentsInShopPool);
            UpdateContainerRooms(reagentsInShopPool.Count, _reagentsShopItems);
            SetUpReagentsShopItem(reagentsInShopPool);
        }

        private void SetUpReagentsShopItem(List<ReagentDefinition> reagentsInShopPool)
        {
            for (int i = 0; i < reagentsInShopPool.Count; i++)
            {
                ReagentDefinition reagentInSlot = reagentsInShopPool[i];
                ItemAppear?.Invoke(reagentInSlot.Id);
                ShopItem shopItem = _reagentsShopItems[i];
                shopItem.SetReagent(reagentInSlot);
                shopItem.gameObject.SetActive(true);
                shopItem.BuyButton.onClick.RemoveAllListeners();

                shopItem.BuyButton.onClick.AddListener(BuyReagent);
                continue;

                void BuyReagent()
                {
                    if (_playerStats.CanSpend(reagentInSlot.Cost))
                    {
                        _inventorySystem.BuyReagent(reagentInSlot.Id);
                        _playerStats.RemoveMoney(reagentInSlot.Cost);
                        shopItem.gameObject.SetActive(false);
                    }
                    else
                    {
                        _shopView.ShowCantBuyAnimation();
                    }
                }
            }
        }

        private void PopulatePoolWithSexReagents(List<ReagentDefinition> reagentsInShopPool)
        {
            var sexReagents = _reagentRepository.Definitions.Where(def => def.ReagentType == ReagentType.Sex).ToList();
            var randomSexReagent = sexReagents[Random.Range(0, sexReagents.Count)];
            reagentsInShopPool.Add(randomSexReagent);
            SexType reagentTypeInPool = reagentsInShopPool[0].SexType;
            sexReagents.RemoveAll(reagent => reagent.SexType == reagentTypeInPool);
            randomSexReagent = sexReagents[Random.Range(0, sexReagents.Count)];
            reagentsInShopPool.Add(randomSexReagent);
        }

        private void UpdateContainerRooms(int requiredRooms, List<ShopItem> list)
        {
            if (requiredRooms > list.Count)
            {
                int reagentsToCreate = requiredRooms - list.Count;
                for (int i = 0; i < reagentsToCreate; i++)
                {
                    var item = Instantiate(_shopItemPrefab);
                    list.Add(item);
                }
            }
        }

        private void PopulatePoolWithNonSexReagents(List<ReagentDefinition> reagentsInShopPool)
        {
            var notSexReagents = _reagentRepository.Definitions.Where(def => def.ReagentType != ReagentType.Sex).ToList();
            for (int i = 0; i < _shopConfig.RandomItemsInShop; i++)
            {
                var randomReagent = notSexReagents[Random.Range(0, notSexReagents.Count)];
                reagentsInShopPool.Add(randomReagent);
                notSexReagents.Remove(randomReagent);
            }
        }

        private void UpdateMoney(int value, int oldValue)
        {
            if (_shopView != null) 
                _shopView.UpdateMoney(value);
        }

        private void Awake()
        {
            _playerStats.OnMoneyChanged += UpdateMoney;
        }

        private void OnDestroy()
        {
            _playerStats.OnMoneyChanged -= UpdateMoney;
            _popupManager.ResetPopup<ShopView>();
        }
    }
}