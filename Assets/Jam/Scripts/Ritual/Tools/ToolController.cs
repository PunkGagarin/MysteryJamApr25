using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Jam.Scripts.Ritual.Tools
{
    public class ToolController : MonoBehaviour
    {
        [SerializeField] private List<RitualTool> _tools;

        public List<ToolDefinition> GetUnlockedTools() => 
            (from tool in _tools where tool.IsUnlocked select tool.Definition).ToList();

        public void BuyTool(ToolDefinition unlockedTool)
        {
            foreach (var tool in _tools)
                if (tool.Definition == unlockedTool)
                    tool.ResetCharges();
        }

        public void UnlockTool(int toolId)
        {
            var toolToUnlock = _tools.FirstOrDefault(tool => tool.Definition.Id == toolId);

            if (toolToUnlock == null)
            {
                Debug.LogError($"trying to unlock tool with id {toolId}, but it not in the ToolControllerPool");
                return;
            }
            
            toolToUnlock.gameObject.SetActive(true);
            toolToUnlock.IsUnlocked = true;
            toolToUnlock.ResetCharges();
        }
    }
}