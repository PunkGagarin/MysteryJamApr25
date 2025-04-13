using Jam.Scripts.Quests.Data;
using UnityEngine;
using Zenject;

namespace Jam.Scripts.GameplayData.Repositories
{
    [CreateAssetMenu(fileName = "Repository Installer", menuName = "Game Resources/Repository Installer")]
    public class RepositoryInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private QuestRepository _questRepository;
        
        public override void InstallBindings()
        {
            BindQuests();
        }
        
        private void BindQuests()
        {
            Container.Bind<QuestRepository>()
                .FromInstance(_questRepository)
                .AsSingle();
        }
    }
}