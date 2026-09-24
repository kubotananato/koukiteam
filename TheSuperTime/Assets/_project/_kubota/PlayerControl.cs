using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float PmoveSpeed = 5f; // 歩く速度
    /*
    public float RunSpeed = 10f;　// 走る速度

    private float currentSpeed = 0f;
    */
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;


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

        /*
        // Shiftを押したら走る
        if(Keyboard.current.shiftKey.isPressed)
        {
            currentSpeed = RunSpeed;
        }
        else
        {

            currentSpeed = MoveSpeed;
        }
        */

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
        transform.position += move * PmoveSpeed * Time.deltaTime;

        // 移動しているか判定して時間を遅くする
        if(move.magnitude > 0f)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0.05f;
        }

    }
}
