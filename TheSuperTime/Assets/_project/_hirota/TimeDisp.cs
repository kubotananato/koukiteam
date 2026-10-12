using TMPro;
using UnityEngine;

public class TimeDisp : MonoBehaviour
{
    float time;
    float dispTimeMinutes;
    float dispTimeSeconds;
    float dispTimeLessSeconds;
    [SerializeField] private TextMeshProUGUI timeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time = GameManeger.Instance.clearTime;

        dispTimeMinutes = Mathf.FloorToInt(time / 3000);
        dispTimeSeconds = Mathf.FloorToInt((time % 3000) / 50);
        dispTimeLessSeconds = (time % 50) / 50;
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text =dispTimeMinutes.ToString()+":"+dispTimeSeconds.ToString()+ ":" + dispTimeLessSeconds.ToString();
    }
}
