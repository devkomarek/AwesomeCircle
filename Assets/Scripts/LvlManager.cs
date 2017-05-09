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

        void Start ()
        {
            _roundBarrierGenerator = GetComponent<RoundBarrierGenerator>();
            _randomizer = GetComponent<Randomizer>();
            _gameInfo = GetComponent<GameInfo>();
        }

        public void SetUpLvl(int lvl)
        {
            _roundBarrierGenerator.Speed = ReturnBarrierSpeedLvl(lvl);
            _randomizer.MinBetweenWavetime = ReturnMinDistanceBeetwenWave(lvl);
            _randomizer.MaxBetweenWavetime = ReturnMaxDistanceBeetwenWave(lvl);
            _randomizer.BetweenRoundBarrierTime = ReturnDistanceBeetwenBarrier(lvl);
        }

        public void SetUpSpeedRoundBarrier(int lvl)
        {
            _roundBarrierGenerator.Speed = ReturnBarrierSpeedLvl(lvl);
        }

        public void FreezAllRoundBarriers()
        {
            _roundBarrierGenerator.Speed = 0;
        }

        public void UnFreezAllRoundBarriers()
        {
            SetUpSpeedRoundBarrier(_gameInfo.Lvl);
        }

        private float ReturnMaxDistanceBeetwenWave(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return Lvl1BeetwenWaveMax;
                case 2:
                    return Lvl2BeetwenWaveMax;
                case 3:
                    return Lvl3BeetwenWaveMax;
                case 4:
                    return Lvl4BeetwenWaveMax;
                case 5:
                    return Lvl5BeetwenWaveMax;
                default:
                    return Lvl1BeetwenWaveMax;
            }
        }

        private float ReturnMinDistanceBeetwenWave(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return Lvl1BeetwenWaveMin;
                case 2:
                    return Lvl2BeetwenWaveMin;
                case 3:
                    return Lvl3BeetwenWaveMin;
                case 4:
                    return Lvl4BeetwenWaveMin;
                case 5:
                    return Lvl5BeetwenWaveMin;
                default:
                    return Lvl1BeetwenWaveMin;
            }
        }

        private float ReturnDistanceBeetwenBarrier(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return Lvl1BeetwenBarrier;
                case 2:
                    return Lvl2BeetwenBarrier;
                case 3:
                    return Lvl3BeetwenBarrier;
                case 4:
                    return Lvl4BeetwenBarrier;
                case 5:
                    return Lvl5BeetwenBarrier;
                default:
                    return Lvl1BeetwenBarrier;
            }
        }

        private float ReturnBarrierSpeedLvl(int lvl)
        {
            switch (lvl)
            {
                case 1:
                    return Lvl1BarrierSpeed;
                case 2:
                    return Lvl2BarrierSpeed;
                case 3:
                    return Lvl3BarrierSpeed;
                case 4:
                    return Lvl4BarrierSpeed;
                case 5:
                    return Lvl5BarrierSpeed;
                default:
                    return Lvl1BarrierSpeed;
            }
        }
    }
}
