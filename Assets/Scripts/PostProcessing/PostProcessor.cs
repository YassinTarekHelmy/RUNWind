using Scripts.InputSystem;
using Scripts.Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using VolFx;

namespace Scripts.PostProcessing {
    public class PostProcessor : MonoBehaviour
    {
        public static PostProcessor Instance { get; private set; }
        [SerializeField] private PlayerFreeze _playerFreeze;
        private Volume _postProcessingVolume;

        ChromaticAberration _chromaticAberration;
        VhsVol _VHSVol;
        Vignette _vignette;

        private bool startEffect = false;
        private void Awake()
        {
            if (Instance != this && Instance != null) {
                Destroy(gameObject);
            } else {
                Instance = this;
            }

            _postProcessingVolume = GetComponent<Volume>();

            if (_postProcessingVolume.profile.TryGet(out _chromaticAberration)) {
                
                _chromaticAberration.active = true;
                
                _chromaticAberration.intensity.value = 0f;
            }
            
            if (_postProcessingVolume.profile.TryGet(out _vignette)) {
                
                _vignette.active = true;
                
                _vignette.intensity.value = 0f;
            }

            if (_postProcessingVolume.profile.TryGet(out _VHSVol)) {
                
                _VHSVol.active = true;
                
                _VHSVol._weight.value = 0f;
            }
        }

        private void Start() {
            InputManager.Instance.OnTimeFreeze += OnTimeFreeze; 
        }

        private void OnTimeFreeze()
        {
            startEffect = !startEffect;
        }

        private void Update() {
            if (startEffect) {
                _chromaticAberration.intensity.value = Mathf.Lerp(_chromaticAberration.intensity.value, 1, Time.unscaledDeltaTime * _playerFreeze.TimeFreezeSpeed);
                
                _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, 0.4f, Time.unscaledDeltaTime * _playerFreeze.TimeFreezeSpeed);
            } else {
                _chromaticAberration.intensity.value = Mathf.Lerp(_chromaticAberration.intensity.value, 0, Time.unscaledDeltaTime * _playerFreeze.TimeFreezeSpeed);
                
                _vignette.intensity.value = Mathf.Lerp(_vignette.intensity.value, 0, Time.unscaledDeltaTime * _playerFreeze.TimeFreezeSpeed);
            }
        }

        public void SetVHSWeight(float weight) {
            _VHSVol._weight.value = weight;
        }
        
        public void SetVHSColor(Color color) {
            _VHSVol._glitch.value = color;
        }
    }
}