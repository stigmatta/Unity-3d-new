using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class ToasterScript : MonoBehaviour
{
    private static ToasterScript instance;
    private GameObject content;
    private TextMeshProUGUI text;
    private CanvasGroup contentGroup;
    private float timeout;
    private float showtime = 3f;
    private float displayTime;
    private bool isStart;
    private Queue<ToastMessage> messageQueue = new Queue<ToastMessage>();

    void Start()
    {

        instance = this;
        Transform t = this.transform.Find("Content");
        content = t.gameObject;
        contentGroup = t.GetComponent<CanvasGroup>();
        text = t.Find("Text").GetComponent<TextMeshProUGUI>();
        content.SetActive(false);
        timeout = 0f;
        isStart = true;
        GameState.AddListener(OnGameStateChange);
        GameEventSystem.Subscribe(OnGameEvent);
    }

    void Update()
    {
        if (timeout > 0)
        {
            if (isStart)
            {
                contentGroup.alpha = Mathf.MoveTowards(contentGroup.alpha, 1f, Time.deltaTime * 2.5f);
                if (contentGroup.alpha >= 1f)
                {
                    isStart = false;
                    displayTime = showtime;
                }
            }
            else if (displayTime > 0)
            {
                displayTime -= Time.deltaTime;
            }
            else
            {
                contentGroup.alpha = Mathf.MoveTowards(contentGroup.alpha, 0f, Time.deltaTime * 1f);
                if (contentGroup.alpha <= 0f)
                {
                    content.SetActive(false);
                }
            }

            timeout -= Time.deltaTime;
        }
        else if (messageQueue.Count > 0)
        {
            var toast = messageQueue.Dequeue();
            instance.content.SetActive(true);
            instance.text.text = toast.message;
            instance.timeout = (toast.time == 0.0f ? instance.showtime : toast.time) + 2.5f;
            instance.isStart = true;
            contentGroup.alpha = 0f;
        }
    }
    private void OnGameStateChange(string fieldName)
    {
    }

    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.toast is string msg)
        {
            Toast(msg);
        }
    }

    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChange);
        GameEventSystem.Unsubscribe(OnGameEvent);
    }

    public static void Toast(string message, float time = 0.0f)
    {
        instance.messageQueue.Enqueue(new ToastMessage { message = message, time = time > 0.0f ? time : instance.showtime});
    }

    private class ToastMessage
    {
        public string message { get; set; }
        public float time { get; set; }
    }
}
