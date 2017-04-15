using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public Text TimeText;
    public float MinusTime;
    public float Timefloat;


    void Start()
    {
        MinusTime = 0;
    }
    void Update()
    {
        Timefloat = Time.fixedTime - MinusTime;
        int minutes = (int)Timefloat / 60;
        int seconds = (int)Timefloat % 60 + (minutes*60);
        int fraction = (int) (Timefloat * 100f)%100;
        TimeText.text = string.Format("Time  {0:00}:{1:00}",seconds, fraction);
        
       // Debug.Log(Timefloat);
       // Debug.Log(TimeText.text);
    }

}
