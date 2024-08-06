using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerManager : MonoBehaviour
{
    [Serializable]
    public struct Message{
        public TriggerEvent _triggerEvent;
        public string _message;
    }

    [Serializable]
    public struct MovingPlatform{
        public TriggerEvent _triggerEvent;
        public GameObject _platform;
    }

    [SerializeField] private List<Message> _messages;
    [SerializeField] private List<MovingPlatform> _movingPlatforms;
    [SerializeField] private List<MovingPlatform> _checkPoints;
    
    [SerializeField] private TriggerEvent _gameEndTrigger;

    [Header("Refereneces")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Respawner _respawner;

    private void Awake()
    {

        foreach (var message in _messages)
        {
            message._triggerEvent.OnPlayerEnter.AddListener(() => MessageLogger.Instance.ShowMessage(message._message));
        }

        foreach (var platform in _movingPlatforms)
        {
            platform._triggerEvent.OnPlayerEnter.AddListener(() => platform._platform.GetComponent<Rigidbody>().isKinematic = false);
        }

        foreach (var checkPoint in _checkPoints)
        {
            checkPoint._triggerEvent.OnPlayerEnter.AddListener(() => _respawner.RespawnableObjects[_player] = checkPoint._platform.transform.position);
        }

        _gameEndTrigger.OnPlayerEnter.AddListener(() => ScreenTransition.Instance.FadeOut(() =>{
            SceneManager.LoadScene("MainMenu");
            ScreenTransition.Instance.FadeIn(null);
         }));
    }

}
