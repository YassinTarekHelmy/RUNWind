
using System;
using System.Collections;
using Scripts.InputSystem;
using Scripts.PostProcessing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using VolFx;

namespace Scripts.Player {
    public class PlayerFreeze : MonoBehaviour
    {
        [Header("Time Freeze Settings")]
        [SerializeField] private float _timeFreezeSpeed;
        [SerializeField][Range(0,1)] private float _minimumTimeScale;

        [Header("References")]
        [SerializeField] private Material _timeFreezeMaterial;
        [SerializeField] private Image _rewindImage;
        [SerializeField] private Color _timeFreezeColor;
        [SerializeField] private Color _noTimeFreezeColor;

        private Color _lerpedColor;
        private float _saturation;

        public static bool IsTimeFrozen { get; private set; } = false;
        public float TimeFreezeSpeed { get => _timeFreezeSpeed; }

        public static float customTimeScale = 1f;

        private void Start()
        {
            InputManager.Instance.OnTimeFreeze += OnTimeFreeze;

            _timeFreezeMaterial.SetFloat("_Saturation", 0f);
        }

        private void OnTimeFreeze()
        {
            IsTimeFrozen = !IsTimeFrozen;
        }
        private void Update() {
            if (IsTimeFrozen) 
            {    
                CustomGravity.Instance.TimeScale =  Mathf.Lerp(CustomGravity.Instance.TimeScale,_minimumTimeScale, Time.deltaTime * _timeFreezeSpeed);
                
                _lerpedColor = Color.Lerp(_rewindImage.color, _timeFreezeColor, Time.deltaTime * _timeFreezeSpeed);
                
                _saturation = 0f; 
                
            }
            else 
            {    
                CustomGravity.Instance.TimeScale = Mathf.Lerp(CustomGravity.Instance.TimeScale, 1f, Time.deltaTime * _timeFreezeSpeed);
                
                _lerpedColor = Color.Lerp(_rewindImage.color, _noTimeFreezeColor, Time.deltaTime * _timeFreezeSpeed);

                _saturation = 1f;
            }

            _rewindImage.color = _lerpedColor;
            
            PostProcessor.Instance.SetVHSColor(_lerpedColor);
            
            _timeFreezeMaterial.SetFloat("_Saturation", Mathf.Lerp(_timeFreezeMaterial.GetFloat("_Saturation"), _saturation, Time.deltaTime * _timeFreezeSpeed));
        }
    }
}