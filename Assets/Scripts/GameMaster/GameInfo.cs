using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameMaster{
    public class GameInfo : MonoBehaviour{
        [SerializeField]private int _lvl;
        [SerializeField]private string _lvlName;
        [SerializeField]private string _courentTime;

        private Text _timerText;
        void Start()
        {
            _timerText = GameObject.Find("Awesome Circle").transform.FindChild("UI").FindChild("Timer").FindChild("Text").GetComponent<Text>();
        }

        void Update()
        {
            UpdateTime();
        }

        public string CourentTime
        {
            get { return _courentTime; }
            set { _courentTime = value; }
        }

        public string LvlName
        {
            get { return _lvlName; }
            set { _lvlName = value; }
        }

        public int Lvl
        {
            get { return _lvl; }
            set { _lvl = value; }
        }

        public float GetFloatTime()
        {
            return GameManager.ConvertTimeToFloat(_courentTime);
        }

        private void UpdateTime()
        {
            _courentTime = _timerText.text;
        }


    }
}