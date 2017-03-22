using System;
using UnityEngine;

namespace Assets.Scripts.ScriptableObject
{
    [Serializable]
    public class Segment
    {
        [SerializeField] private Color _color;
        [SerializeField] private int _concentration;
        [SerializeField] private float _end;
        [SerializeField] private Material _material;
        [SerializeField] private float _start;
        [SerializeField] private string _tagSegment;
        [SerializeField] private float _width;

        public float Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public float End
        {
            get { return _end; }
            set { _end = value; }
        }

        public string TagSegment
        {
            get { return _tagSegment; }
            set { _tagSegment = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public Material Material
        {
            get { return _material; }
            set { _material = value; }
        }

        public int Concentration
        {
            get { return _concentration; }
            set { _concentration = value; }
        }

        public float Width
        {
            get { return _width; }
            set { _width = value; }
        }
    }
}