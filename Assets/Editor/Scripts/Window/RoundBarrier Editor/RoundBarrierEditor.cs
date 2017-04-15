#region

using Assets.Scripts.ScriptableObject.Database;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

#endregion

namespace Assets.Editor.Scripts.Window.RoundBarrier_Editor{
    public partial class RoundBarrierEditor : EditorWindow{
        private const string ROUNDBARRIER_DATABASE_NAME = @"RoundBarrierDatabase.asset";
        private const string SEGMENT_DATABASE_NAME = @"SegmentDatabase.asset";
        private const string WAVE_DATABASE_NAME = @"WaveDatabase.asset";
        private const string DATABASE_PATH = @"Editor/Database";
        private const string DONE_WAVE_DATABASE_PATH = @"Editor/Database/Done";
        private const string DATABASE_PROPERTY_NAME = @"_database";
        private const string WAVE_PROPERTY_NAME = @"_waveName";
        private const string ROUNDBARRIER_PROPERTY_NAME = @"_roundBarrierName";
        private const string ROUNDBARRIER_PROPERTY_BETWEENTIME = @"_timeBetweenRoundBarriers";
        private const string SEGMENT_PROPERTY_START = @"_start";
        private const string SEGMENT_PROPERTY_END = @"_end";
        private RoundBarrierDatabase _roundBarrierDatabase;
        private ReorderableList _roundBarrierReorderableList;
        private Vector2 _roundBarrierscrollVector2;
        private SerializedObject _roundBarrierSerializedObject;
        private SegmentDatabase _segmentDatabase;
        private ReorderableList _segmentReorderableList;
        private SerializedObject _segmentSerializedObject;
        private WaveDatabase _waveDatabase;
        private ReorderableList _waveReorderableList;
        private Vector2 _waveScrollVector2;
        private SerializedObject _waveSerializedObject;
        private Vector2 _viewScrollVector2;
        private int _sellectedRoundBarrier;
        private int _sellectedRoundBarrierRemove = -1;
        private int _sellectedSegmentRemove = -1;
        private int _sellectedWave;
        private int _sellectedWaveRemove = -1;

        [MenuItem("Window/RoundBarrierEditorWindow")]
        private static void Init()
        {
            var window = GetWindow<RoundBarrierEditor>();
            window.minSize = new Vector2(600, 300);
            window.maxSize = new Vector2(650, 600);
            window.Show();
        }

        private void OnEnable()
        {
            GetDatabase();
            _sellectedWave = 0;
            _sellectedRoundBarrier = 0;
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("Box", GUILayout.ExpandHeight(true));
            WaveScrollView();
            RoundBarrierScrollView();
            DetailsView();
            DeleteSellected();
            EditorGUILayout.EndHorizontal();
            ApplayModified();         
        }     

        private void VerticalSpace()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            EditorGUILayout.EndVertical();
        }

        private void GetDatabase()
        {
            if (_waveDatabase == null)
                _waveDatabase = WaveDatabase.GetDatabase<WaveDatabase>(DATABASE_PATH, WAVE_DATABASE_NAME);
            if (_roundBarrierDatabase == null)
                _roundBarrierDatabase = RoundBarrierDatabase.GetDatabase<RoundBarrierDatabase>(DATABASE_PATH, ROUNDBARRIER_DATABASE_NAME);
            if (_segmentDatabase == null)
                _segmentDatabase = SegmentDatabase.GetDatabase<SegmentDatabase>(DATABASE_PATH, SEGMENT_DATABASE_NAME);
        }

        private void ApplayModified()
        {          
            if (_roundBarrierSerializedObject != null)
                _roundBarrierSerializedObject.ApplyModifiedProperties();
            if (_segmentSerializedObject != null)
                _segmentSerializedObject.ApplyModifiedProperties();
            if (_waveSerializedObject != null)
            {
                _waveSerializedObject.ApplyModifiedProperties();
                _waveSerializedObject.Update();
            }           
            EditorUtility.SetDirty(_waveDatabase);
        }
    }
}