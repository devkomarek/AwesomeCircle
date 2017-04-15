using System.Collections;
using System.Diagnostics.CodeAnalysis;
using Assets.Scripts.ScriptableObject.Database;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameMaster{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GM : MonoBehaviour
    {
        private const string LVL_IN_CIRCLE = "LEVEL ";

        public static GM Gm;
        public static WaitForSeconds ShootDuration = new WaitForSeconds(.07f);
        public static float DeltaTime;
        public bool TestMode = false;
        private static bool _gameHasEnded = false;
        public Animator AwesomeCircleAnimator;
        public Animator LightAnimator;

        private AudioController _audioController;
        private Text _lvlText;
        private GameObject _smallCircle1;
        private GameObject _smallCircle2;
        private GameInfo _gameInfo;
        private Randomizer _randomizer;
        private Timer _timer;
        private Text _timerText;
        private GameObject _overloadGun;
        private Hero _hero;
        private GameObject _heroGameObject;
        private Text _best;
        private bool _smallCircle1InTop = true;
        private int _previousState = 0;
        private WaveDatabase _waveDatabase;
        private Image _lockImage;
        private Animator _nextLvlAnimator;
        private Text _nextLvlText;
        private RoundBarrierGenerator _roundBarrierGenerator;
        private void Start()
        {
            GameObject awesomeCircle = GameObject.Find("Awesome Circle");
            GameObject ui = awesomeCircle.transform.FindChild("UI").gameObject;
            _roundBarrierGenerator = GetComponent<RoundBarrierGenerator>();
            _nextLvlAnimator = ui.transform.FindChild("Next_lvl").GetComponent<Animator>();
            _nextLvlText = ui.transform.FindChild("Next_lvl").FindChild("Text").GetComponent<Text>();
            _audioController = awesomeCircle.transform.FindChild("Audio").GetComponent<AudioController>();
            _lvlText = ui.transform.FindChild("Lvl").FindChild("Text").GetComponent<Text>();
            _smallCircle1 = ui.transform.FindChild("Big_circle").transform.FindChild("Small_circle").gameObject;
            _smallCircle2 = ui.transform.FindChild("Big_circle").transform.FindChild("Small_circle (1)").gameObject;
            _timerText = ui.transform.FindChild("Timer").transform.FindChild("Text").gameObject.GetComponent<Text>();
            _gameInfo = GetComponent<GameInfo>();
            _randomizer = GetComponent<Randomizer>();
            _timer = ui.gameObject.GetComponent<Timer>();
            _overloadGun = awesomeCircle.transform.FindChild("Hero").transform.FindChild("Overload Gun").gameObject;
            _hero = awesomeCircle.transform.FindChild("Hero").GetComponent<Hero>();
            _best = ui.gameObject.transform.Find("Best").GetComponentInChildren<Text>();
            _heroGameObject = awesomeCircle.transform.FindChild("Hero").gameObject;         
            _overloadGun.SetActive(false);
            _heroGameObject.SetActive(false);
            _best.text = Saver.GetTimeBest(1);

        }
        private void Update()
        {
            DeltaTime = Time.deltaTime;
        }

        public static void KillRoundBarrier(GameObject rb)
        {
            Destroy(rb.gameObject);
        }

        public void EndGame()
        {
            if(_gameHasEnded)
                return;
            SetActivite(false);
           FreezAllRoundBarriers();
            _audioController.StopLvlPlay();
            
          SetTriggers("EndGame");
            
            string actualTime = _timerText.text;
           if (Saver.GetFloatBest(_gameInfo.Lvl) < Saver.ConvertTimeToFloat(actualTime))
                Saver.SetBest(_gameInfo.Lvl, actualTime);

            _best.text = Saver.GetTimeBest(_gameInfo.Lvl);
            _audioController.StopLvlPlay();
        }

        public void StartGame()
        {
            for (int i = 1; i < 6; i++)
            {
                if (AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + i + " Menu"))
                {
                    StartLvl(i);
                }
            }          
        }

        private void StartLvl(int lvl)
        {
            if(Saver.ReturnNameLvl(lvl) == "Lock") return;
            SetTriggers("StartGame");
            _nextLvlAnimator.SetTrigger("Hide");
            _gameInfo.LvlName = Saver.ReturnNameLvl(lvl);
            
            _audioController.StartLvlPlay();
            _gameInfo.Lvl = lvl;
            _timer.MinusTime = Time.time;
            _randomizer.SetUpWaveDatabase(lvl);
            _lvlText.text = "Lvl " + _gameInfo.LvlName;
            UnFreezAllRoundBarriers();
            _heroGameObject.SetActive(true);
            if(TestMode == false)
            SetActivite(true);

        }

        public void BackToMenu()
        {
          SetTriggers("BackToMenu");
            SetActivite(false);
            _heroGameObject.SetActive(false);
           
            DestroyAllRoundBarriers();
            _smallCircle1.transform.FindChild("lvl_name").GetComponent<Text>().text = Saver.LVL_1;
            _smallCircle1.transform.FindChild("lvl_text").GetComponent<Text>().text = LVL_IN_CIRCLE + "1";
            _smallCircle2.transform.FindChild("lvl_name").GetComponent<Text>().text = Saver.LVL_1;
            _smallCircle2.transform.FindChild("lvl_text").GetComponent<Text>().text = LVL_IN_CIRCLE + "1";
            _best.text = Saver.GetTimeBest(1);
             _smallCircle1InTop = true;
             _previousState = 0;
    }

        public void RestartLvl()
        {
            _audioController.RestartLvlPlay();
            AwesomeCircleAnimator.SetTrigger("Lvl_" + _gameInfo.Lvl);
            SetTriggers("RestartLvl");          

            DestroyAllRoundBarriers();
            _randomizer.SetUpWaveDatabase(_gameInfo.Lvl);
            SetActivite(true);
            _timer.MinusTime = Time.time;
            UnFreezAllRoundBarriers();

        }

        public static IEnumerator ShootEffect(LineRenderer lr)
        {
            lr.enabled = true;
            yield return ShootDuration;
            lr.enabled = false;
        }

        private void SetTriggers(string trigger)
        {
            AwesomeCircleAnimator.SetTrigger(trigger);
            LightAnimator.SetTrigger(trigger);
        }

        private void SetActivite(bool b)
        {
            _randomizer.enabled = b;
            _timer.enabled = b;
            _overloadGun.SetActive(b);
            _gameHasEnded = !b;
            _hero.enabled = b;
        }

        public void OverloadEffect()
        {
            
        }
        
        public void DestroyAllRoundBarriers()
        {
            foreach (var gm in GameObject.FindGameObjectsWithTag("RoundBarrier"))
            {
                Destroy(gm.gameObject);
            }
        }

        public void FreezAllRoundBarriers()
        {
            _roundBarrierGenerator.Speed = 0;
        }

        public void UnFreezAllRoundBarriers()
        {
            _roundBarrierGenerator.Speed = 10;
        }

        private void Swipe(string direction)
        {
            for (int i = 1; i < 6; i++)
            {
                if (AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + i + " Menu") ||
                    AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + i +" Gray") &&
                    _previousState != i)
                {
                    Text lvlName;
                    Text lvlText;                   
                    SwipeToGray(i,direction);                
                    
                    if (i > 1 && direction == "Left" || i < 5 && direction == "Right")
                    {
                        if (_smallCircle1InTop == false)
                        {
                            lvlName = _smallCircle1.transform.FindChild("lvl_name").GetComponent<Text>();
                            lvlText = _smallCircle1.transform.FindChild("lvl_text").GetComponent<Text>();
                            _lockImage = _smallCircle1.transform.FindChild("Lock").GetComponent<Image>();
                            _smallCircle1InTop = true;
                        }
                        else
                        {
                            lvlName = _smallCircle2.transform.FindChild("lvl_name").GetComponent<Text>();
                            lvlText = _smallCircle2.transform.FindChild("lvl_text").GetComponent<Text>();
                            _lockImage = _smallCircle2.transform.FindChild("Lock").GetComponent<Image>();
                            _smallCircle1InTop = false;
                        }

                        if (direction == "Left")
                        {
                            _audioController.SwipeLeftPlay();
                            _gameInfo.LvlName = Saver.ReturnNameLvl(i - 1);
                            _gameInfo.Lvl = i - 1;
                            _nextLvlText.text = ReturnNextLvlAtSeconds(i -1);
                        }

                        if (direction == "Right")
                        {
                            _audioController.SwipeRightPlay();
                            _gameInfo.LvlName = Saver.ReturnNameLvl(i + 1);
                            _gameInfo.Lvl = i + 1;
                            _nextLvlText.text = ReturnNextLvlAtSeconds(i + 1);
                        }
                        lvlName.text = _gameInfo.LvlName;
                        lvlText.text = LVL_IN_CIRCLE + _gameInfo.Lvl;
                        _best.text = Saver.GetTimeBest(_gameInfo.Lvl);
                        TurnImageLock();
                        _previousState = i;

                    }

                    return;
                }
            }
        }

        private void TurnImageLock()
        {
            if (_gameInfo.LvlName == "Lock")
            {
                _lockImage.enabled = true;
            }
            else
            {
                _lockImage.enabled = false;
            }
                
        }

        public void SwipeToRight()
        {
           Swipe("Right");
        }

        public void SwipeToLeft()
        {
            Swipe("Left");

        }

        private void SwipeToGray(int lvl, string direction)
        {
            if (direction == "Right")
            {
                if (Saver.LvlIsLock(lvl + 1))
                    SetTrigerToRightGray();
                else
                {
                    SetTriggers("Swipe" + direction);
                }
            }
            if (direction == "Left")
            {
                if (Saver.LvlIsLock(lvl - 1))
                {
                    switch (lvl - 1)
                    {
                        case 0:
                            SetTriggers("Swipe" + direction);
                            break;
                        case 1:
                            if (AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + lvl + " Gray"))
                                SetTriggerToLvl(lvl);
                            else
                            {
                                SetTriggers("Swipe" + direction);
                            }
                            break;
                        default:
                            SetTrigerToLeftGray();
                            break;
                    }
                  
                }
                else
                {
                    if (AwesomeCircleAnimator.GetCurrentAnimatorStateInfo(0).IsName("Lvl " + lvl + " Gray"))
                        SetTriggerToLvl(lvl);
                    else
                    {
                        SetTriggers("Swipe" + direction);
                    }
                }
            }

        }

        private void SetTriggerToLvl(int lvl)
        {
            SetTriggers("Lvl_" + (lvl - 1)); 
            _audioController.UnloackLvlPlay();
        }

        private void SetTrigerToLeftGray()
        {
            AwesomeCircleAnimator.SetTrigger("LeftGray");
            _audioController.LockLvlPlay();
        }

        private void SetTrigerToRightGray()
        {
            SetTriggers("RightGray");
            _audioController.LockLvlPlay();
        }

        private string ReturnNextLvlAtSeconds(int lvl)
        {
            if (Saver.GetFloatBest(lvl) >= 3600)
            {
                _nextLvlAnimator.SetTrigger("Hide");
                return "Next level at 10 seconds";
            }
            string time = Saver.GetTimeBest(lvl);
            string ss = time.Substring(time.Length - 5,2);
            int s = int.Parse(ss);
            if(s == 0)
                _nextLvlAnimator.SetTrigger("Hide");
            else
                _nextLvlAnimator.SetTrigger("Show");
            return "Next level at " + (60 - s).ToString() + " " + "seconds";
        }
    }
}