using System;
using DG.Tweening;
using Enums;
using UnityEngine;

namespace Audio {
    public class MusicManager : PersistentSingleton<MusicManager> {
        [Header("Audio Sources")] 
        [SerializeField] private AudioSource sourceA;
        [SerializeField] private AudioSource sourceB;

        [Header("Settings")] 
        [SerializeField] private float maxVolume = 1.0f;
        [SerializeField] private float crossFadeDuration = 2.0f;
        [SerializeField] private float fadeOutInDuration = 1.5f;
        [SerializeField] private float fadeOutInSilenceGap = 0.5f;
        [SerializeField] private float fadeInDuration = 1.0f;
        [SerializeField] private float fadeOutDuration = 1.0f;

        private Sequence _currentTransition;
        private bool _isSourceAPlaying;

        private void Start() {
            if (sourceA.isPlaying) _isSourceAPlaying = true;
            else sourceA.volume = 0;
            
            sourceB.volume = 0;
        }

        public void PlayMusic(AudioClip newClip, AudioTransitionType transitionType) {
            var activeSource = _isSourceAPlaying ? sourceA : sourceB;

            if (activeSource.clip == newClip && activeSource.isPlaying) return;

            if (_currentTransition != null && _currentTransition.IsActive())
                _currentTransition.Kill();

            var newSource = _isSourceAPlaying ? sourceB : sourceA;

            switch (transitionType) {
                case AudioTransitionType.None:
                    ImmediatelyChangeTrack(activeSource, newSource, newClip);
                    _currentTransition = null; 
                    break;
                case AudioTransitionType.FadeIn:
                    _currentTransition = CreateFadeInSequence(activeSource, newSource, newClip);
                    break;
                    
                case AudioTransitionType.CrossFade:
                    _currentTransition = CreateCrossFadeSequence(activeSource, newSource, newClip);
                    break;
                    
                case AudioTransitionType.FadeOutIn:
                    _currentTransition = CreateFadeOutInSequence(activeSource, newSource, newClip);
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(transitionType), transitionType, null);
            }

            _currentTransition?.Play();
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
            
            _currentTransition.Play();
        }

        private Sequence CreateFadeInSequence(AudioSource activeSource, AudioSource newSource, AudioClip clip) {
            CleanupSource(activeSource);
            CleanupSource(newSource);
            
            newSource.clip = clip;
            newSource.loop = true;
            newSource.volume = 0;
            newSource.Play();
            
            var seq = DOTween.Sequence();
            seq.Append(newSource.DOFade(maxVolume, fadeInDuration).SetEase(Ease.OutQuad));
            
            return seq;
        }
        
        private void ImmediatelyChangeTrack(AudioSource active, AudioSource next, AudioClip clip) {
            next.clip = clip;
            next.loop = true;
            next.volume = maxVolume;
            next.Play();
            
            CleanupSource(active);
        }

        private Sequence CreateCrossFadeSequence(AudioSource active, AudioSource next, AudioClip clip) {
            var seq = DOTween.Sequence();

            next.clip = clip;
            next.loop = true;
            next.volume = 0;
            next.Play();

            seq.Join(active.DOFade(0, crossFadeDuration).SetEase(Ease.InOutQuad));
            seq.Join(next.DOFade(maxVolume, crossFadeDuration).SetEase(Ease.InOutQuad));

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

            seq.Append(next.DOFade(maxVolume, fadeOutInDuration).SetEase(Ease.OutQuad));

            return seq;
        }

        private void CleanupSource(AudioSource source) {
            source.Stop();
            source.clip = null;
            source.volume = 0;
        }
    }
}