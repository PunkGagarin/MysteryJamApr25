using System.Collections.Generic;
using Jam.Scripts.GameplayData.Repositories;
using Jam.Scripts.Manual;
using UnityEngine;

namespace Jam.Scripts.Manual
{
    [CreateAssetMenu(menuName = "Game Resources/Repositories/Manual Pages")]
    public class ManualPagesRepository : Repository<ManualPage>
    {
        [field: SerializeField] public List<ManualPage> InitPages { get; private set; }
        [field: SerializeField] public List<ManualPageList> Pages { get; private set; }
    }
}
[System.Serializable]
public class ManualPageList
{
    public List<ManualPage> Pages;
}
