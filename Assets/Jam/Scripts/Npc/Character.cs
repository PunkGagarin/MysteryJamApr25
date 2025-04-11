using System;
using Jam.Scripts.Npc.Data;
using Jam.Scripts.Quests;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Npc
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private PointerShower _pointerShower;
        [SerializeField] private NpcMovement _movement;
        [SerializeField] private NpcTalk _talk;
        
        [Inject] private QuestPresenter _questPresenter;
        
        private NPCDefinition _definition;
        private bool _arrived;
        private bool _readyToLeave;

        public event Action OnCharacterLeave;

        public void SetCharacter(NPCDefinition definition)
        {
            _definition = definition;
            _movement.ShowNpc(CharacterArrived);
        }

        private void CharacterArrived()
        {
            _arrived = true;
            _pointerShower.Show();
        }

        private void Update()
        {
            if (_arrived && !_talk.DialogueInProcess && Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    _pointerShower.Hide();
                    _talk.Talk(_definition.Dialogue);
                }
            }
        }
        
        private void CharacterLeave()
        {
            _arrived = false;
            _movement.HideNpc(OnCharacterLeave);
        }
        
        private void QuestRemoved(int questId)
        {
            if (questId == _definition.BelongQuestId)
                _readyToLeave = true;
        }

        private void DialogueComplete()
        {
            if (_readyToLeave)
                CharacterLeave();
        }

        private void Awake()
        {
            _talk.OnDialogueComplete += DialogueComplete;
            _questPresenter.OnQuestRemoved += QuestRemoved;
        }
        
        private void OnDestroy()
        {
            _talk.OnDialogueComplete -= DialogueComplete;
            _questPresenter.OnQuestRemoved -= QuestRemoved;
        }
    }
}
