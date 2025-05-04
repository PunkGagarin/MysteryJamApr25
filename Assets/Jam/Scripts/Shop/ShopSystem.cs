using System;
using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Audio.Domain;
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

        [SerializeField]
        private ShopItem _shopItemPrefab;

        [Inject] private PopupManager _popupManager;
        [Inject] private ReagentRepository _reagentRepository;
        [Inject] private ShopConfig _shopConfig;
        [Inject] private DayController _dayController;
        [Inject] private PlayerStatsPresenter _playerStats;
        [Inject] private InventorySystem _inventorySystem;
        [Inject] private AudioService _audioService;

        private ShopView _shopView;
        private List<ShopItem> _reagentsShopItems = new();
        private List<ShopItem> _toolsShopItems = new();

        public event Action<int> ItemAppear;
        public event Action CantBuy;

        public void SetShopView(ShopView shopView) =>
            _shopView = shopView;

        public void ShowShop()
        {
            _shopView.gameObject.SetActive(!_dayController.IsFirstDay && !_dayController.IsLastDay);
            PopulateShop();
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
                        _playerStats.RemoveMoney(toolInSlot.Cost);
                        shopItem.gameObject.SetActive(false);
                        _audioService.PlaySound(Sounds.buttonClick);
                    }
                    else
                    {
                        CantBuy?.Invoke();
                    }
                }
            }
        }

        private void SetupReagentsShopItems()
        {
            var reagentsInShopPool = new List<ReagentDefinition>();
            AddAllReagentsToPool(reagentsInShopPool);
            // PopulatePoolWithSexReagents(reagentsInShopPool);
            // PopulatePoolWithNonSexReagents(reagentsInShopPool);
            UpdateContainerRooms(reagentsInShopPool.Count, _reagentsShopItems);
            SetUpReagentsShopItem(reagentsInShopPool);
        }

        private void AddAllReagentsToPool(List<ReagentDefinition> reagentsInShopPool)
        {
            var allReags = _reagentRepository.Definitions
                .Where(def => def != _shopConfig.ExcludeReagent).ToList();
            reagentsInShopPool.AddRange(allReags);
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
                        _audioService.PlaySound(Sounds.buttonClick);
                    }
                    else
                    {
                        CantBuy?.Invoke();
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
            var notSexReagents = _reagentRepository.Definitions
                .Where(def => def.ReagentType != ReagentType.Sex && def != _shopConfig.ExcludeReagent).ToList();
            for (int i = 0; i < _shopConfig.RandomItemsInShop; i++)
            {
                var randomReagent = notSexReagents[Random.Range(0, notSexReagents.Count)];
                reagentsInShopPool.Add(randomReagent);
                notSexReagents.Remove(randomReagent);
            }
        }

    }
}