namespace Assets.Editor.Scripts.Window.RoundBarrier_Editor
{
    public partial class RoundBarrierEditor
    {
        private void DeleteSellected()
        {
            if (_sellectedWaveRemove != -1)
            {
                _waveDatabase.Remove(_sellectedWaveRemove);
                if (_sellectedWaveRemove == _sellectedWave)
                {
                    _sellectedWave = 0;
                    _sellectedRoundBarrier = 0;
                }
                ApplayModified();
                _sellectedWaveRemove = -1;
            }
            if (_sellectedRoundBarrierRemove != -1)
            {
                _waveDatabase.Get(_sellectedWave).RoundBarriersList.RemoveAt(_sellectedRoundBarrierRemove);
                if (_sellectedRoundBarrier == _sellectedRoundBarrierRemove)
                {
                    _sellectedRoundBarrier = 0;
                }
                ApplayModified();
                _sellectedRoundBarrierRemove = -1;
            }
            if (_sellectedSegmentRemove != -1)
            {
                _waveDatabase.Get(_sellectedWave).RoundBarriersList[_sellectedRoundBarrier].SegmentsList.RemoveAt(_sellectedSegmentRemove);
                ApplayModified();
                _sellectedSegmentRemove = -1;
            }
        }
    }
}