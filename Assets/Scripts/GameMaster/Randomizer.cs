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
        public float BetweenRoundBarrierTime;
        public float MinBetweenWavetime;
        public float MaxBetweenWavetime;
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
            if (_triger && _pullRoundBarrierIsActive == false)
            {      
                 
                ChangeWaveDatabaseDependingAtTime();      
                PrepareWave();
                PullRoundBarrier();
            }
        }

        public void SetUpWaveDatabase(int lvl)
        {
            _waveDatabase = LoadResources(_waveDatabase, "LvL"+lvl+"/lvl0/WaveDatabase");
            if (_waveDatabase == null)
                throw new Exception("WaveDatabase must be not null!");
            _triger = true;

        }

        public void Disable()
        {
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

        private int _courentChapter = -1;
        private void ChangeWaveDatabaseDependingAtTime()
        {
            float courentTime = _gameInfo.GetFloatTime();
            if (_courentChapter != 0 &&courentTime > 0 && courentTime < HALF_CHAPTER)
            {
                _waveDatabase = null;
                _waveDatabase = LoadResources(_waveDatabase, "LvL" + _gameInfo.Lvl + "/lvl0/WaveDatabase");
                _courentChapter = 0;
            }
            else if (_courentChapter != 1 && courentTime > HALF_CHAPTER && courentTime < THIRD_END_CHAPTER)
            {
                _waveDatabase = null;
                _waveDatabase = LoadResources(_waveDatabase, "LvL" + _gameInfo.Lvl + "/lvl1/WaveDatabase");
                _courentChapter = 1;
            }               
            else if (_courentChapter != 2 && courentTime > THIRD_END_CHAPTER)
            {
                _waveDatabase = null;
                _waveDatabase = LoadResources(_waveDatabase, "LvL" + _gameInfo.Lvl + "/lvl2/WaveDatabase");
                _courentChapter = 2;
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
            var times = ReturnTimeBetweenRoundBarrierList(_readyWave);
            for (var i = 0; i < times.Count; i++)
            {
                if (i == 0)               
                    _roundBarrierGenerator.CreateRoundBarrier(_readyWave.RoundBarriersList[0], _randomOffset);                             
                    yield return new WaitForSeconds(float.Parse(times[i]) + BetweenRoundBarrierTime);
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