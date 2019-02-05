using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    public GameObject LogMessagePrefab;
    public RectTransform LogMessageHolder;

    public void LogMessage(string message)
    {
        var messageObject = GameObject.Instantiate(LogMessagePrefab, LogMessageHolder);
        messageObject.transform.localScale = Vector3.one;
        var messageText = messageObject.GetComponentInChildren<Text>();
        messageText.text = message;
        GameObject.Destroy(messageObject, 7.0f);
    }
}
