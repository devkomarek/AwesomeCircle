using System.Collections.Generic;
using Assets.Editor.Database;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Popup
{
    public class LoadRoundBarrier : PopupWindowContent
    {
        private const string WAVE_DATABASE_NAME = @"WaveDatabase.asset";
        private const string DATABASE_PATH = @"Database";
        private RoundBarrierDatabase _roundBarrierDatabase;
        private Vector2 _roundBarrierscrollVector2;
        private List<bool> _toogleList;
        private WaveDatabase _waveDatabase;
        public int SellectedWave;

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 400);
        }

        public override void OnGUI(Rect rect)
        {
            _roundBarrierscrollVector2 = EditorGUILayout.BeginScrollView(_roundBarrierscrollVector2, "Box", GUILayout.ExpandHeight(true));

            for (var j = 0; j < _roundBarrierDatabase.Count; j++)
            {
                _toogleList[j] = EditorGUILayout.Toggle(_roundBarrierDatabase.Get(j).RoundBarrierName, _toogleList[j]);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Count: " + _toogleList.Count);
            EditorGUILayout.EndScrollView();
        }

        public override void OnOpen()
        {
            _waveDatabase = WaveDatabase.GetDatabase<WaveDatabase>(DATABASE_PATH, WAVE_DATABASE_NAME);
            var waveCloneDatabase = Object.Instantiate(_waveDatabase);
            _roundBarrierDatabase = ScriptableObject.CreateInstance<RoundBarrierDatabase>();
            _toogleList = new List<bool>();
            for (var i = 0; i < _waveDatabase.Count; i++)
            {
                for (var j = 0; j < _waveDatabase.Get(i).RoundBarriersList.Count; j++)
                {
                    _roundBarrierDatabase.Add(waveCloneDatabase.Get(i).RoundBarriersList[j]);
                    _toogleList.Add(false);
                }
            }
        }

        public override void OnClose()
        {
            for (var j = 0; j < _roundBarrierDatabase.Count; j++)
            {
                if (_toogleList[j])
                {
                    _waveDatabase.Get(SellectedWave).RoundBarriersList.Add(_roundBarrierDatabase.Get(j));
                }
            }
            _roundBarrierDatabase.Clear();
            _toogleList.Clear();
        }
    }
}