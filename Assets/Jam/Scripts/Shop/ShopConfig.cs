using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;

namespace Jam.Scripts.Shop
{
    [CreateAssetMenu(fileName = "ShopConfig", menuName = "Game Resources/Configs/Shop")]
    public class ShopConfig : ScriptableObject
    {
        [field: SerializeField] public int RandomItemsInShop { get; private set; }
        [field: SerializeField] public ReagentDefinition ExcludeReagent { get; private set; }
    }
}