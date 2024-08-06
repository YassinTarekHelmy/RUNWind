using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrapper : MonoBehaviour
{
    private void Start() {
        ScreenTransition.Instance.FadeOut(()=> {
            SceneManager.LoadScene("MainMenu");
            ScreenTransition.Instance.FadeIn(null);
        });
    }
}
