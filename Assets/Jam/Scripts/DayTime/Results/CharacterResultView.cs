using Jam.Scripts.Npc.Data;
using TMPro;
using UnityEngine;

namespace Jam.Scripts.DayTime.Results
{
    public class CharacterResultView : MonoBehaviour
    {
        [SerializeField] private NpcRepository _npcRepository;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _reputation;
        [SerializeField] private TMP_Text _money;

        public void SetInfo(CharacterResult characterResult)
        {
            _name.text = characterResult.CharacterId.ToString();
            _reputation.text = characterResult.EarnReputation.ToString();
            _money.text = characterResult.EarnMoney.ToString();
        }
    }
}