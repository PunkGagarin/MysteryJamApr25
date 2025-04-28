using System.Collections.Generic;
using Jam.Scripts.Dialogue.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.Manual
{
    public class ReagentUI : MonoBehaviour
    {
        [SerializeField] private ToLocalize _name;
        [SerializeField] private Sprite _undefinedSprite;
        [SerializeField] private List<Image> _conflictImages;

        public void InitData(string reagentLocalizeId, List<Sprite> conflictSprites)
        {
            _name.SetKey(reagentLocalizeId);
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