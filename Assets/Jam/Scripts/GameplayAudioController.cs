using Jam.Scripts.Audio.Domain;
using UnityEngine;
using Zenject;

namespace Jam.Scripts
{
    public class GameplayAudioController : MonoBehaviour
    {
        [Inject] private AudioService _audioService;
        
        private void Awake() =>
            _audioService.PlayMusic(Sounds.gameplayBgm.ToString(), true);
    }
}
