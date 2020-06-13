using Assets.Source.Components.Base;
using Assets.Source.Constants;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Source.Components.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicPlayerComponent : ComponentBase
    {
        private AudioSource audioSource;

        private bool isLooping = true;

        private AudioClip music;

        public enum Song
        {
            Prototype
        }

        private Dictionary<Song, AudioClip> songDictionary;

        public override void ComponentAwake()
        {
            audioSource = GetRequiredComponent<AudioSource>();

            songDictionary = new Dictionary<Song, AudioClip>()
            {
                { Song.Prototype, GetRequiredResource<AudioClip>($"{ResourcePaths.MusicFolder}/Prototype") }
            };

            base.ComponentAwake();
        }

        public void Loop(Song song)
        {

            audioSource.Stop();
            music = songDictionary[song];
            audioSource.loop = true;
            audioSource.clip = music;
            audioSource.Play();
        }

        public void FadeToSong(Song song)
        {
            //todo: implement
        }


    }
}
