using System.Collections;
using Jam.Scripts.Npc;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Jam.Scripts.VFX
{
    public class LightController : MonoBehaviour
    {
        [Inject] private Character _characterController;
        [SerializeField] private Light2D _curtainsLight;
        [SerializeField] private float _maxCurtainsIntensity;

        private void Awake()
        {
            _characterController.OnCharacterArrived += ShowLight;
            _characterController.OnCharacterLeave += HideLight;
        }

        private void HideLight() => ChangeIntensity(0, 0.5f);

        private void ShowLight(int obj) => ChangeIntensity(_maxCurtainsIntensity, 0.5f);

        private void ChangeIntensity(float targetIntensity, float duration)
        {
            StopAllCoroutines();
            StartCoroutine(ChangeLightIntensityCoroutine(targetIntensity, duration));
        }

        private IEnumerator ChangeLightIntensityCoroutine(float target, float duration)
        {
            float start = _curtainsLight.intensity;
            float time = 0f;

            while (time < duration)
            {
                time += Time.deltaTime;
                _curtainsLight.intensity = Mathf.Lerp(start, target, time / duration);
                yield return null;
            }

            _curtainsLight.intensity = target;
        }

        private void OnDestroy()
        {
            _characterController.OnCharacterArrived -= ShowLight;
            _characterController.OnCharacterLeave -= HideLight;
        }
    }
}