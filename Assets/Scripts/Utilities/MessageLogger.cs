using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MessageLogger : MonoBehaviour
{
    public static MessageLogger Instance { get; private set; }
    [Header("Audio")]
    [SerializeField] private AudioClip _messageSound;
    [SerializeField] private AudioSource __audioSource;
    
    [Header("UI")]
    [SerializeField] private GameObject _messagePanel;
    [SerializeField] private TMP_Text _messaeText;
    [SerializeField] private float _messageDuration = 3f;

    private void Awake() {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

    }

    public void ShowMessage(string message)
    {
        _messaeText.text = message;
        _messagePanel.SetActive(true);
        __audioSource.PlayOneShot(_messageSound);
        StartCoroutine(HideMessage());
    }

    private IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(_messageDuration);
        _messagePanel.SetActive(false);
    }
}
