using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class ReplayUI : MonoBehaviour
{
    ReplayTimer repT;
    [SerializeField] GameObject black;
    [SerializeField] GameObject titlesObj;
    [SerializeField] Sprite[] titles;
    Image titlesImage;
    [SerializeField] int changeFrame = 50;
    RectTransform rect;
    bool start = false;
    int frame = 0;
    int idx = 0;
    float minW = 1980 * 0.1f;
    float minH = 1080 * 0.1f;
    float maxW = 1980;
    float maxH = 1080;
    void Start()
    {
        repT = GameObject.Find("ReplayManager").GetComponent<ReplayTimer>();
        titlesImage = titlesObj.GetComponent<Image>();
        rect = titlesImage.rectTransform;
        titlesImage.sprite = titles[idx];

        black.SetActive(false);
        titlesObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(repT.IsStartReplay())
        {
            black.SetActive(true);
            titlesObj.SetActive(true);
            start = true;
        }

    }

    void FixedUpdate()
    {
        if(!start) return;
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
