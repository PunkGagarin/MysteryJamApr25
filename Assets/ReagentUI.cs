using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReagentUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Image _conflictImage1;
    [SerializeField] private Image _conflictImage2;
    [SerializeField] private Sprite _undefinedSprite;

    public void InitData(string reagentName, Sprite conflictImage1, Sprite conflictImage2)
    {
        _name.text = reagentName;
        _conflictImage1.sprite = conflictImage1 == null ? _undefinedSprite : conflictImage1;
        _conflictImage2.sprite = conflictImage2 == null ? _undefinedSprite : conflictImage2;
    }
}