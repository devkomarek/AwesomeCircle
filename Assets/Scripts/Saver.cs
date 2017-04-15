using System;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Saver
    {
        public const string LVL_1 = "pierwszy";
        public const string LVL_2 = "drugi";
        public const string LVL_3 = "trzeci";
        public const string LVL_4 = "czwarty";
        public const string LVL_5 = "piaty";
        private const string BEST = "BEST_TIME_";
        private const string LOCK = "Lock";

        public static float GetFloatBest(int lvl)
        {
            return ConvertTimeToFloat(PlayerPrefs.GetString(BEST + lvl,"Best 0:00"));
        }

        public static string GetTimeBest(int lvl)
        {
            return PlayerPrefs.GetString(BEST + lvl, "Best 0:00");
        }

        public static void SetBest(int lvl, string v)
        {
            if (v.Substring(0, 4) == "Time")
                v = "Best " + v.Substring(5, v.Length - 5);
            PlayerPrefs.SetString(BEST+lvl, v);
        }

        public static string ReturnNameLvl(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return LVL_1;
                case 2:
                    return LvlIsLock(lvl) ? LOCK : LVL_2;
                case 3:
                    return LvlIsLock(lvl) ? LOCK : LVL_3;
                case 4:
                    return LvlIsLock(lvl) ? LOCK : LVL_4;
                case 5:
                    return LvlIsLock(lvl) ? LOCK : LVL_5;
                default:
                    return LOCK;
            }
        }

        public static bool LvlIsLock(int lvl)
        {
            if((lvl -1 ) == 0) return true;
            return !(GetFloatBest(lvl-1) >= 3600.0f);
        }

        private static string GetTime(float time)
        {
            int minutes = (int)time / 60;
            int seconds = (int)time % 60 + (minutes * 60);
            int fraction = (int)(time * 1000f) % 100;
            return string.Format("Best  {0:00}:{1:00}", seconds, fraction);
        }

        public static float ConvertTimeToFloat(string time)
        {
            String[] splitStrings = time.Substring(5, time.Length - 5).Split(':');
            return float.Parse(splitStrings[0])*60 + float.Parse(splitStrings[1])/ 1000f;

        }
    }
}
