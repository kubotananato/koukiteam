using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class ReplayUI : MonoBehaviour
{
    [SerializeField]Material mat;
    Input input;
    int lastReplayVersion;
    [SerializeField] GameObject black;
    [SerializeField] GameObject titlesObj;
    [SerializeField] Sprite[] titles;
    Image titlesImage;
    [SerializeField] int changeFrame = 50;
    RectTransform rect;
    bool start = false;
    int frame = 0;
    int idx = 0;
    float minW = 1980 * 0.3f;
    float minH = 1080 * 0.3f;
    float maxW = 1980;
    float maxH = 1080;
    NextScene nextScene;
    int startFrame = 50;
    PlayerStatus status;
    void Start()
    {
        nextScene = GameObject.Find("ReplayManager").GetComponent<NextScene>();
        status = GameObject.Find("Player").GetComponent<PlayerStatus>();
        input = status.GetComponent<Input>();
        lastReplayVersion = input.replayVersion;
        titlesImage = titlesObj.GetComponent<Image>();
        rect = titlesImage.rectTransform;
        titlesImage.sprite = titles[idx];

        black.SetActive(false);
        titlesObj.SetActive(false);
        mat.SetFloat("_Intensity",1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(lastReplayVersion != input.replayVersion)
        {
            lastReplayVersion = input.replayVersion;
            black.SetActive(true);
            titlesObj.SetActive(true);
            start = true;
            frame = 0;
        }
        if(status.isDead)
        {
            black.SetActive(true);
        }
        if(nextScene.next)
        {
            mat.SetFloat("_Intensity",1.0f);
        }
    }

    void FixedUpdate()
    {
        if(!start)
        {
            frame += 10;
            if(frame < startFrame)
            {
                mat.SetFloat("_Intensity",1.0f);
            }
            else mat.SetFloat("_Intensity",0.0f);
            return;
        } 
        ++frame;
        if(frame >= changeFrame)
        {
            frame = 0;
            idx++;
            if(idx >= titles.Length)
            {
                idx = 0;
            }
            titlesImage.sprite = titles[idx];
        }

        float rate = (float)frame / (float)changeFrame;
        float w = (maxW-minW) * rate+minW;
        float h = (maxH-minH) * rate + minH;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
    }
}
