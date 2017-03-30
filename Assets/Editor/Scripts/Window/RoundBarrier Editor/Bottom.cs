using System.Collections.Generic;
using Assets.Editor.Scripts.Popup;
using Assets.Scripts.ScriptableObject;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.Scripts.Window.RoundBarrier_Editor
{
    partial class RoundBarrierEditor
    {
        private void BottomWaveBar()
        {
            if (GUILayout.Button("New"))
            {
                _waveDatabase.Add(new Wave
                {
                    RoundBarriersList = new List<RoundBarrier> {new RoundBarrier {RoundBarrierName = "NoName", SegmentsList = new List<Segment>()}},
                    TimeBetweenRoundBarriers = "0",
                    WaveName = "NoName"
                });
                ApplayModified();
            }
        }

        private void BottomRoundBarrierBar()
        {
            if (GUILayout.Button("New"))
            {
                _waveDatabase.Get(_sellectedWave).RoundBarriersList.Add(new RoundBarrier {RoundBarrierName = "NoName", SegmentsList = new List<Segment>()});
                ApplayModified();
            }
            if (GUILayout.Button("Load"))
            {
                PopupWindow.Show(new Rect(), new LoadRoundBarrier {SellectedWave = _sellectedWave});
            }
        }

        private void BottomSegmentBar()
        {
            if (!GUILayout.Button("New")) return;
            if (_roundBarrierDatabase.Count != 0)
                _roundBarrierDatabase.Get(_sellectedRoundBarrier).SegmentsList.Add(new Segment());
        }
    }
}