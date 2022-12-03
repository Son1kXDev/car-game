using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Plugins.Audio
{
    public enum SoundsType
    {
        multi,
        solo
    }

    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("Button Sprites")]
        public Sprite Enable;

        public Sprite Disable;

        [Header("Buttons")]
        public Button SoundButton;

        public Button MusicButton;

        [NonSerialized] public AudioSource source;

        public bool OnlyOneSoundState;
        public SoundsType type;

        public event Action ChangeSoundStateCallback;

        private Image soundImage, musicImage;

        private Data data;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            data = Data.Instance;
            source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            type = OnlyOneSoundState ? SoundsType.solo : SoundsType.multi;
        }

        public void UpdateSoundButton()
        {
            soundImage = SoundButton.GetComponent<Image>();
            SoundButton.onClick.AddListener(ChangeSoundState);
        }

        public void UpdateMusicButton()
        {
            musicImage = MusicButton.GetComponent<Image>();
            MusicButton.onClick.AddListener(ChangeMusicState);
        }

        public void ChangeSoundState()
        {
            data.Sound = !data.Sound;
            CheckAllSoundsState();
        }

        public void ChangeMusicState()
        {
            data.Music = !data.Music;
            CheckAllSoundsState();
        }

        public void CheckAllSoundsState()
        {
            if (soundImage != null) soundImage.sprite = data.Sound ? Enable : Disable;
            if (musicImage != null) musicImage.sprite = data.Music ? Enable : Disable;
            ChangeSoundStateCallback?.Invoke();
        }
    }
}