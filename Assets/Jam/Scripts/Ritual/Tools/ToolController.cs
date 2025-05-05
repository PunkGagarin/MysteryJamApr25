using System.Collections.Generic;
using System.Linq;
using Jam.Scripts.Audio.Domain;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.Ritual.Tools
{
    public class ToolController : MonoBehaviour
    {
        [SerializeField] private List<RitualTool> _tools;
        [Inject] private AudioService _audioService;

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
            
            _audioService.PlaySound(Sounds.foundConflict);
            toolToUnlock.gameObject.SetActive(true);
            toolToUnlock.IsUnlocked = true;
            toolToUnlock.ResetCharges();
        }
    }
}