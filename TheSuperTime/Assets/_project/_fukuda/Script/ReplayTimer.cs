using Unity.VisualScripting;
using UnityEngine;
[DefaultExecutionOrder(99)] 
public class ReplayTimer : MonoBehaviour
{
    SaveReplay sReplay;
    ClearCheck clear;
    public int frame = 0;
    public bool isReplay = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sReplay = GetComponent<SaveReplay>();
        clear = GetComponent<ClearCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if(clear.isClear && !isReplay)
        {
            isReplay = true;
            return;
        }
        if(!isReplay) return;
        Debug.Log("inputCount:"+sReplay.GetInputsCount());
        if (++frame > sReplay.GetInputsCount() - 1)
        {
            Debug.Log("reset");
            frame = 0;
        }
    }
}
