using System.Collections.Generic;
using Jam.Scripts.Dialogue.Gameplay;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Manual
{
    public class ReagentUI : MonoBehaviour
    {
        [SerializeField] private ReagentDefinition _reagent;
        [SerializeField] private ToLocalize _name;
        [SerializeField] private Sprite _undefinedSprite;
        [SerializeField] private List<Image> _conflictImages;
        [SerializeField] private Image _reagentImage;

        public ReagentDefinition Definition => _reagent;

        public void InitData(List<Sprite> conflictSprites)
        {
            _name.SetKey(_reagent.LocalizeId);
            _reagentImage.sprite = _reagent.ManualIcon;
            for (int i = 0; i < _conflictImages.Count; i++)
            {
                if (conflictSprites.Count <= i)
                    _conflictImages[i].sprite = _undefinedSprite;
                else
                    _conflictImages[i].sprite = conflictSprites[i] == null ? _undefinedSprite : conflictSprites[i];
            }
        }
    }
}