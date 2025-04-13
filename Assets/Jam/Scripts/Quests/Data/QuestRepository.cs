using Jam.Scripts.GameplayData.Repositories;
using UnityEngine;

namespace Jam.Scripts.Quests.Data
{
    [CreateAssetMenu(menuName = "Game Resources/Repositories/Quests")]
    public class QuestRepository : Repository<QuestDefinition>
    {
        public QuestDefinition GetQuest(int questId)
        {
            foreach (QuestDefinition questDefinition in Definitions)
            {
                if (questDefinition.Id == questId)
                {
                    return questDefinition;
                }
            }

            Debug.LogError($"There is no quest in repository with id {questId}!");
            return null;
        }
    }
}
