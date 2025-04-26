using Jam.Scripts.Ritual.Inventory.Reagents;
using Jam.Scripts.Ritual.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Shop
{
    public class ShopItem : MonoBehaviour
    {
        [field: SerializeField] public Button BuyButton { get; private set; } 
        [SerializeField] private Image _itemVisual;
        [SerializeField] private TMP_Text _itemPrice;

        public void SetReagent(ReagentDefinition reagentDefinition)
        {
            _itemVisual.sprite = reagentDefinition.Visual;
            _itemPrice.text = reagentDefinition.Cost.ToString();
        }

        public void SetTool(ToolDefinition toolDefinition)
        {
            _itemVisual.sprite = toolDefinition.Visual;
            _itemPrice.text = toolDefinition.Cost.ToString();
        }
    }
}