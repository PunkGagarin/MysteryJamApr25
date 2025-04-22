using System;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Npc.Data;
using Jam.Scripts.Quests;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Npc
{
    public class Character : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private PointerShower _pointerShower;
        [SerializeField] private NpcTalk _talk;
        [SerializeField] private SpriteRenderer _visual;

        [Inject] private QuestPresenter _questPresenter;
        [Inject] private AudioService _audioService;

        private UnityEngine.Camera _camera;
        private NPCDefinition _definition;
        private bool _arrived;

        public event Action<int> OnCharacterArrived;
        public event Action OnCharacterLeave;

        public void SetCharacter(NPCDefinition definition)
        {
            _definition = definition;
            CharacterArrived();
            OnCharacterArrived?.Invoke(definition.Id);
            _visual.sprite = definition.Visual;
        }

        private void CharacterArrived()
        {
            _arrived = true;
            _pointerShower.Show();
        }
        
        private void CharacterLeave()
        {
            _arrived = false;
            OnCharacterLeave?.Invoke();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                return;
            
            if (_arrived)
            {
                _audioService.PlaySound(Sounds.buttonClick.ToString());
                _pointerShower.Hide();
                _talk.Talk(_definition.Dialogue, CharacterLeave);
            }
        }

        private void Awake()
        {
            _camera = UnityEngine.Camera.main;
        }
    }
}
