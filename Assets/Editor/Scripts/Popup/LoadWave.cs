using System.Collections.Generic;
using Assets.Scripts.ScriptableObject.Database;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Scripts.Popup
{
    public class LoadRoundWave : PopupWindowContent
    {
        private const string WAVE_DATABASE_NAME = @"WaveDatabase.asset";
        private const string DATABASE_PATH = @"Editor/Database";
        private const string DATABASE_PATH_ARCHIVE = @"Editor/Database/Archive/Lvl";
        private WaveDatabase _waveDatabaseTemp;
        private Vector2 _scrollVector2;
        private List<bool> _toogleList;
        private WaveDatabase _waveDatabaseOrg;

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 400);
        }

        public override void OnGUI(Rect rect)
        {
            _scrollVector2 = EditorGUILayout.BeginScrollView(_scrollVector2, "Box", GUILayout.ExpandHeight(true));

            for (var j = 0; j < _waveDatabaseTemp.Count; j++)
            {
                _toogleList[j] = EditorGUILayout.Toggle(_waveDatabaseTemp.Get(j).WaveName, _toogleList[j]);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Count: " + _toogleList.Count);
            EditorGUILayout.EndScrollView();
        }

        public override void OnOpen()
        {
            _toogleList = new List<bool>();
            _waveDatabaseTemp = ScriptableObject.CreateInstance<WaveDatabase>();
            _waveDatabaseOrg = WaveDatabase.GetDatabase<WaveDatabase>(DATABASE_PATH, WAVE_DATABASE_NAME);
            for (int i = 0; i < 5; i++)
            {
                
                for (int j = 0; j < 3; j++)
                {
                    var wave = WaveDatabase.GetDatabase<WaveDatabase>(DATABASE_PATH_ARCHIVE + (i + 1) + "/lvl" + j, WAVE_DATABASE_NAME);
                    var waveDatabaseCopy = Object.Instantiate(wave);
                    for (int k = 0; k < waveDatabaseCopy.Count; k++)
                    {
                        _waveDatabaseTemp.Add(waveDatabaseCopy.Get(k));
                        _toogleList.Add(false);
                    }
                }
            }                  
        }

        public override void OnClose()
        {
            for (var j = 0; j < _waveDatabaseTemp.Count; j++)
            {
                if (_toogleList[j])
                {
                    _waveDatabaseOrg.Add(_waveDatabaseTemp.Get(j));
                }
            }
            _toogleList.Clear();
        }
    }
}