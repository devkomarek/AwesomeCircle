using System;
using System.Collections;
using Assets.Scripts.GameMaster;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts
{
    public class AudioController : MonoBehaviour
    {
        private const string AUDIO_SOURCE_LVL_1 = "Awesome Circle - Welcome in the Circle - komarek"; 
        private const string AUDIO_SOURCE_LVL_2 = "Awesome Circle - Radians or Degrees - komarek";
        private const string AUDIO_SOURCE_LVL_3 = "Awesome Circle - Sinus 360 - komarek";
        private const string AUDIO_SOURCE_LVL_4 = "Awesome Circle - Polygon - komarek";
        private const string AUDIO_SOURCE_LVL_5 = "Awesome Circle - We are Unstoppable - komarek";

        private readonly float[] _audioTimeLvl1 = {0, 13, 29.5f, 44.5f,73.9f };
        private readonly float[] _audioTimeLvl2 = {0, 24, 70, 103, 127 };
        private readonly float[] _audioTimeLvl3 = {0, 12, 36, 59.2f, 84.13f, 102 };
        private readonly float[] _audioTimeLvl4 = { 0, 37.5f, 68.2f };
        private readonly float[] _audioTimeLvl5 = {0, 6.5f, 21f, 35.5f, 64, 77.5f };

        public Timer Timer;
        public AudioMixer MasterLvl;
        public AudioMixer Master;
        public VoiceController Voice;
        static public bool IsMuteAudio;
        private AudioSource[] _audioSources;
        private GameInfo _gameInfo;
        private GM _gM;

        private SoundVisualizer _soundVisual;
        private AudioMixerSnapshot _overloadSnapshot;
        //private AudioMixerSnapshot _endGameSnapshot;
        private AudioSource _swipeRight;
        private AudioSource _swipeLeft;
        private AudioSource _endGameSource;
        private AudioSource _smallButton;
        private AudioMixerSnapshot _courentAudioMixerSnapshot;
    //    private AudioClip _bufferClip;

        public AudioSource CourentSource { get; set; }

        void Start ()
        {
            GameObject effects = transform.Find("Effects").gameObject;
            _audioSources = GetComponentsInChildren<AudioSource>();
            _gM = GameObject.Find("Awesome Circle").transform.Find("Game Master").GetComponent<GM>();
            _gameInfo = GameObject.Find("Awesome Circle").transform.Find("Game Master").GetComponent<GameInfo>();  
            if(MasterLvl == null) ThrowException(AuthenticationMethod.AUDIO_MIXER_IS_NULL);
            _overloadSnapshot = Master.FindSnapshot("Overload");
           // _endGameSnapshot = Master.FindSnapshot("End Game");
            _swipeLeft = effects.transform.Find("Swipe Left").GetComponent<AudioSource>();
            _swipeRight = effects.transform.Find("Swipe Right").GetComponent<AudioSource>();
            _endGameSource = effects.transform.Find("End Game").GetComponent<AudioSource>();
            _smallButton = effects.transform.Find("Small Button").GetComponent<AudioSource>();
            _soundVisual = GetComponent<SoundVisualizer>();
        }

        void Update()
        {
            if (_gM.IsLvlPlay && CourentSource.isPlaying != null && _gameInfo.GetFloatTime() > 10 && CourentSource.isPlaying == false)
            {
                if(_gameInfo.Lvl == 5 && _gameInfo.GetFloatTime() >= 1) return;
                CourentSource.time = 0;
                CurentSourcePlay();
            }

            if (_isPause)
            {
                CourentSource.Pause();
            }
        }

        public void StartLvlPlay()
        {
            Voice.PlayWhennStartGame();
            SetUpCourentAudio();
            CurentSourcePlay();
            _soundVisual.SmoothSpeed = 8.0f;
            _firstPlay = 2;
        }

        
        public void PauseLvl()
        {
            _isPause = true;
            CourentSource.Pause();            
        }

        public void ResumeLvl()
        {
            _isPause = false;
            CourentSource.UnPause();
        }

        public void StopLvlPlay()
        {
            SetUpCourentAudio();
            Voice.PlayWhennLose();
            _endGameSource.time = 0;
            _endGameSource.Play();
        }

        public void RestartLvlPlay()
        {
            Voice.PlayWhennRestartGame();
            SetUpCourentAudio();
            RandromPlay();
        }

        public void ButtonStartPlay()
        {
            Voice.PlayButtonStart();
        }

        public void ButtonSmallPlay()
        {
            _smallButton.Play();
        }

        public void ButtonSmallMuterPLay(bool mute)
        {
            Master.FindSnapshot("Snapshot").TransitionTo(0);
            _smallButton.Play();
            StartCoroutine(Wait(mute));

        }

        IEnumerator Wait(bool mute)
        {
            while (_smallButton.isPlaying)
            {
                yield return null;
            }
            if (_smallButton.isPlaying == false)
            {
                if (mute)
                {
                    Master.FindSnapshot("Silence").TransitionTo(0);
                    IsMuteAudio = true;
                }
                else
                {
                    Master.FindSnapshot("Snapshot").TransitionTo(0);
                    IsMuteAudio = false;
                }
                    
              //  AudioListener.volume = mute == true ? 0 : 1;
            }
            
        }

        public void LockLvlPlay()
        {
            _soundVisual.SmoothSpeed = -52.0f;
        }

        public void UnloackLvlPlay()
        {
            _soundVisual.SmoothSpeed = 50.0f;
        }

        public void PlayIncredible()
        {
            Voice.PlayWhennIs100Seconds();
        }

        public void OverloadPlay()
        {
            if(Timer.Seconds>2)
            StartCoroutine(AudioEffect());
        }

        private bool _isAudioEffectActive;
        private IEnumerator AudioEffect()
        {
            if(_isAudioEffectActive)
                _soundVisual.SmoothSpeed = -28.0f;
            else
                _soundVisual.SmoothSpeed = -5.0f;

            _isAudioEffectActive = true;
            if (!IsMuteAudio)
                _overloadSnapshot.TransitionTo(0.24f);
            yield return new WaitForSeconds(.25f);
            if (!IsMuteAudio)
                Master.FindSnapshot("Snapshot").TransitionTo(1f);
            _soundVisual.SmoothSpeed = 8.0f;
            _isAudioEffectActive = true;
            yield return new WaitForSeconds(.5f);
            _isAudioEffectActive = false;
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

        public void EndAppPlay()
        {
            Voice.PlayWhennWinGame();
            CourentSource.time = 0;
            CourentSource.Stop();
        }

        public IEnumerator WaitCongratulation()
        {
            yield return new WaitForSeconds(2f);
            
        }

        public void PlayAwesome60Seconds()
        {
            Voice.PlayWhennIs60Seconds();
        }

        public void Play()
        {
            CourentSource.Play();
        }

        private bool _isPause = false;
        public void Pause()
        {
            CourentSource.Pause();
        }

        private void CurentSourcePlay()
        {
            CourentSource.Play();
            _soundVisual.StartSoundVisual();
        }

        private int _firstPlay = 2;
        private void RandromPlay()
        {
            _firstPlay --;
            if (_firstPlay < 0)
                CourentSource.time = ReturnTimeAudio(_gameInfo.Lvl);
            else
                CourentSource.time = 0;
        
            CourentSource.Play();
        }

        private float ReturnTimeAudio(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return _audioTimeLvl1[UnityEngine.Random.Range(0,5)];
                case 2:
                    return _audioTimeLvl2[UnityEngine.Random.Range(0, 5)];
                case 3:
                    return _audioTimeLvl3[UnityEngine.Random.Range(0, 3)];
                case 4:
                    return _audioTimeLvl4[UnityEngine.Random.Range(0, 6)];
                case 5:
                    return _audioTimeLvl5[UnityEngine.Random.Range(0, 6)];
            }
            return 0;
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
