using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Window.RoundBarrier_Editor
{
    public partial class RoundBarrierEditor
    {
        private void WaveScrollView()
        {
            EditorGUILayout.BeginVertical();
            CreateReorderableWaveList();
            DisplayWave();
            BottomWaveBar();
            EditorGUILayout.EndVertical();
        }

        private void RoundBarrierScrollView()
        {
            EditorGUILayout.BeginVertical("Box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.MinWidth(100), GUILayout.MaxWidth(100));
            CreateReorderableRoundBarrierList();
            DisplayHeadWave();
            DisplayRoundBarriers();
            BottomRoundBarrierBar();
            EditorGUILayout.EndVertical();
        }

        private void DetailsView()
        {
            EditorGUILayout.BeginVertical("Box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.MinWidth(200));
            CreateReorderableSegmentList();
            DisplayHeadRoundBarrier();
            DisplayDetails();
            BottomSegmentBar();
            EditorGUILayout.EndVertical();
        }

        private void DisplayWave()
        {
            _waveScrollVector2 = EditorGUILayout.BeginScrollView(_waveScrollVector2, "Box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.MaxWidth(200), GUILayout.MinWidth(50));
            ApplayModified();
            _waveReorderableList.DoLayoutList();
            EditorGUILayout.EndScrollView();
        }

        private void DisplayRoundBarriers()
        {
            _roundBarrierscrollVector2 = EditorGUILayout.BeginScrollView(_roundBarrierscrollVector2, "Box", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.MaxWidth(350), GUILayout.MinWidth(100));
            ApplayModified();
            _roundBarrierReorderableList.DoLayoutList();
            EditorGUILayout.EndScrollView();
            VerticalSpace();
        }

        private void DisplayDetails()
        {
            _viewScrollVector2 = EditorGUILayout.BeginScrollView(_viewScrollVector2, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true), GUILayout.MinWidth(200), GUILayout.MaxWidth(200));
            ApplayModified();
            _segmentReorderableList.DoLayoutList();
            EditorGUILayout.EndScrollView();
            VerticalSpace();
        }

        private void DisplayHeadRoundBarrier()
        {
            if (_roundBarrierDatabase.Count == 0) return;
            var roundBarrierName = _roundBarrierSerializedObject.FindProperty(DATABASE_PROPERTY_NAME).GetArrayElementAtIndex(_sellectedRoundBarrier).FindPropertyRelative(ROUNDBARRIER_PROPERTY_NAME);
            roundBarrierName.stringValue = EditorGUILayout.TextField("Name", roundBarrierName.stringValue);
           
        }

        private void DisplayHeadWave()
        {
            if (_waveDatabase.Count == 0) return;
            var wave = _waveReorderableList.serializedProperty.GetArrayElementAtIndex(_sellectedWave);
            var waveDataName = wave.FindPropertyRelative(WAVE_PROPERTY_NAME);
            var waveBetweenTime = wave.FindPropertyRelative(ROUNDBARRIER_PROPERTY_BETWEENTIME);

            waveDataName.stringValue = EditorGUILayout.TextField("Name", waveDataName.stringValue, GUILayout.MinWidth(100));
            waveBetweenTime.stringValue = EditorGUILayout.TextField("InterTime", waveBetweenTime.stringValue);

            ApplayModified();
        }
    }
}