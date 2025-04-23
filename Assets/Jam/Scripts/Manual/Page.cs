using System.Collections.Generic;
using Jam.Scripts.Ritual;
using Jam.Scripts.Ritual.Inventory.Reagents;
using UnityEngine;

namespace Jam.Scripts.Manual
{
    public class Page : MonoBehaviour
    {
        [SerializeField] private List<ReagentUI> _reagentsViews;
        [SerializeField] private List<ReagentDefinition> _reagentsDefinitions;

        public void Initialize(List<ReagentExclusion> reagentExclusions)
        {
            if (_reagentsDefinitions == null || _reagentsDefinitions.Count == 0) 
                return;
            
            for (int i = 0; i < _reagentsDefinitions.Count; i++)
            {
                Sprite excludedReagentSprite1 = null;
                Sprite excludedReagentSprite2 = null;

                foreach (var reagentExclusion in reagentExclusions)
                {
                    if (reagentExclusion.ReagentId == _reagentsDefinitions[i].Id)
                    {
                        if (_reagentsDefinitions[i].ExcludedReagents[0].Id == reagentExclusion.ExcludedReagentId)
                            excludedReagentSprite1 = _reagentsDefinitions[i].ExcludedReagents[0].Visual;
                        
                        if (_reagentsDefinitions[i].ExcludedReagents[1].Id == reagentExclusion.ExcludedReagentId)
                            excludedReagentSprite2 = _reagentsDefinitions[i].ExcludedReagents[1].Visual;
                    }
                }
                
                _reagentsViews[i].InitData(_reagentsDefinitions[i].Name, excludedReagentSprite1, excludedReagentSprite2);
            }
        }
    }
}