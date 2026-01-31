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
        
        [Header("Default Settings")] 
        [SerializeField] private float defaultVolume = 1.0f;
        [SerializeField] private float defaultCrossFadeDuration = 2.0f;
        [SerializeField] private float defaultFadeOutInDuration = 1.5f;
        [SerializeField] private float defaultFadeOutInSilenceGap = 0.5f;
        [SerializeField] private float defaultFadeInDuration = 1.0f;
        [SerializeField] private float defaultFadeOutDuration = 1.0f;

        private Sequence _currentTransition;
        private bool _isSourceAPlaying;

        private void Start() {
            if (sourceA.isPlaying) _isSourceAPlaying = true;
            else sourceA.volume = 0;
            
            sourceB.volume = 0;
        }

        public void PlayMusic(AudioClip newClip, AudioTransitionType transitionType, float customDuration = -1f, float customVolume = -1f) {
            var activeSource = _isSourceAPlaying ? sourceA : sourceB;
            var newSource = _isSourceAPlaying ? sourceB : sourceA;

            if (activeSource.clip == newClip && activeSource.isPlaying) return;

            if (_currentTransition != null && _currentTransition.IsActive())
                _currentTransition.Kill();

            var targetVolume = customVolume < 0 ? defaultVolume : Mathf.Clamp(customVolume, 0f, 1.0f);
            float duration;

            switch (transitionType) {
                case AudioTransitionType.None:
                    ImmediatelyChangeTrack(activeSource, newSource, newClip, targetVolume);
                    _currentTransition = null; 
                    break;
                case AudioTransitionType.FadeIn:
                    duration = customDuration < 0 ? defaultFadeInDuration : customDuration;
                    _currentTransition = CreateFadeInSequence(activeSource, newSource, newClip, duration, targetVolume);
                    break;
                    
                case AudioTransitionType.CrossFade:
                    duration = customDuration < 0 ? defaultCrossFadeDuration : customDuration;
                    _currentTransition = CreateCrossFadeSequence(activeSource, newSource, newClip, duration, targetVolume);
                    break;
                    
                case AudioTransitionType.FadeOutIn:
                    duration = customDuration < 0 ? defaultFadeOutInDuration : customDuration;
                    _currentTransition = CreateFadeOutInSequence(activeSource, newSource, newClip, duration, targetVolume);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(transitionType), transitionType, null);
            }

            _currentTransition?.Play();
            _isSourceAPlaying = !_isSourceAPlaying;
        }
        
        public void StopMusic(float customDuration = -1f) {
            if (_currentTransition != null && _currentTransition.IsActive())
                _currentTransition.Kill();
            
            var fadeDuration = customDuration < 0 ? defaultFadeOutDuration : customDuration;
            
            _currentTransition = DOTween.Sequence();

            // Fade out of both active audio sources in case the music gets stopped during a cross-fade transition
            if (sourceA.volume > 0 || sourceA.isPlaying) {
                _currentTransition.Join(sourceA.DOFade(0, fadeDuration).SetEase(Ease.InQuad));
            }

            if (sourceB.volume > 0 || sourceB.isPlaying) {
                _currentTransition.Join(sourceB.DOFade(0, fadeDuration).SetEase(Ease.InQuad));
            }

            _currentTransition.OnComplete(() => {
                CleanupSource(sourceA);
                CleanupSource(sourceB);
            });
            
            _currentTransition.Play();
        }

        private Sequence CreateFadeInSequence(AudioSource activeSource, AudioSource newSource, AudioClip clip, float duration, float endVolume) {
            CleanupSource(activeSource);
            CleanupSource(newSource);
            
            newSource.clip = clip;
            newSource.loop = true;
            newSource.volume = 0;
            newSource.Play();
            
            var seq = DOTween.Sequence();
            seq.Append(newSource.DOFade(endVolume, duration).SetEase(Ease.OutQuad));
            
            return seq;
        }
        
        private void ImmediatelyChangeTrack(AudioSource active, AudioSource next, AudioClip clip, float endVolume) {
            CleanupSource(active);
            
            next.clip = clip;
            next.loop = true;
            next.volume = endVolume;
            next.Play();
        }

        private Sequence CreateCrossFadeSequence(AudioSource active, AudioSource next, AudioClip clip, float duration, float endVolume) {
            var seq = DOTween.Sequence();

            next.clip = clip;
            next.loop = true;
            next.volume = 0;
            next.Play();

            seq.Join(active.DOFade(0, duration).SetEase(Ease.InOutQuad));
            seq.Join(next.DOFade(endVolume, duration).SetEase(Ease.InOutQuad));

            seq.OnComplete(() => CleanupSource(active));

            return seq;
        }

        private Sequence CreateFadeOutInSequence(AudioSource active, AudioSource next, AudioClip clip, float duration, float endVolume) {
            var seq = DOTween.Sequence();

            seq.Append(active.DOFade(0, duration).SetEase(Ease.InQuad));
            
            seq.AppendCallback(() => CleanupSource(active));

            if (defaultFadeOutInSilenceGap > 0) {
                seq.AppendInterval(defaultFadeOutInSilenceGap);
            }

            seq.AppendCallback(() => {
                next.clip = clip;
                next.loop = true;
                next.volume = 0;
                next.Play();
            });

            seq.Append(next.DOFade(endVolume, duration).SetEase(Ease.OutQuad));

            return seq;
        }

        private void CleanupSource(AudioSource source) {
            source.Stop();
            source.clip = null;
            source.volume = 0;
        }
    }
}