using System.Collections;
using Jam.Scripts.Utils.Timer;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Jam.Scripts.PostProcessing
{
    public class GhostResponseEffect : MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        [SerializeField] private float _effectDuration = 1f;
        [Header("Vignette Settings")] 
        [SerializeField] private string hexColor = "#051022";
        [SerializeField] private Vector2 _vignetteCenter = new(0.5f, 0.5f);
        [SerializeField] private float _vignetteIntensity = .3f;
        [SerializeField] private float _vignetteSmoothness = 0.5f;

        [Header("ChromaticAberration Settings")] 
        [SerializeField] private float _chromaticAberrationIntensity = 1f;

        [Header("LensDistortion Settings")] 
        [SerializeField] public float _lensDistortionIntensity = -.3f;

        [Inject] private TimerService _timerService;
        
        private Vignette _vignette;
        private LensDistortion _lensDistortion;
        private ChromaticAberration _chromaticAberration;
        
        private void Start()
        {
            SetUpVignette();
            SetUpChromaticAberration();
            SetUpLensDistortion();
        }

        private void SetUpLensDistortion()
        {
            if (_volume != null && _volume.profile.TryGet(out _lensDistortion))
            {
                _lensDistortion.active = true;
            }
            else
            {
                Debug.LogWarning("Add LensDistortion to Volume");
            }
        }

        private void SetUpChromaticAberration()
        {
            if (_volume != null && _volume.profile.TryGet(out _chromaticAberration))
            {
                _chromaticAberration.active = true;
            }
            else
            {
                Debug.LogWarning("Add ChromaticAberration to Volume");
            }
        }

        private void SetUpVignette()
        {
            if (_volume != null && _volume.profile.TryGet(out _vignette))
            {
                _vignette.active = true;
                if (ColorUtility.TryParseHtmlString(hexColor, out Color color))
                {
                    _vignette.color.value = color;
                }
                else
                {
                    Debug.LogWarning("Incorrect HEX color");
                }
                _vignette.center.value = _vignetteCenter;
                _vignette.smoothness.value = _vignetteSmoothness;
            }
            else
            {
                Debug.LogWarning("Add Vignette to Volume");
            }
        }

        public void ToggleEffect()
        {
            TriggerVignetteFadeInOut(_vignetteIntensity, _effectDuration);
            TriggerLensDistortionFadeInOut( _lensDistortionIntensity, _effectDuration);
            TriggerChromaticAberrationFadeInOut(_chromaticAberrationIntensity, _effectDuration);
        }

        private void TriggerVignetteFadeInOut(float to, float duration)
        {
            if (_vignette != null) 
                StartCoroutine(FadeInOutEffect(_vignette.intensity, _vignette.intensity.value, to, duration));
        }

        private void TriggerLensDistortionFadeInOut(float to, float duration)
        {
            if (_lensDistortion != null) 
                StartCoroutine(FadeInOutEffect(_lensDistortion.intensity, _lensDistortion.intensity.value, to, duration));
        }

        private void TriggerChromaticAberrationFadeInOut(float to, float duration)
        {
            if (_chromaticAberration != null) 
                StartCoroutine(FadeInOutEffect(_chromaticAberration.intensity, _chromaticAberration.intensity.value, to, duration));
        }

        private IEnumerator FadeEffect(FloatParameter parameter, float from, float to, float duration)
        {
            
            parameter.overrideState = true;

            float t = 0f;
            while (t < duration)
            {
                parameter.value = Mathf.Lerp(from, to, t / duration);
                t += Time.deltaTime;
                yield return null;
            }
            parameter.value = to;
        }


        private IEnumerator FadeInOutEffect(FloatParameter parameter, float from, float to, float duration)
        {
            parameter.overrideState = true;

            float t = 0f;
            while (t < duration)
            {
                parameter.value = Mathf.Lerp(from, to, t / duration);
                t += Time.deltaTime;
                yield return null;
            }
            parameter.value = to;

            t = 0f;
            while (t < duration)
            {
                parameter.value = Mathf.Lerp(to, from, t / duration);
                t += Time.deltaTime;
                yield return null;
            }
            parameter.value = from;
        }
    }
}