using UnityEngine;

namespace Jam.Scripts.Shop
{
    [CreateAssetMenu(fileName = "ShopConfig", menuName = "Game Resources/Configs/Shop")]
    public class ShopConfig : ScriptableObject
    {
        [field: SerializeField] public int RandomItemsInShop { get; private set; }
    }
}