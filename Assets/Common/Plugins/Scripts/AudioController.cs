using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Audio
{
    public enum componentType
    {
        Sound,
        Music,
        SoundButton,
        MusicButton
    }

    public class AudioController : MonoBehaviour
    {
        private AudioManager manager;
        private AudioSource[] srcs;
        private bool isMusic;
        public componentType type;

        private void Start()
        {
            manager = AudioManager.instance;
            CheckControllerState();
        }

        public void CheckControllerState()
        {
            switch (type)
            {
                case componentType.SoundButton:
                    {
                        manager.SoundButton = GetComponent<Button>();
                        manager.UpdateSoundButton();
                        manager.CheckAllSoundsState();
                        break;
                    }
                case componentType.MusicButton:
                    {
                        manager.MusicButton = GetComponent<Button>();
                        manager.UpdateMusicButton();
                        manager.CheckAllSoundsState();
                        break;
                    }
                case componentType.Sound:
                    {
                        srcs = GetComponents<AudioSource>();
                        foreach (var src in srcs)
                        {
                            src.mute = !Data.Instance.Sound;
                        }

                        manager.ChangeSoundStateCallback += CheckSoundState;
                        break;
                    }
                case componentType.Music:
                    {
                        srcs = GetComponents<AudioSource>();
                        foreach (var src in srcs)
                        {
                            src.mute = !Data.Instance.Music;
                        }

                        manager.ChangeSoundStateCallback += CheckSoundState;
                        break;
                    }
            }
        }

        private void OnDestroy()
        {
            if (type == componentType.MusicButton || type == componentType.SoundButton) return;
            if (manager == null) manager = AudioManager.instance;
            manager.ChangeSoundStateCallback -= CheckSoundState;
        }

        private void CheckSoundState()
        {
            if (type == componentType.Music)
            {
                foreach (var src in srcs)
                {
                    src.mute = !Data.Instance.Music;
                }
            }

            if (type == componentType.Sound)
            {
                foreach (var src in srcs)
                {
                    src.mute = !Data.Instance.Sound;
                }
            }
        }
    }
}