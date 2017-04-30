using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text TimeText;
    public float MinusTime;
    public float Timefloat;
    public float Seconds;
//    void Start()
//    {
//        TimeText.text = "Time  00:00";
//    }
    void Update()
    {
        Timefloat = Time.fixedTime - MinusTime;
        int minutes = (int)Timefloat / 60;
        Seconds = (int)Timefloat % 60 + (minutes*60);
        int fraction = (int) (Timefloat * 100f)%100;
        if(Seconds > 9 && fraction > 9)
            TimeText.text = string.Concat("Time  ", Seconds, ":", fraction);
        else if (Seconds < 10 && fraction < 10)
            TimeText.text = string.Concat("Time  ", "0", Seconds, ":", "0", fraction);
        else if (fraction < 10)
            TimeText.text = string.Concat("Time  ", Seconds, ":","0", fraction);
        else if (Seconds < 10)
            TimeText.text = string.Concat("Time  ", "0", Seconds, ":", fraction);
        else
            TimeText.text = string.Concat("Time  ", "0", Seconds, ":", "0", fraction);
    }

    public void TheEnd()
    {
        TimeText.text = "Time 60:00";
        this.enabled = false;
    }

}
