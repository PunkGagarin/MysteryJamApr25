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
        [SerializeField] private TMP_Text _itemCount;

        public void SetReagent(ReagentDefinition reagentDefinition, int currentAmount, int maxAmount)
        {
            _itemVisual.sprite = reagentDefinition.Visual;
            _itemPrice.text = reagentDefinition.Cost.ToString();
            _itemCount.text = $"{currentAmount}/{maxAmount}";
        }

        public void SetTool(ToolDefinition toolDefinition)
        {
            _itemVisual.sprite = toolDefinition.Visual;
            _itemPrice.text = toolDefinition.Cost.ToString();
            _itemCount.transform.parent.gameObject.SetActive(false);
        }
    }
}