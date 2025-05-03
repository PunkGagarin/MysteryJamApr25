using System;
using Jam.Scripts.Audio.Domain;
using Jam.Scripts.Npc.Data;
using Jam.Scripts.Quests;
using Jam.Scripts.Ritual;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Jam.Scripts.Npc
{
    public class Character : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private NpcTalk _talk;
        [SerializeField] private SpriteRenderer _visual;

        [Inject] private QuestPresenter _questPresenter;
        [Inject] private AudioService _audioService;
        [Inject] private RitualController _ritualController;

        private NPCDefinition _definition;
        private bool _canInteract;

        public event Action<int, string> OnCharacterArrived;
        public event Action OnCharacterLeave;

        public void SetCharacter(NPCDefinition definition)
        {
            _definition = definition;
            CharacterArrived();
            OnCharacterArrived?.Invoke(definition.Id, definition.Name);
            _visual.sprite = definition.Visual;
            _questPresenter.SetCharacter(definition.Id);
        }

        public void Talk() => 
            _talk.Talk(_definition.Dialogue, CharacterLeave);

        private void CharacterArrived() => 
            _canInteract = true;

        private void CharacterLeave() => 
            OnCharacterLeave?.Invoke();

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!EventSystem.current.IsPointerOverGameObject() )
                return;
            
            if (_canInteract && !_talk.IsDialogueActive)
            {
                _canInteract = false;
                _audioService.PlaySound(Sounds.buttonClick.ToString());
                Talk();
            }
        }

        private void Awake()
        {
            _ritualController.OnRitual += Talk;
        }

        private void OnDestroy()
        {
            _ritualController.OnRitual -= Talk;
        }
    }
}
