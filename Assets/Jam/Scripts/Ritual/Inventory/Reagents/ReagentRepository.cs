using System.Linq;
using Jam.Scripts.GameplayData.Repositories;
using UnityEngine;

namespace Jam.Scripts.Ritual.Inventory.Reagents
{
    [CreateAssetMenu(menuName = "Game Resources/Repositories/Reagents")]
    public class ReagentRepository : Repository<ReagentDefinition>
    {
        public ReagentDefinition GetDefinition(int id)
        {
            if (Definitions.All(def => def.Id != id))
            {
                Debug.LogError($"Not found component with id {id}");
                return null;
            }
            
            return Definitions.First(def => def.Id == id);
        }
    }
}