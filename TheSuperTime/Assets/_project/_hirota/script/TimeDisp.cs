using TMPro;
using UnityEngine;

public class TimeDisp : MonoBehaviour
{
    float time;
    int dispTimeMinutes;
    int dispTimeSeconds;
    int dispTimeLessSeconds;
    [SerializeField] private TextMeshProUGUI timeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        time = GameManeger.Instance.clearTime;

        dispTimeMinutes = Mathf.FloorToInt(time / 3000);
        dispTimeSeconds = Mathf.FloorToInt((time % 3000) / 50);
        dispTimeLessSeconds = Mathf.FloorToInt(time % 50);
        timeText.text =$"{dispTimeMinutes}:{dispTimeSeconds:D2}.{dispTimeLessSeconds:D2}";
    }
}
