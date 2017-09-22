using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class UnityAds : MonoBehaviour
    {
        public AudioController AudioController;
        public UnityAnalytics UnityAnalytics;
        public bool TestMode = false;
        public string AndroidGameID = "1400566";
        public string IOSGameID = "1400567";
        public int MaxAttempt;
        public bool Cancel = false;
        public bool IsRequest = false;
        public int CountTryDisplay;
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
            _creditSmallCircle = GameObject.Find("Awesome Circle").transform.Find("UI").Find("Credit_circle").Find("Credit_small_circle").gameObject;
            _creditSmallCircleNo = GameObject.Find("Awesome Circle").transform.Find("UI").Find("Credit_circle").Find("Credit_small_circle_no").gameObject;
            _creditSmallCircleYes = GameObject.Find("Awesome Circle").transform.Find("UI").Find("Credit_circle").Find("Credit_small_circle_yes").gameObject;
            _text = _creditSmallCircle.transform.Find("Button").Find("Text").GetComponent<Text>();
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

        public bool IsAdShowing = false;
        public void ShowAd()
        {
            CountTryDisplay++;
            if (_enable && CountTryDisplay >= MaxAttempt)
            {
                StartCoroutine(Request());
            }
            if (_enable == false)
                CountTryDisplay = 0;
        }

        public void OnOffAd()
        {

            if (_text.text == "on")
            {
                AudioController.ButtonSmallPlay();
                _creditSmallCircleNo.SetActive(true);
                _creditSmallCircleYes.SetActive(false);
                _creditSmallCircle.SetActive(false);
            }
            if (_text.text == "off")
            {
                AudioController.ButtonSmallPlay();
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
            AudioController.ButtonSmallPlay();
            _creditSmallCircleNo.SetActive(false);
            _creditSmallCircleYes.SetActive(false);
            _creditSmallCircle.SetActive(true);
            _text.text = "on";
        }

        public void Yes()
        {
            AudioController.ButtonSmallPlay();
            _creditSmallCircleNo.SetActive(false);
            _creditSmallCircleYes.SetActive(false);
            _creditSmallCircle.SetActive(true);
            _text.text = "off";
            PlayerPrefs.SetString("Ads", "false");
            _enable = false;
        }

        IEnumerator ShowAdWhennReady()
        {
            while (!Advertisement.IsReady())
                yield return null;
            Advertisement.Show();
            IsAdShowing = false;
        }

        IEnumerator Request()
        {
            Cancel = false;
            bool adsIsReady = false;
            for (int i = 0; i < 20; i++)
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                    break;
                if (Advertisement.IsReady())
                {
                    adsIsReady = true;
                    break;
                }
                yield return new WaitForSeconds(0.05f);
            }
            if (Cancel == false && adsIsReady == true)
            {
                ShowOptions options = new ShowOptions();
                options.resultCallback = AdCallbackhandler;
                Advertisement.Show(options);
            }
            else
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                    CountTryDisplay ++;
                else
                {
                    CountTryDisplay = 0;
                }
                    
            }
      
            Cancel = false;
        }

        void AdCallbackhandler(ShowResult result)
        {
            switch (result)
            {
                case ShowResult.Finished:
                    UnityAnalytics.AdFinished();
                    CountTryDisplay = 0;
                    break;
                case ShowResult.Skipped:
                    UnityAnalytics.AdSkipped();
                    CountTryDisplay = 0;
                    break;
                case ShowResult.Failed:
                   UnityAnalytics.AdFailed();
                    CountTryDisplay = 0;
                    break;
            }
        }


        public
            void SetEnable(bool b)
        {
            _enable = b;
        }
    }
}
