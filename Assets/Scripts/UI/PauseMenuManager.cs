using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.InputSystem;
using Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private Slider _sensitivitySlider;
    [SerializeField] private TMP_Text _sliderValueText;
    [SerializeField] private Button _mainMenuButton;


    [Header("References")]
    [SerializeField] private PlayerCamera _playerCamera;

    private bool _isPaused = false;

    private void Awake() {
        _sensitivitySlider.value = 10f;

        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnMainMenuButtonClicked()
    {
        ScreenTransition.Instance.FadeOut(() => {
            SceneManager.LoadScene("MainMenu");
            ScreenTransition.Instance.FadeIn(null);
        });
    }

    private void Start() {
        InputManager.Instance.OnPause += TogglePauseMenu;
    }

    private void TogglePauseMenu()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            Time.timeScale = 0;
            InputManager.Instance.DisablePlayerInput();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            InputManager.Instance.EnablePlayerInput();
            _pauseMenu.SetActive(false);
        }
    }

    private void Update()
    {
        _sliderValueText.text = _sensitivitySlider.value.ToString();

        _playerCamera.SensitivityX = _sensitivitySlider.value;
        _playerCamera.SensitivityY = _sensitivitySlider.value;
    }
}
