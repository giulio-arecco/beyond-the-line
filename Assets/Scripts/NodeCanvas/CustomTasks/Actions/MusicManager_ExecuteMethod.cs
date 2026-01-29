using System;
using Audio;
using Enums;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.CustomTasks.Actions {
    [Category("Custom/MusicManager")]
    [Description("Call different MusicManager methods without needing an instance reference.")]
    public class MusicManager_ExecuteMethod : ActionTask {
        public enum MusicManagerMethodType {
            PlayMusic,
            StopMusic,
        }

        public MusicManagerMethodType ActionToPerform;

        [ShowIf("ActionToPerform", (int) MusicManagerMethodType.PlayMusic)]
        public BBParameter<AudioClip> AudioClip;
        [ShowIf("ActionToPerform", (int) MusicManagerMethodType.PlayMusic)]
        public BBParameter<AudioTransitionType> AudioTransitionType;
        
        protected override string info => $"Call <b>MusicManager</b> method '<b>{ActionToPerform}</b>'";
        
        protected override void OnExecute() {
            if (MusicManager.TryGetInstance(out var musicManager)) {
                switch (ActionToPerform) {
                    case MusicManagerMethodType.PlayMusic:
                        musicManager.PlayMusic(AudioClip.value, AudioTransitionType.value);
                        break;
                    case MusicManagerMethodType.StopMusic:
                        musicManager.StopMusic();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(ActionToPerform), ActionToPerform, null);
                }
                EndAction(true);
            }
            else {
                EndAction(false);
            }
        }
    }
}