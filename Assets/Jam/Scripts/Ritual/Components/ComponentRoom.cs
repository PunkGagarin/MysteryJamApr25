using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Ritual.Components
{
    public class ComponentRoom : MonoBehaviour
    {
        [SerializeField] private Image _componentSprite;
        [SerializeField] private Color _freeColor;
        [SerializeField] private Color _fillColor;

        public ComponentDefinition ComponentInside { get; private set; }
        public bool IsFree => ComponentInside == null;
        public Vector3 Position => transform.position;

        public void SetComponent(ComponentDefinition component)
        {
            ComponentInside = component;
            _componentSprite.sprite = component.Visual;
        }

        public void ActivateComponent()
        {
            _componentSprite.color = _fillColor;
            _componentSprite.gameObject.SetActive(true);
        }

        public void ReleaseComponent()
        {
            _componentSprite.gameObject.SetActive(false);
            _componentSprite.color = _freeColor;
            ComponentInside = null;
        }
    }
}