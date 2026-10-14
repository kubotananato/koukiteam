using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-100)] 
public class Input : MonoBehaviour
{
    SaveReplay replay;
    ReplayTimer repT;
    public Dictionary<string, bool> inputs = new Dictionary<string, bool>{
    { "left", false },
    { "right", false },
    { "forward", false },
    { "back", false },
    { "shot", false },
    { "pick", false }
};
    public Vector2 mouseDelta;
    GameObject obj;
    Vector3 init;
    Quaternion initDir;
    public float gameDeltaTime;

    void Start()
    {
        obj = GameObject.Find("ReplayManager");
        replay = obj.GetComponent<SaveReplay>();
        repT = obj.GetComponent<ReplayTimer>();
        init = transform.position;
        initDir = transform.rotation;
    }
    void Update()
    {
        if(repT.isReplay)
        {
            if(repT.frame == 0) 
            {
                // 位置と回転をリセット
                transform.position = init;
                transform.rotation = initDir;
            }
            inputs = replay.GetInputs(repT.frame);
            mouseDelta = replay.GetMouse(repT.frame); 
            gameDeltaTime = replay.GetDeltaTime(repT.frame);
            return;
        }
           inputs = new Dictionary<string, bool>
           {
               {"left", Keyboard.current.aKey.isPressed}, 
                {"right", Keyboard.current.dKey.isPressed}, 
                {"forward", Keyboard.current.wKey.isPressed}, 
                {"back", Keyboard.current.sKey.isPressed},
                {"shot", Mouse.current.leftButton.wasPressedThisFrame},
                {"pick", Mouse.current.leftButton.wasPressedThisFrame}
           };

           mouseDelta = Mouse.current.delta.ReadValue();

           gameDeltaTime = Time.deltaTime;
    }
}
