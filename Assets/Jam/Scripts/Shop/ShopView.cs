using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Shop
{
    public class ShopView : MonoBehaviour
    {
        [SerializeField] private ShopItem _shopItemPrefab; 
        [SerializeField] private Transform _reagentsContainer;
        [SerializeField] private Transform _toolsContainer;

        public void SetItems(List<ShopItem> reagents, List<ShopItem> tools)
        {
            UpdateReagentsList(reagents);
            UpdateToolsList(tools);
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

    }
}