using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.ScriptableObject;
using Assets.Scripts.ScriptableObject.Database;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameMaster{
    public class Randomizer : MonoBehaviour
    {
        public Timer GameTimer;
        public int HalfIncriserSpeedLvl1;
        public int HalfIncriserSpeedLvl2;
        public int HalfIncriserSpeedLvl3;
        public int HalfIncriserSpeedLvl4;
        public int HalfIncriserSpeedLvl5;

        public float BetweenRoundBarrierTime;
        public float MinBetweenWavetime;
        public float MaxBetweenWavetime;
        public bool TestMode;
        private const int HALF_CHAPTER = 1800;
        private const int THIRD_END_CHAPTER = 3600;     
        private WaveDatabase _waveDatabase;
        private RoundBarrierGenerator _roundBarrierGenerator;
        private Wave _readyWave;
        private GameInfo _gameInfo;
        private bool _triger;
        private int _randomOffset;

        private void Start()
        {
            _gameInfo = GetComponent<GameInfo>();
            _roundBarrierGenerator = GetComponent<RoundBarrierGenerator>();         
        }

        private void Update()
        {
            if (!TestMode)
            {
                if (_triger && _pullRoundBarrierIsActive == false)
                {

                    ChangeWaveDatabaseDependingAtTime();
                    ChangeSpeedDependingAtTime();
                    PrepareWave();
                    PullRoundBarrier();
                }
                if(GameTimer.Seconds > 60 && _isWaveChangedBegin == false)
               StartCoroutine(ChangeWave());
            }
          
        }

        public void SetUpWaveDatabase(int lvl)
        {
            _waveDatabase = null;
            _isWaveChangedBegin = false;
            _waveDatabase = LoadResources(_waveDatabase, "LvL"+lvl+"/lvl0/WaveDatabase");
            if (_waveDatabase == null)
                throw new Exception("WaveDatabase must not be null!");
            _triger = true;

        }

        public void Disable()
        {
            _waveDatabase = null;
            _triger = false;
        }

        private void PrepareWave()
        {
            if (_readyWave == null)
            {
                int select = Random.Range(0, _waveDatabase.Count - 1);
                _readyWave = Instantiate(_waveDatabase).Get(select);
            }
        }

        private int _courentChapterForDatabase = -1;
        private void ChangeWaveDatabaseDependingAtTime()
        {
            float courentTime = _gameInfo.GetFloatTime();
            if (_courentChapterForDatabase != 0 &&courentTime > 0 && courentTime < HALF_CHAPTER)
            {
                _waveDatabase = null;
                _waveDatabase = LoadResources(_waveDatabase, "LvL" + _gameInfo.Lvl + "/lvl0/WaveDatabase");
                _courentChapterForDatabase = 0;
            }
            else if (_courentChapterForDatabase != 1 && courentTime > HALF_CHAPTER && courentTime < THIRD_END_CHAPTER)
            {
                _waveDatabase = null;
                _waveDatabase = LoadResources(_waveDatabase, "LvL" + _gameInfo.Lvl + "/lvl1/WaveDatabase");
                _courentChapterForDatabase = 1;
            }               
            else if (_courentChapterForDatabase != 2 && courentTime > THIRD_END_CHAPTER)
            {
                _waveDatabase = null;
                _waveDatabase = LoadResources(_waveDatabase, "LvL" + _gameInfo.Lvl + "/lvl2/WaveDatabase");
                _courentChapterForDatabase = 2;
            }
                
        }

        private int _courentChapterForSpeed = -1;
        private int _timerCount = 60;
        private void ChangeSpeedDependingAtTime()
        {
            float courentTime = _gameInfo.GetFloatTime();
            if (_courentChapterForSpeed != 1 && courentTime > HALF_CHAPTER && courentTime < THIRD_END_CHAPTER)
            {
                switch (_gameInfo.Lvl)
                {
                    case 1:
                        _roundBarrierGenerator.Speed += _roundBarrierGenerator.Speed * 1 / HalfIncriserSpeedLvl1;
                        _roundBarrierGenerator.BasicsSpeed += _roundBarrierGenerator.BasicsSpeed * 1 / HalfIncriserSpeedLvl1;
                        break;
                    case 2:
                        _roundBarrierGenerator.Speed += _roundBarrierGenerator.Speed * 1 / HalfIncriserSpeedLvl2;
                        _roundBarrierGenerator.BasicsSpeed += _roundBarrierGenerator.BasicsSpeed * 1 / HalfIncriserSpeedLvl2;
                        break;
                    case 3:
                        _roundBarrierGenerator.Speed += _roundBarrierGenerator.Speed * 1 / HalfIncriserSpeedLvl3;
                        _roundBarrierGenerator.BasicsSpeed += _roundBarrierGenerator.BasicsSpeed * 1 / HalfIncriserSpeedLvl3;
                        break;
                    case 4:
                        _roundBarrierGenerator.Speed += _roundBarrierGenerator.Speed * 1 / HalfIncriserSpeedLvl4;
                        _roundBarrierGenerator.BasicsSpeed+= _roundBarrierGenerator.BasicsSpeed * 1 / HalfIncriserSpeedLvl4;
                        break;
                    case 5:
                        _roundBarrierGenerator.Speed += _roundBarrierGenerator.Speed * 1 / HalfIncriserSpeedLvl5;
                        _roundBarrierGenerator.BasicsSpeed += _roundBarrierGenerator.BasicsSpeed * 1 / HalfIncriserSpeedLvl5;
                        break;
                }
                _courentChapterForSpeed = 1;
            }
            else if (_courentChapterForSpeed == 1 && courentTime > THIRD_END_CHAPTER)
            {
                _roundBarrierGenerator.Speed += _roundBarrierGenerator.Speed*1/6;
                _roundBarrierGenerator.BasicsSpeed += _roundBarrierGenerator.BasicsSpeed*1/6;
                _courentChapterForSpeed = 7;
            }
            else if (courentTime > THIRD_END_CHAPTER + 600 && GameTimer.Seconds > _timerCount && _roundBarrierGenerator.Speed < 20)
            {
                _roundBarrierGenerator.Speed += 0.1f;
                _roundBarrierGenerator.BasicsSpeed += 0.1f;
                _timerCount += 2;
                _courentChapterForSpeed += 2;
            }

        }

        private readonly float[] BETWEEN = {0.15f/60f, 0.04f/60f, -0.08f/60f, 0.05f/60f, 0.05f / 60f };
        private readonly float[] MIN = {0.12f/60f, -0.02f/60f, 0.13f/60f, 0.05f / 60f, 0.05f / 60f };
        private readonly float[] MAX = {0.31f/60f, 0.26f/60f, 0.22f/60f, 0.05f / 60f, 0.2f / 60f };

        private bool _isWaveChangedBegin = false;
        IEnumerator ChangeWave()
        {
            float d = 1;
            if (PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "true")
                d = 2;
            _isWaveChangedBegin = true;
            for (int i = 0; i < 60 / d; i++)
            {
                BetweenRoundBarrierTime -= BETWEEN[_gameInfo.Lvl - 1] / d;
                MinBetweenWavetime -= MIN[_gameInfo.Lvl - 1]/d;
                MaxBetweenWavetime -= MAX[_gameInfo.Lvl - 1]/d;
                yield return new WaitForSeconds(1f);
                if (_isWaveChangedBegin == false)
                    i += 100;
            }
        
        }

        private bool _pullRoundBarrierIsActive = false;
        private void PullRoundBarrier()
        {
            if (_readyWave != null)
            {
                _pullRoundBarrierIsActive = true;
                GenerateRandomRotation();
                StartCoroutine(WaitBetween(DoLast));
            }
        }


        private IEnumerator WaitBetween(Action doLast)
        {
            var time = _readyWave.TimeBetweenRoundBarriers;
            for (var i = 0; i < _readyWave.RoundBarriersList.Count -1; i++)
            {
                if (i == 0 && _triger)               
                    _roundBarrierGenerator.CreateRoundBarrier(_readyWave.RoundBarriersList[0], _randomOffset);                             
                    yield return new WaitForSeconds(BetweenRoundBarrierTime + float.Parse(time));
                if(_triger)
                _roundBarrierGenerator.CreateRoundBarrier(_readyWave.RoundBarriersList[i+1],_randomOffset);
            }
            doLast();
        }

        private void GenerateRandomRotation()
        {
            _randomOffset = UnityEngine.Random.Range(0, 360);
        }

        private void DoLast()
        {          
            StartCoroutine(Wait(Random.Range(MinBetweenWavetime,MaxBetweenWavetime)));           
        }

        private IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            _pullRoundBarrierIsActive = false;
            _readyWave = null;
           
        }

        private List<string> ReturnTimeBetweenRoundBarrierList(Wave wave)
        {
            var times = wave.TimeBetweenRoundBarriers.Split('/').ToList();
            times.RemoveAll(s => s == "");
            return times;
        }

        private T LoadResources<T>(T data, string path) where T : UnityEngine.ScriptableObject
        {
            return data ?? Resources.Load<T>(path);
        }
    }


}