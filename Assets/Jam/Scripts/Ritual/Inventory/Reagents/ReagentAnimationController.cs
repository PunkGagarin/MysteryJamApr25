using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    public class ReagentAnimationController : MonoBehaviour
    {
        [SerializeField] private ReagentAnimation _reagentAnimationPrefab;
        [Inject] private InventoryConfig _inventoryConfig;
        private List<ReagentAnimation> _animations = new();

        private void Awake()
        {
            for (int i = 0; i < _inventoryConfig.RoomsForRitual; i++)
            {
                var reagentAnimation = Instantiate(_reagentAnimationPrefab, transform);
                reagentAnimation.Initialize(_inventoryConfig);
                _animations.Add(reagentAnimation);
            }
        }
        
        public void PlayAnimationFromInventory(ReagentDefinition reagentDefinition, Vector3 componentPosition, ReagentRoom reagentRoom)
        {
            _animations.FirstOrDefault(anim => !anim.IsPlaying)
                ?.StartAnimation(componentPosition, reagentRoom.Position, reagentDefinition.Visual, reagentRoom.ActivateRoom);
        }
        
        public void PlayAnimationFromPosition(Vector3 startPosition, Vector3 endPosition, Sprite visual, Action action)
        {
            _animations.FirstOrDefault(anim => !anim.IsPlaying)
                ?.StartAnimation(startPosition, endPosition, visual, action);
        }
    }
}
