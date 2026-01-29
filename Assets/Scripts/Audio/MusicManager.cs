using System;
using DG.Tweening;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Audio {
    public class MusicManager : PersistentSingleton<MusicManager> {
        [Header("Audio Sources")] 
        [SerializeField] private AudioSource sourceA;
        [SerializeField] private AudioSource sourceB;

        [Header("Settings")] 
        [SerializeField] private float crossFadeDuration = 2.0f;
        [SerializeField] private float fadeOutInDuration = 1.5f;
        [SerializeField] private float fadeOutInSilenceGap = 0.5f;
        [SerializeField] private float fadeOutDuration = 1.0f;

        private Sequence _currentTransition;
        private bool _isSourceAPlaying;

        protected override void Awake() {
            base.Awake();

            if (sourceA) sourceA.volume = 0;
            if (sourceB) sourceB.volume = 0;
        }

        public void PlayMusic(AudioClip newClip, AudioTransitionType transitionType) {
            if (_currentTransition != null && _currentTransition.IsActive())
                _currentTransition.Kill();

            var activeSource = _isSourceAPlaying ? sourceA : sourceB;
            var newSource = _isSourceAPlaying ? sourceB : sourceA;

            if (activeSource.clip == null) {
                PlayFirstTrack(newSource, newClip);
                _isSourceAPlaying = !_isSourceAPlaying;
                return;
            }

            _currentTransition = transitionType switch {
                AudioTransitionType.FadeOutIn => CreateFadeOutInSequence(activeSource, newSource, newClip),
                AudioTransitionType.CrossFade => CreateCrossfadeSequence(activeSource, newSource, newClip),
                _ => throw new ArgumentOutOfRangeException(nameof(transitionType))
            };

            _isSourceAPlaying = !_isSourceAPlaying;
        }
        
        public void StopMusic() {
            if (_currentTransition != null && _currentTransition.IsActive())
                _currentTransition.Kill();

            _currentTransition = DOTween.Sequence();

            // Fade out of both active audio sources in case the music gets stopped during a cross-fade transition
            if (sourceA.volume > 0 || sourceA.isPlaying) {
                _currentTransition.Join(sourceA.DOFade(0, fadeOutDuration).SetEase(Ease.InQuad));
            }

            if (sourceB.volume > 0 || sourceB.isPlaying) {
                _currentTransition.Join(sourceB.DOFade(0, fadeOutDuration).SetEase(Ease.InQuad));
            }

            _currentTransition.OnComplete(() => {
                CleanupSource(sourceA);
                CleanupSource(sourceB);
            });
        }

        private void PlayFirstTrack(AudioSource source, AudioClip clip) {
            source.clip = clip;
            source.loop = true;
            source.volume = 0;
            source.Play();
            source.DOFade(1f, crossFadeDuration).SetEase(Ease.OutQuad);
        }

        private Sequence CreateCrossfadeSequence(AudioSource active, AudioSource next, AudioClip clip) {
            var seq = DOTween.Sequence();

            next.clip = clip;
            next.loop = true;
            next.volume = 0;
            next.Play();

            seq.Join(active.DOFade(0, crossFadeDuration).SetEase(Ease.InOutQuad));
            seq.Join(next.DOFade(1, crossFadeDuration).SetEase(Ease.InOutQuad));

            seq.OnComplete(() => CleanupSource(active));

            return seq;
        }

        private Sequence CreateFadeOutInSequence(AudioSource active, AudioSource next, AudioClip clip) {
            var seq = DOTween.Sequence();

            seq.Append(active.DOFade(0, fadeOutInDuration).SetEase(Ease.InQuad));
            
            seq.AppendCallback(() => CleanupSource(active));

            if (fadeOutInSilenceGap > 0) {
                seq.AppendInterval(fadeOutInSilenceGap);
            }

            seq.AppendCallback(() => {
                next.clip = clip;
                next.loop = true;
                next.volume = 0;
                next.Play();
            });

            seq.Append(next.DOFade(1, fadeOutInDuration).SetEase(Ease.OutQuad));

            return seq;
        }

        private void CleanupSource(AudioSource source) {
            source.Stop();
            source.clip = null;
            source.volume = 0;
        }
    }
}