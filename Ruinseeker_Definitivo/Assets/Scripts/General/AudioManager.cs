using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ruinseeker
{
    public class AudioManager : MonoBehaviour
    {
        static public AudioManager instance;

        [Header("Music")]
        [SerializeField] private AudioSource titleScreenSource;
        [SerializeField] private AudioSource menuSource;
        [SerializeField] private AudioSource inGameSource;
        private bool isPlayingTitleMusic = false;
        private bool isPlayingMenuMusic = false;
        private bool isPlayingInGameMusic = false;

        [Header("Player")]
        [SerializeField] private AudioSource jumpSource;
        [SerializeField] private AudioSource dashSource;
        [SerializeField] private AudioSource deathSource;
        [SerializeField] private AudioSource damageSource;
        [SerializeField] private AudioSource GemCollected;
        [SerializeField] private AudioSource portalSource;

        [Header("Enemy")]
        [SerializeField] private AudioSource BommerangSource;

        [Header("UI")]
        [SerializeField] private AudioSource buttonSource;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
            DontDestroyOnLoad(this.gameObject);
        }

        public void PlayTitleMusic()
        {
            if (!isPlayingTitleMusic)
            {
                titleScreenSource.Play();
                menuSource.Stop();
                inGameSource.Stop();
                isPlayingTitleMusic = true;
                isPlayingMenuMusic = false;
                isPlayingInGameMusic = false;
            }
        }

        public void PlayMenuMusic() {
            if (!isPlayingMenuMusic)
            {
                titleScreenSource.Stop();
                menuSource.Play();
                inGameSource.Stop();
                isPlayingTitleMusic = false;
                isPlayingMenuMusic = true;
                isPlayingInGameMusic = false;
            }
        }

        public void PlayInGameMusic()
        {
            if (!isPlayingInGameMusic)
            {
                titleScreenSource.Stop();
                menuSource.Stop();
                inGameSource.Play();
                isPlayingTitleMusic = false;
                isPlayingMenuMusic = false;
                isPlayingInGameMusic = true;
            }
        }

        public void PlayBoomerangSource()
        {
            BommerangSource.Play();
        }
        public void PlayDeathSound()
        {
            deathSource.Play();
        }

        public void PlayDashSound()
        {
            dashSource.Play();
        }

        public void PlayPortalSound()
        {
            portalSource.Play();
        }
        public void PlayDamageSound()
        {
            damageSource.Play();
        }

        public void PlayGemCollectedSound()
        {
            GemCollected.Play();
        }
        public void PlayJumpSound()
        {
            jumpSource.Play();
        }

        public void PlayButtonSound()
        {
            buttonSource.Play();
        }
    }
}
