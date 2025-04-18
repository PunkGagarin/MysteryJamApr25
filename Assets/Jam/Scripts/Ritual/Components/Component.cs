using TMPro;
using Jam.Scripts.Audio.Domain;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Ritual.Components
{
    public class Component : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ComponentDefinition _componentDefinition;
        [SerializeField] private TMP_Text _name;
        [Inject] private RitualController _ritualController;
        [Inject] private ComponentsAnimationController _componentsAnimationController;
        [Inject] private AudioService _audioService;

        private void Awake()
        {
            _spriteRenderer.sprite = _componentDefinition.Visual;
            _name.text = _componentDefinition.Name;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;

            _audioService.PlaySound(Sounds.buttonClick.ToString());
            if (_ritualController.TryAddComponent(_componentDefinition, out ComponentRoom room))
                _componentsAnimationController.PlayAnimation(_componentDefinition, transform.position, room);
        }
    }
}
