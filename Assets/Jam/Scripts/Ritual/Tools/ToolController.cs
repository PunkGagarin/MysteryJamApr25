using System.Collections.Generic;
using UnityEngine;

namespace Jam.Scripts.Ritual.Tools
{
    public class ToolController : MonoBehaviour
    {
        [SerializeField] private MirrorTool _mirrorTool;

        public List<ToolDefinition> GetUnlockedTools()
        {
            List<ToolDefinition> unlockedTools = new List<ToolDefinition>();
            
            if (_mirrorTool.IsUnlocked)
                unlockedTools.Add(_mirrorTool.Definition);

            return unlockedTools;
        }

        public void BuyTool(ToolDefinition unlockedTool)
        {
            if (unlockedTool == _mirrorTool.Definition) 
                _mirrorTool.ResetCharges();
        }

        public void UnlockMirror()
        {
            _mirrorTool.gameObject.SetActive(true);
            _mirrorTool.IsUnlocked = true;
        }
    }
}