using System;
using System.Collections;
using Assets.Scripts.GameMaster;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts
{
    public class AudioController : MonoBehaviour
    {
        private const string AUDIO_SOURCE_LVL_1 = "Open Hexagon - Apeirogon - Milky Ways";
        private const string AUDIO_SOURCE_LVL_2 = "Open Hexagon - Baby Steps - Dr Finkel Fracken";
        private const string AUDIO_SOURCE_LVL_3 = "Open Hexagon - Flattering Shape - Starship Showdown";
        private const string AUDIO_SOURCE_LVL_4 = "Open Hexagon - Pi - 3.1415 - Call Me Katla";
        private const string AUDIO_SOURCE_LVL_5 = "Open Hexagon - Second Dimension - Maze Of Mayonnaise";

        public AudioMixer MasterLvl;
        public AudioMixer Master;
        private AudioSource[] _audioSources;
        private GameInfo _gameInfo;

        private SoundVisualizer _soundVisual;
        private AudioMixerSnapshot _overloadSnapshot;
        //private AudioMixerSnapshot _endGameSnapshot;
        private AudioSource _swipeRight;
        private AudioSource _swipeLeft;
        private AudioSource _endGameSource;
        private AudioSource _overloadSource;
        private AudioMixerSnapshot _courentAudioMixerSnapshot;
        private AudioClip _bufferClip;

        public AudioSource CourentSource { get; set; }

        void Start ()
        {
            GameObject effects = transform.FindChild("Effects").gameObject;
            _audioSources = GetComponentsInChildren<AudioSource>();
            _gameInfo = GameObject.Find("Awesome Circle").transform.FindChild("Game Master").GetComponent<GameInfo>();  
            if(MasterLvl == null) ThrowException(AuthenticationMethod.AUDIO_MIXER_IS_NULL);
            _overloadSnapshot = Master.FindSnapshot("Overload");
           // _endGameSnapshot = Master.FindSnapshot("End Game");
            _swipeLeft = effects.transform.FindChild("Swipe Left").GetComponent<AudioSource>();
            _swipeRight = effects.transform.FindChild("Swipe Right").GetComponent<AudioSource>();
            _endGameSource = effects.transform.FindChild("End Game").GetComponent<AudioSource>();
            _overloadSource = effects.transform.FindChild("Overload").GetComponent<AudioSource>();
            _soundVisual = GetComponent<SoundVisualizer>();
        }

        public void StartLvlPlay()
        {
            SetUpCourentAudio();
            CurentSourcePlay();
            _soundVisual.SmoothSpeed = 7.0f;
        }

        public void StopLvlPlay()
        {
            SetUpCourentAudio();
            _endGameSource.time = 0;
            _endGameSource.Play();
        }

        public void RestartLvlPlay()
        {
            SetUpCourentAudio();
            RandromPlay();
        }

        public void OverloadPlay()
        {
            _overloadSource.time = 0;
            _overloadSource.Play();
            StartCoroutine(AudioEffect());
        }

        public void LockLvlPlay()
        {
            _soundVisual.SmoothSpeed = -52.0f;
        }

        public void UnloackLvlPlay()
        {
            _soundVisual.SmoothSpeed = 50.0f;
        }

        private IEnumerator AudioEffect()
        {
            _soundVisual.SmoothSpeed =  -28.0f;
            _overloadSnapshot.TransitionTo(0.14f);
            yield return new WaitForSeconds(.25f);
            Master.FindSnapshot("Snapshot").TransitionTo(1f);
            _soundVisual.SmoothSpeed = 7.0f;

        }

        public void SwipeRightPlay()
        {
            _swipeRight.time = 0;
            _swipeRight.Play();
        }

        public void SwipeLeftPlay()
        {
            _swipeLeft.time = 0;
            _swipeLeft.Play();
        }

        public void StartAppPlay()
        {
        
        }

        public void EndAppPlay()
        {
            CourentSource.time = 0;
            CourentSource.Stop();
        }

        public void Play()
        {
            CourentSource.Play();
        }

        public void Pause()
        {
            CourentSource.Pause();
        }

        private void CurentSourcePlay()
        {
            CourentSource.Play();
            _soundVisual.StartSoundVisual();
        }

        private int _firstPlay = 1;
        private void RandromPlay()
        {
            _firstPlay --;
            if (_firstPlay < 0)
                CourentSource.time = UnityEngine.Random.Range(0, 120);
            else
                CourentSource.time = 0;
        
        
            CourentSource.Play();
        }

        private void SetUpCourentAudio()
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                if (audioSource.name == "Lvl " + _gameInfo.Lvl)
                {
                    audioSource.clip = Resources.Load<AudioClip>("Audio/" + ReturnNameAudio(_gameInfo.Lvl));
                    CourentSource = audioSource;
                }

            }
            if (CourentSource == null) ThrowException(AuthenticationMethod.AUDIO_SOURCE_IS_NULL);
            _courentAudioMixerSnapshot = MasterLvl.FindSnapshot("Lvl " + _gameInfo.Lvl);
            if (_courentAudioMixerSnapshot == null) ThrowException(AuthenticationMethod.AUDIO_SNAPSHOT_IS_NULL);
            _courentAudioMixerSnapshot.TransitionTo(0.1f);
            CourentSource.Stop();
            CourentSource.time = 0;
        }

        private string ReturnNameAudio(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return AUDIO_SOURCE_LVL_1;
                case 2:
                    return AUDIO_SOURCE_LVL_2;
                case 3:
                    return AUDIO_SOURCE_LVL_3;
                case 4:
                    return AUDIO_SOURCE_LVL_4;
                case 5:
                    return AUDIO_SOURCE_LVL_5;
                default:
                    return AUDIO_SOURCE_LVL_1;
            }
        }

        private void ThrowException(AuthenticationMethod authenticationMethod)
        {
            throw new Exception(authenticationMethod.ToString());
        }

        //type-safe-enum pattern
        public sealed class AuthenticationMethod
        {
            private readonly String _name;

            public static readonly AuthenticationMethod AUDIO_MIXER_IS_NULL = new AuthenticationMethod("Set up AudiMixer in Inspector");
            public static readonly AuthenticationMethod AUDIO_SOURCE_IS_NULL = new AuthenticationMethod("No finde Audio matching to lvl");
            public static readonly AuthenticationMethod AUDIO_SNAPSHOT_IS_NULL = new AuthenticationMethod("No finde Snapshot matching to lvl");
            public static readonly AuthenticationMethod OVERLOAD_SNAPSHOT_IS_NULL = new AuthenticationMethod("Set up OverloadSnapshot in Inspector");

            private AuthenticationMethod(String name)
            {
                _name = name;
            }

            public override String ToString()
            {
                return _name;
            }

        }
    }
}
