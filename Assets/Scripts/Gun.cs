using Assets.Scripts.GameMaster;
using UnityEngine;

namespace Assets.Scripts
{
    public class Gun : MonoBehaviour
    {
        public AudioController AudioController;
        public float EnoughAmmo;
        public float Ammo;
        [SerializeField]private float _minAmmo;
        [SerializeField]private float _powerShot;
        [SerializeField]private bool _isOverloaded;
        [SerializeField]private float _multipleRecovery;
        private GM _gm;
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
            _gm = GameObject.Find("Awesome Circle").transform.Find("Game Master").GetComponent<GM>();
        }
	
        void Update ()
        {
            Ammo += Time.deltaTime * _multipleRecovery;
            CheckAmmo();
            if (IsEmpty())
            {
                _isOverloaded = true;                
                _gm.OverloadEffect();
            }else if (Ammo > EnoughAmmo)
            {
                _isOverloaded = false;
            }
            if(_isOverloaded)
                Ammo += Time.deltaTime * _multipleRecovery*4;
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
            AudioController.OverloadPlay();
            Ammo -= _powerShot;
        }


    }
}
