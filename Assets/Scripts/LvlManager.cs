using Assets.Scripts.GameMaster;
using UnityEngine;

namespace Assets.Scripts
{
    public class LvlManager : MonoBehaviour
    {
        private RoundBarrierGenerator _roundBarrierGenerator;
        private Randomizer _randomizer;
        private GameInfo _gameInfo;
        public float Lvl1BarrierSpeed = 14;
        public float Lvl2BarrierSpeed = 11;
        public float Lvl3BarrierSpeed = 12;
        public float Lvl4BarrierSpeed = 13;
        public float Lvl5BarrierSpeed = 14;

        public float Lvl1BeetwenBarrier = 0.4f;
        public float Lvl2BeetwenBarrier = 0.3f;
        public float Lvl3BeetwenBarrier = 0.3f;
        public float Lvl4BeetwenBarrier = 0.3f;
        public float Lvl5BeetwenBarrier = 0.3f;

        public float Lvl1BeetwenWaveMin = 0.3f;
        public float Lvl2BeetwenWaveMin = 0.3f;
        public float Lvl3BeetwenWaveMin = 0.3f;
        public float Lvl4BeetwenWaveMin = 0.3f;
        public float Lvl5BeetwenWaveMin = 0.3f;

        public float Lvl1BeetwenWaveMax = 0.8f;
        public float Lvl2BeetwenWaveMax = 1f;
        public float Lvl3BeetwenWaveMax = 1f;
        public float Lvl4BeetwenWaveMax = 1f;
        public float Lvl5BeetwenWaveMax = 1f;

        //
        //

        public float Lvl1HexagonBarrierSpeed = 14;
        public float Lvl2HexagonBarrierSpeed = 11;
        public float Lvl3HexagonBarrierSpeed = 12;
        public float Lvl4HexagonBarrierSpeed = 13;
        public float Lvl5HexagonBarrierSpeed = 14;

        public float Lvl1HexagonBeetwenBarrier = 0.4f;
        public float Lvl2HexagonBeetwenBarrier = 0.3f;
        public float Lvl3HexagonBeetwenBarrier = 0.3f;
        public float Lvl4HexagonBeetwenBarrier = 0.3f;
        public float Lvl5HexagonBeetwenBarrier = 0.3f;

        public float Lvl1HexagonBeetwenWaveMin = 0.3f;
        public float Lvl2HexagonBeetwenWaveMin = 0.3f;
        public float Lvl3HexagonBeetwenWaveMin = 0.3f;
        public float Lvl4HexagonBeetwenWaveMin = 0.3f;
        public float Lvl5HexagonBeetwenWaveMin = 0.3f;

        public float Lvl1HexagonBeetwenWaveMax = 0.8f;
        public float Lvl2HexagonBeetwenWaveMax = 1f;
        public float Lvl3HexagonBeetwenWaveMax = 1f;
        public float Lvl4HexagonBeetwenWaveMax = 1f;
        public float Lvl5HexagonBeetwenWaveMax = 1f;

        void Start ()
        {
            _roundBarrierGenerator = GetComponent<RoundBarrierGenerator>();
            _randomizer = GetComponent<Randomizer>();
            _gameInfo = GetComponent<GameInfo>();
        }

        public void SetUpLvl(int lvl)
        {
            _roundBarrierGenerator.Speed = ReturnBarrierSpeedLvl(lvl);
            _roundBarrierGenerator.BasicsSpeed = ReturnBarrierSpeedLvl(lvl);
            if (PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "true")
            {
                _roundBarrierGenerator.NumCapVertices = 1;
                _roundBarrierGenerator.NumCornerVertices = 6;
            }
            else
            {
                _roundBarrierGenerator.NumCapVertices = 4;
                _roundBarrierGenerator.NumCornerVertices = 0;
            }
            _randomizer.MinBetweenWavetime = ReturnMinDistanceBeetwenWave(lvl);
            _randomizer.MaxBetweenWavetime = ReturnMaxDistanceBeetwenWave(lvl);
            _randomizer.BetweenRoundBarrierTime = ReturnDistanceBeetwenBarrier(lvl);
        }

        public void SetUpSpeedRoundBarrier(int lvl)
        {
            _roundBarrierGenerator.Speed = ReturnBarrierSpeedLvl(lvl);
            _roundBarrierGenerator.BasicsSpeed = ReturnBarrierSpeedLvl(lvl);
        }

        public void FreezAllRoundBarriers()
        {
            _roundBarrierGenerator.Speed = 0;
            _roundBarrierGenerator.BasicsSpeed = 0;
        }

        public void UnFreezAllRoundBarriers()
        {
            SetUpSpeedRoundBarrier(_gameInfo.Lvl);
        }

        private bool ReturnTrueIfAwesomeHexagonActive()
        {
            return PlayerPrefs.GetString("Awesome Hexagon Active", "false") == "true";
        }

        private float ReturnMaxDistanceBeetwenWave(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl1HexagonBeetwenWaveMax : Lvl1BeetwenWaveMax;
                case 2:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl2HexagonBeetwenWaveMax : Lvl2BeetwenWaveMax;
                case 3:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl3HexagonBeetwenWaveMax : Lvl3BeetwenWaveMax;
                case 4:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl4HexagonBeetwenWaveMax : Lvl4BeetwenWaveMax;
                case 5:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl5HexagonBeetwenWaveMax : Lvl5BeetwenWaveMax;
                default:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl1HexagonBeetwenWaveMax : Lvl1BeetwenWaveMax;
            }
        }

        private float ReturnMinDistanceBeetwenWave(int lvl)
        {

            switch (lvl)
            {
                case 1:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl1HexagonBeetwenWaveMin : Lvl1BeetwenWaveMin;
                case 2:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl2HexagonBeetwenWaveMin : Lvl2BeetwenWaveMin;
                case 3:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl3HexagonBeetwenWaveMin : Lvl3BeetwenWaveMin;
                case 4:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl4HexagonBeetwenWaveMin : Lvl4BeetwenWaveMin;
                case 5:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl5HexagonBeetwenWaveMin : Lvl5BeetwenWaveMin;
                default:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl1HexagonBeetwenWaveMin : Lvl1BeetwenWaveMin;
            }
        }

        private float ReturnDistanceBeetwenBarrier(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl1HexagonBeetwenBarrier : Lvl1BeetwenBarrier;
                case 2:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl2HexagonBeetwenBarrier : Lvl2BeetwenBarrier;
                case 3:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl3HexagonBeetwenBarrier : Lvl3BeetwenBarrier;
                case 4:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl4HexagonBeetwenBarrier : Lvl4BeetwenBarrier;
                case 5:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl5HexagonBeetwenBarrier : Lvl5BeetwenBarrier;
                default:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl1HexagonBeetwenBarrier : Lvl1BeetwenBarrier;
            }
        }

        private float ReturnBarrierSpeedLvl(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl1HexagonBarrierSpeed : Lvl1BarrierSpeed;
                case 2:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl2HexagonBarrierSpeed : Lvl2BarrierSpeed;
                case 3:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl3HexagonBarrierSpeed : Lvl3BarrierSpeed;
                case 4:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl4HexagonBarrierSpeed : Lvl4BarrierSpeed;
                case 5:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl5HexagonBarrierSpeed : Lvl5BarrierSpeed;
                default:
                    return ReturnTrueIfAwesomeHexagonActive() ? Lvl1HexagonBarrierSpeed : Lvl1BarrierSpeed;
            }
        }
    }
}
