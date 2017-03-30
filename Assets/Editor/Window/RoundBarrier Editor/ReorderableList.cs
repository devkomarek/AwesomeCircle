using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.Editor.Window.RoundBarrier_Editor
{
    public partial class RoundBarrierEditor
    {
        private void CreateReorderableWaveList()
        {
            _waveSerializedObject = new SerializedObject(_waveDatabase);
            _waveReorderableList = new ReorderableList(_waveSerializedObject, _waveSerializedObject.FindProperty(DATABASE_PROPERTY_NAME), false, false, false, false)
            {
                drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Wave"); },
                onSelectCallback = (ReorderableList list) =>
                {
                    _sellectedWave = list.index;
                    _sellectedRoundBarrier = 0;
                },
                onRemoveCallback = (ReorderableList list) =>
                {
                    _sellectedWave = -1;
                    _sellectedRoundBarrier = -1;
                },
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var element = _waveReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                    EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width*3/4 - 15, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative(WAVE_PROPERTY_NAME).stringValue);
                    if (GUI.Button(new Rect(rect.width*3/4, rect.y, rect.width*1/4, EditorGUIUtility.singleLineHeight), "X"))
                    {
                        _sellectedWaveRemove = index;
                    }
                }
            };
        }

        private void CreateReorderableRoundBarrierList()
        {
            _roundBarrierDatabase.Clear();
            _roundBarrierSerializedObject = new SerializedObject(_roundBarrierDatabase);
            _roundBarrierReorderableList = new ReorderableList(_roundBarrierSerializedObject, _roundBarrierSerializedObject.FindProperty(DATABASE_PROPERTY_NAME), false,
                false, false, false);
            if (_waveDatabase.Count == 0 || _waveDatabase.Get(_sellectedWave).RoundBarriersList == null) return;
            var wave = _waveDatabase.Get(_sellectedWave);
            _roundBarrierDatabase.Clear();
            foreach (var r in wave.RoundBarriersList)
            {
                _roundBarrierDatabase.Add(r);
            }


            _roundBarrierSerializedObject = new SerializedObject(_roundBarrierDatabase);
            _roundBarrierReorderableList = new ReorderableList(_roundBarrierSerializedObject, _roundBarrierSerializedObject.FindProperty(DATABASE_PROPERTY_NAME), false,
                false, false, false)
            {
                drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Round Barrier"); },
                onSelectCallback = (ReorderableList list) => { _sellectedRoundBarrier = list.index; },
                onRemoveCallback = (ReorderableList list) =>
                {
                    _sellectedRoundBarrier = -1;
                    ReorderableList.defaultBehaviours.DoRemoveButton(list);
                },
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var element = _roundBarrierReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight),
                        element.FindPropertyRelative(ROUNDBARRIER_PROPERTY_NAME).stringValue);
                    if (GUI.Button(new Rect(rect.width*3/4, rect.y, rect.width*1/4, EditorGUIUtility.singleLineHeight), "X"))
                    {
                        _sellectedRoundBarrierRemove = index;
                    }
                }
            };
        }

        private void CreateReorderableSegmentList()
        {
            _segmentDatabase.Clear();
            _segmentSerializedObject = new SerializedObject(_segmentDatabase);
            _segmentReorderableList = new ReorderableList(_segmentSerializedObject, _segmentSerializedObject.FindProperty(DATABASE_PROPERTY_NAME), false, false, false,
                false);
            if (_waveDatabase.Count == 0 || _waveDatabase.Get(_sellectedWave).RoundBarriersList == null || _waveDatabase.Get(_sellectedWave).RoundBarriersList.Count == 0) return;
            var roundBarrier = _waveDatabase.Get(_sellectedWave).RoundBarriersList[_sellectedRoundBarrier];

            foreach (var t in roundBarrier.SegmentsList)
            {
                _segmentDatabase.Add(t);
            }
            _segmentSerializedObject = new SerializedObject(_segmentDatabase);
            _segmentReorderableList = new ReorderableList(_segmentSerializedObject, _segmentSerializedObject.FindProperty(DATABASE_PROPERTY_NAME), false, false, false,
                false)
            {
                elementHeight = 60,
                drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Segment"); },
                drawElementCallback = (rect, index, active, focused) =>
                {
                    var element = _segmentReorderableList.serializedProperty.GetArrayElementAtIndex(index);

                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), "Start");
                    element.FindPropertyRelative(SEGMENT_PROPERTY_START).floatValue = EditorGUI.FloatField(
                        new Rect(rect.x + 85, rect.y, 100, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative(SEGMENT_PROPERTY_START).floatValue);

                    rect.y += EditorGUIUtility.singleLineHeight;

                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), "End");
                    element.FindPropertyRelative(SEGMENT_PROPERTY_END).floatValue = EditorGUI.FloatField(
                        new Rect(rect.x + 85, rect.y, 100, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative(SEGMENT_PROPERTY_END).floatValue);

                    rect.y += EditorGUIUtility.singleLineHeight;

                    if (GUI.Button(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Delet"))
                    {
                        _sellectedSegmentRemove = index;
                    }
                }
            };
        }
    }
}