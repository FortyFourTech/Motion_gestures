using UnityEngine;
using System;
using Dimar.Gestures;

namespace Dimar.Examples
{
    public class BallThrower : MonoBehaviour
    {
        #region Settings
        [Header("Input Settings")]
        public KeyCode holdButton = KeyCode.UpArrow;
        public GestureBase.IDataSource motionSource;
        
        public BallGestureType gestType;

        [SerializeField] private Transform shootingOrigin; // Откуда будут лететь шары
        [SerializeField] private float ballChargeTime = 1f;
        [SerializeField] private float minimalBallTrowSpeed = 0f;
        #endregion

        #region Private fields
        private bool _canThrowBall = true;

        private GestureBase _activationGesture;
        private Coroutine _gestureCoroutine = null;

        private object _chargingBall = null;
        #endregion

        #region Unity functions

        private void Awake()
        {
            if (shootingOrigin == null)
            {
                shootingOrigin = transform;
            }
        }

        private void Start()
        {
            switch (gestType)
            {
                default:
                case BallGestureType.Button:
                    _activationGesture = new JustButton(holdButton);
                    _activationGesture.onStart += _OnGestureStart;
                    _activationGesture.onFire += _OnGestureFire;
                    break;
                case BallGestureType.ButtonThrow:
                    _activationGesture = new ButtonThrow(holdButton, motionSource);
                    _activationGesture.onStart += _OnGestureStart;
                    _activationGesture.onFire += _OnGestureFire;
                    break;
                case BallGestureType.Throw:
                    _activationGesture = new Throw(motionSource, Camera.main.transform);
                    _activationGesture.onStart += _OnGestureStart;
                    _activationGesture.onBrake += _OnGestureFail;
                    _activationGesture.onFire += _OnGestureFire;
                    break;
                    // default:
                    // Debug.LogError("gesture for type not defined");
                    // break;
            }

            StartRead();
        }

        #endregion

        private void _OnGestureFire()
        {
            TryRelease();
        }

        private void _OnGestureStart()
        {
            SpawnBall();
        }

        private void _OnGestureFail()
        {
            DestroyBall();
        }

        public void SpawnBall()
        {
            if (_chargingBall == null)
            {
                _chargingBall = new object();
                // call StartCharge on _chargingBall
                Debug.Log("Ball spawned");
            }
        }

        public void DestroyBall()
        {
            if (_chargingBall != null)
            {
                // call DestroyBall on _chargingBall or simply Destroy(_chargingBall)

                _chargingBall = null;

                Debug.Log("Ball destroyed");
            }
        }

        public void ReleaseBall(Vector3 pos, Quaternion rot)
        {
            // position _chargingBall to pos, rot and call Launch

            _chargingBall = null;
            
            Debug.Log("Ball released");
        }

        public void TryRelease()
        {
            if (_chargingBall != null)
            {
                var minThrowSpeed = minimalBallTrowSpeed;
                var handVelocity = motionSource.Velocity;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
                handVelocity = shootingOrigin.forward * Mathf.Max(minThrowSpeed, 1f);
#endif

                bool shouldThrow;
                Vector3 shootDir;
                switch (gestType)
                {
                    default:
                    case BallGestureType.Button:
                        shouldThrow = true;
                        shootDir = motionSource.Rotation * Vector3.forward;
                        break;
                    case BallGestureType.ButtonThrow:
                        var velocityVal = handVelocity.magnitude;
                        shouldThrow = velocityVal >= minThrowSpeed;//!_chargingBall.IsCharging() && velocityVal >= minThrowSpeed;
#if UNITY_EDITOR
                        shouldThrow = true;
#endif
                        // get direction from max velocity
                        // rotate by current ang velocity
                        shootDir = (Quaternion.Lerp(Quaternion.identity, motionSource.AngVelocity, 0.75f)) * handVelocity.normalized;
                        break;
                    case BallGestureType.Throw:
                        shouldThrow = true;
                        var throwGest = _activationGesture as Throw;
                        shootDir = Quaternion.Inverse(throwGest.MaxAngVelocity) * throwGest.MaxVelocity;//Camera.main.transform.forward;
                        break;
                }

                if (shouldThrow)
                {
                    ReleaseBall(shootingOrigin.position, Quaternion.FromToRotation(Vector3.forward, shootDir));
                }
                else
                {
                    DestroyBall();
                }
            }
        }

        public void StartRead()
        {
            if (!_canThrowBall) return;
            
            if (_gestureCoroutine != null)
            {
                StopCoroutine(_gestureCoroutine);
            }

            if (_activationGesture != null)
            {
                _gestureCoroutine = StartCoroutine(_activationGesture.StartReceiving());
            }
        }

        public void StopRead()
        {
            if (_gestureCoroutine != null)
            {
                StopCoroutine(_gestureCoroutine);
            }
        }
    }
    
    [Serializable]
    public enum BallGestureType
    {
        Button,
        ButtonThrow,
        Throw,
        Keyturn,
        Push,
    }
}
