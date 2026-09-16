using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float MoveSpeed = 5f; // 歩く速度
    public float RunSpeed = 10f;　// 走る速度
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        float currentSpeed = MoveSpeed;

        // WASD移動
        if (Keyboard.current.aKey.isPressed)
        {
            horizontal = -1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            horizontal = 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            vertical = -1f;
        }

        if (Keyboard.current.wKey.isPressed)
        {
            vertical = 1f;
        }

        // Shiftを押したら走る
        if(Keyboard.current.shiftKey.isPressed)
        {
            currentSpeed = RunSpeed;
        }

        // Playerの向いている方向を基準にする
        Vector3 move =
            transform.forward * vertical +
            transform.right * horizontal;

        // 斜め移動が速くならないようにする
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }

        // 移動
        transform.position += move * currentSpeed * Time.deltaTime;

    }
}
