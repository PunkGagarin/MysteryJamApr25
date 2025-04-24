using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReagentUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Sprite _undefinedSprite;
    [SerializeField] private List<Image> _conflictImages;

    public void InitData(string reagentName, List<Sprite> conflictSprites)
    {
        _name.text = reagentName;
        for (int i = 0; i < _conflictImages.Count; i++)
        {
            if (conflictSprites.Count <= i)
                _conflictImages[i].sprite = _undefinedSprite;
            else
                _conflictImages[i].sprite = conflictSprites[i] == null ? _undefinedSprite : conflictSprites[i];
        }
    }
}