using UnityEngine;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour{
        private ParticleSystem _boomEffect;
        private AudioController _audioController;
        public float EnoughAmmo;
        public float Ammo;
        [SerializeField]private float _minAmmo;
        [SerializeField]private float _powerShot;
        [SerializeField]private bool _isOverloaded;
        [SerializeField]private float _multipleRecovery;
        private float _maxAmmo;
        public float MultipleCooling
        {
            get { return _multipleRecovery; }
            set { _multipleRecovery = value; }
        }

        public bool IsOverloaded
        {
            get { return _isOverloaded; }
            set { _isOverloaded = value; }
        }

        public float PowerShot
        {
            get { return _powerShot; }
            set { _powerShot = value; }
        }

        public float MaxOverload
        {
            get { return _minAmmo; }
            set { _minAmmo = value; }
        }

        void Start ()
        {
            _maxAmmo = Ammo;
            _boomEffect = transform.FindChild("Overload Gun").FindChild("BoomEffect").GetComponent<ParticleSystem>();
            _audioController = GameObject.Find("Awesome Circle").transform.FindChild("Audio").GetComponent<AudioController>();
        }
	
        void Update ()
        {
            Ammo += Time.deltaTime * _multipleRecovery;
            CheckAmmo();
            ResizeOverloadGun();
            if (IsEmpty())
            {
                _isOverloaded = true;
                _boomEffect.time = 0;
                _boomEffect.Play();
                _audioController.OverloadPlay();
            }else if (Ammo > EnoughAmmo)
            {
                _isOverloaded = false;
            }
            if(_isOverloaded)
                Ammo += Time.deltaTime * _multipleRecovery*4;
        }

        private void ResizeOverloadGun()
        {
            transform.FindChild("Overload Gun").localScale = new Vector3(Ammo, Ammo, 0.1f);
        }

        private bool IsEmpty()
        {
            return _minAmmo > Ammo;
        }

        private void CheckAmmo()
        {
            if (Ammo > _maxAmmo)
                Ammo = _maxAmmo;
        }

        public void Miss()
        {
            Ammo -= _powerShot;
        }


    }
}
