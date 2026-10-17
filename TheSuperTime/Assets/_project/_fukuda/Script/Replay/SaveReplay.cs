using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(100)]
public class SaveReplay : MonoBehaviour
{
    Input input;
    ReplayTimer repT;
    List<Dictionary<string, bool>> inputs = new List<Dictionary<string, bool>>();
    List<Vector2> mouseDeltas = new List<Vector2>();

    public List<float> deltaTimes = new List<float>();

    void Start()
    {
        input = GameObject.Find("Player").GetComponent<Input>();
        repT = GetComponent<ReplayTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(repT.isReplay) return;
        // インプットから入力を受け取り、リストに追加
        inputs.Add(new Dictionary<string,bool>(input.inputs));
        mouseDeltas.Add(input.mouseDelta);
        deltaTimes.Add(Time.deltaTime);
        
    }

    void FixedUpdate()
    {
        
    }

    // ここからゲッター
    public float GetDeltaTime(int _frame)
    {
        return deltaTimes[_frame];
    }

    public Dictionary<string, bool> GetInputs(int _frame)
    {
        return inputs[_frame];
    }

    public int GetInputsCount()
    {
        return inputs.Count;
    }

    public Vector2 GetMouse(int _frame)
    {
        return mouseDeltas[_frame];
    }
}
