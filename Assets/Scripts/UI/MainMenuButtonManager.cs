using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.PostProcessing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonManager : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _tutorialButton;
    [SerializeField] private Button _quitButton;

    private void Awake() {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _tutorialButton.onClick.AddListener(OnTutorialButtonClicked);
        _quitButton.onClick.AddListener(OnApplicationQuit);

        _playButton.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnTutorialButtonClicked()
    {
        ScreenTransition.Instance.FadeOut(() => {
            //TODO: Tutorial scene will be loaded here
            SceneManager.LoadScene("Level 1");
            ScreenTransition.Instance.FadeIn(null);
        });
    }

    private void OnPlayButtonClicked()
    {
        //TODO: Levels will be loaded here
    }

    private void OnApplicationQuit()
    {
        Application.Quit();
    }
}
