using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.GameMaster;
using UnityEngine;
using UnityEngine.Analytics;

namespace Assets.Scripts
{
    public class UnityAnalytics : MonoBehaviour
    {
        private GameInfo _gameInfo;
        private Timer _timer;
        private List<string> _achieveLvl;
        private Dictionary<string, object> _dead;
        private List<int> _playTheSameLvl;
        private int _deadCounter;
        void Start ()
        {
            _deadCounter = 0;
            _achieveLvl = new List<string>();
            _dead = new Dictionary<string, object>();
            _playTheSameLvl = new List<int>();
            _gameInfo = GetComponent<GameInfo>();
            _timer = GameObject.Find("Awesome Circle").transform.FindChild("UI").GetComponent<Timer>();
        }

        public void Analise()
        {
            _deadCounter ++;
            GetAheadCounte();
            DeadCounte();
            PlayTheSameLvlCounte();
            //Analytics.limitUserTracking = true;
        }

        private void GetAheadCounte()
        {
            if (_timer.Seconds >= 60 && GameManager.LvlIsLock(_gameInfo.Lvl + 1))
            {
                _achieveLvl.Add(_gameInfo.LvlName);
                Debug.Log("ukonczyles lvl");
            }
              
        }

        private void DeadCounte()
        {
            if (_timer.Seconds >= 0 && _timer.Seconds <= 30)
                _dead.Add("1 Half " + _deadCounter,_gameInfo.Lvl);
            else if (_timer.Seconds > 30 && _timer.Seconds < 60)
                _dead.Add("2 Half " + _deadCounter, _gameInfo.Lvl);
            else if(_timer.Seconds >= 60)
                _dead.Add("ExtraT " + _deadCounter, _gameInfo.Lvl);
        }

        private void PlayTheSameLvlCounte()
        {
            if (GameManager.GetFloatBest(_gameInfo.Lvl) >= 3600)
                _playTheSameLvl.Add(_gameInfo.Lvl);
        }

        public void SendData()
        {
            if (_achieveLvl.Count != 0)
            {
                for (int i = 0; i < _achieveLvl.Count; i++)
                {
                    Debug.Log(_achieveLvl[i]);

                    Analytics.CustomEvent("Progres", new Dictionary<string, object>() { { "Level", _achieveLvl[i] } });
                }
            }

            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            for (int i = 1; i < 6; i++)
            {
                dictionary.Clear();
                var halfs = _dead.Where(x => (int)x.Value == i).Select(x => x.Key);
                if (halfs.Count() != 0)
                {
                    int half1 = 0, half2 = 0, extratime = 0;
                    foreach (var half in halfs)
                    {
                        var subString = half.Substring(0, 6);
                        if (subString == "1 Half")
                            half1++;
                        if (subString == "2 Half")
                            half2++;
                        if (subString == "ExtraT")
                            extratime++;
                    }
                    if (half1 != 0)
                        dictionary.Add("1 Half", half1);
                    if (half2 != 0)
                        dictionary.Add("2 Half", half2);
                    if (extratime != 0)
                        dictionary.Add("Extra Time", extratime);
                    Analytics.CustomEvent("Zgony Lvl " + i, dictionary);
                }
            }

            if (_playTheSameLvl.Count != 0)
            {
                for (int i = 1; i < 6; i++)
                {
                    var res = _playTheSameLvl.Count(x => x == i);
                    if (res != 0)
                        Analytics.CustomEvent("Gram jeszcze raz level " + i, new Dictionary<string, object>() { { "Level", res } });
                }
            }
//            _achieveLvl.Clear();
//            _dead.Clear();
//            _playTheSameLvl.Clear();
           // Analytics.FlushEvents();
        }

        void OnApplicationQuit()
        {
            SendData();   
        }
    }
}
