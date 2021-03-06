﻿using System;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GameManager
    {
        public const string LVL_1 = "Welcome in the Circle";
        public const string LVL_1_HEXAGON = "Welcome in the Hexagon";
        public const string LVL_2 = "Radians or Degrees";
        public const string LVL_2_HEXAGON = "Diagonal?";
        public const string LVL_3 = "Polygon";
        public const string LVL_3_HEXAGON = "Square";
        public const string LVL_4 = "Sinus 360";
        public const string LVL_4_HEXAGON = "Pythagoras";
        public const string LVL_5 = "We are Unstoppable";


        private const string BEST = "BEST_TIME_";
        private const string HEXAGON = "BEST_TIME_HEXAGON";
        private const string END = "END";
        private const string LOCK = "Lock";
        private const string LOCK_2 = "Lock2";
        private const string LOCK_3 = "Lock3";
        private const string LOCK_4 = "Lock4";
        private const string LOCK_5 = "Lock5";

        public static float GetFloatBest(int lvl)
        {
            return ConvertTimeToFloat(PlayerPrefs.GetString(BEST + lvl,"Best 00:00"));
        }

        public static float GetFloatHexagon(int lvl)
        {
            return ConvertTimeToFloat(PlayerPrefs.GetString(HEXAGON + lvl, "Best 00:00"));
        }

        public static string GetTimeBest(int lvl)
        {
            return PlayerPrefs.GetString(BEST + lvl, "Best 00:00");
        }

        public static string GetTimeHexagon(int lvl)
        {
            return PlayerPrefs.GetString(HEXAGON + lvl, "Best 00:00");
        }

        public static void SetBest(int lvl, string v)
        {
            if (v.Substring(0, 4) == "Time")
                v = "Best " + v.Substring(5, v.Length - 5);
            PlayerPrefs.SetString(BEST+lvl, v);
        }

        public static void SetHexagon(int lvl, string v)
        {
            if (v.Substring(0, 4) == "Time")
                v = "Best " + v.Substring(5, v.Length - 5);
            PlayerPrefs.SetString(HEXAGON + lvl, v);
        }

        public static bool IsEnd()
        {
            return PlayerPrefs.GetString(END, "false") == "true";
        }

        public static void TheEnd()
        {
            PlayerPrefs.SetString(END, "true");
        }

        public static string ReturnNameLvl(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return LVL_1;
                case 2:
                    return LvlIsLock(lvl) ? LOCK_2 : LVL_2;
                case 3:
                    return LvlIsLock(lvl) ? LOCK_3 : LVL_3;
                case 4:
                    return LvlIsLock(lvl) ? LOCK_4 : LVL_4;
                case 5:
                    return LvlIsLock(lvl) ? LOCK_5 : LVL_5;
                default:
                    return LOCK;
            }
        }

        public static string ReturnNameLvlHexagon(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return LVL_1_HEXAGON;
                case 2:
                    return LvlIsLockHexagon(lvl) ? LOCK_2 : LVL_2_HEXAGON;
                case 3:
                    return LvlIsLockHexagon(lvl) ? LOCK_3 : LVL_3_HEXAGON;
                case 4:
                    return LvlIsLockHexagon(lvl) ? LOCK_4 : LVL_4_HEXAGON;
                case 5:
                    return LvlIsLockHexagon(lvl) ? LOCK_5 : LVL_5;
                default:
                    return LOCK;
            }
        }

        public static bool IsLvlComplete(int lvl)
        {
            return PlayerPrefs.GetString("Lvl " + lvl, "false") == "true";
        }

        public static bool IsLvlCompleteHexagon(int lvl)
        {
            return PlayerPrefs.GetString("Lvl Hexagon" + lvl, "false") == "true";
        }

        public static bool LvlIsLock(int lvl)
        {
            if((lvl -1 ) == 0) return true;
            return !(GetFloatBest(lvl-1) >= 3600.0f);
        }

        public static bool LvlIsLockHexagon(int lvl)
        {
            if ((lvl - 1) == 0) return true;
            return !(GetFloatHexagon(lvl - 1) >= 3600.0f);
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
