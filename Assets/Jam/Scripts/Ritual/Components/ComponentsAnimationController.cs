using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jam.Scripts.Ritual.Components
{
    public class ComponentsAnimationController : MonoBehaviour
    {
        [SerializeField] private ComponentAnimation _componentAnimationPrefab;

        private List<ComponentAnimation> _animations = new();

        private void Awake()
        {
            for (int i = 0; i < 4; i++)
                _animations.Add(Instantiate(_componentAnimationPrefab, transform));
        }
        
        public void PlayAnimation(ComponentDefinition componentDefinition, Vector3 componentPosition, ComponentRoom componentRoom)
        {
            _animations.FirstOrDefault(anim => !anim.IsPlaying)
                ?.StartAnimation(componentPosition, componentRoom.Position, componentDefinition.Visual, componentRoom.ActivateComponent);
        }
    }
}
