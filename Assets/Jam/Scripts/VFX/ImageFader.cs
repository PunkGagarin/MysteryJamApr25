using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Jam.Scripts.VFX
{
    public class ImageFader : MonoBehaviour
    {
        [SerializeField] private Image targetImage;
        [SerializeField] private float fadeDuration = 0.5f;
        [SerializeField] private float delayBeforeFadeOut = 0.5f;

        private void Awake()
        {
            if (targetImage == null)
                targetImage = GetComponent<Image>();
        }

        public void FadeInThenOut(Action onComplete)
        {
            gameObject.SetActive(true);
            targetImage.DOFade(1f, fadeDuration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    DOVirtual.DelayedCall(delayBeforeFadeOut, () =>
                    {
                        targetImage.DOFade(0f, fadeDuration)
                            .SetEase(Ease.InOutSine)
                            .OnComplete(() =>
                            {
                                gameObject.SetActive(false);
                                onComplete?.Invoke();
                            });
                    });
                });
        }
    }
}