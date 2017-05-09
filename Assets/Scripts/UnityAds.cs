using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UnityAds : MonoBehaviour
    {
        public bool TestMode = false;
        public string AndroidGameID = "1400566";
        public string IOSGameID = "1400567";
        public int MaxAttempt;
        public bool Cancel = false;
        private int _countTryDisplay;
        private bool _enable = true;
        private GameObject _creditSmallCircle;
        private GameObject _creditSmallCircleNo;
        private GameObject _creditSmallCircleYes;
        private Text _text;

        private void Awake()
        {
            if (!Advertisement.isInitialized)
            {
                Advertisement.Initialize(AndroidGameID, TestMode);
            }
        }

        private void Start()
        {
            _creditSmallCircle = GameObject.Find("Awesome Circle").transform.FindChild("UI").FindChild("Credit_circle").FindChild("Credit_small_circle").gameObject;
            _creditSmallCircleNo = GameObject.Find("Awesome Circle").transform.FindChild("UI").FindChild("Credit_circle").FindChild("Credit_small_circle_no").gameObject;
            _creditSmallCircleYes = GameObject.Find("Awesome Circle").transform.FindChild("UI").FindChild("Credit_circle").FindChild("Credit_small_circle_yes").gameObject;
            _text = _creditSmallCircle.transform.FindChild("Button").FindChild("Text").GetComponent<Text>();
            if (PlayerPrefs.GetString("Ads","true") == "true")
            {
                _text.text = "on";
                _enable = true;
            }
            else
            {
                _text.text = "off";
                _enable = false;
            }
        }

        public void ShowAd()
        {
            _countTryDisplay++;
            if (_enable && _countTryDisplay >= MaxAttempt)
            {
               StartCoroutine(Request());
            }
        }

        public void OnOfAd()
        {

            if (_text.text == "on")
            {
                _creditSmallCircleNo.SetActive(true);
                _creditSmallCircleYes.SetActive(false);
                _creditSmallCircle.SetActive(false);
            }
            if (_text.text == "off")
            {
                _creditSmallCircleNo.SetActive(false);
                _creditSmallCircleYes.SetActive(true);
                _creditSmallCircle.SetActive(false);
                StartCoroutine(Wait());
                _text.text = "on";
            }
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1.5f);
            _creditSmallCircleNo.SetActive(false);
            _creditSmallCircleYes.SetActive(false);
            _creditSmallCircle.SetActive(true);
            _enable = true;
            PlayerPrefs.SetString("Ads", "true");
        }

        public void No()
        {
            _creditSmallCircleNo.SetActive(false);
            _creditSmallCircleYes.SetActive(false);
            _creditSmallCircle.SetActive(true);
            _text.text = "on";
        }

        public void Yes()
        {
            _creditSmallCircleNo.SetActive(false);
            _creditSmallCircleYes.SetActive(false);
            _creditSmallCircle.SetActive(true);
            _text.text = "off";
            PlayerPrefs.SetString("Ads", "false");
            _enable = false;
        }

        IEnumerator Request()
        {
            Cancel = false;
            bool adsIsReady = false;
            for (int i = 0; i < 100; i++)
            {
                if (Advertisement.IsReady())
                {
                    adsIsReady = true;
                    break;
                }
                yield return new WaitForSeconds(0.05f);
            }
            if (Cancel == false && adsIsReady == true)
            {
                Advertisement.Show();
                _countTryDisplay = 0;
            }
            Cancel = false;
        }

        public void SetEnable(bool b)
        {
            _enable = b;
        }
    }
}
