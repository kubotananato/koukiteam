using UnityEngine;
using UnityEngine.InputSystem;
[DefaultExecutionOrder(-50)]
public class PlaeyerCamera : MonoBehaviour
{
    public float mouseSens = 2f;
    private float xRotation = 0f;

 //   public GameObject HandReticle;
//    public GameObject WeaponReticle;
    public Input input;
    float initialXRotation;
    int lastReplayVersion;
    private float yRotation;
    private float initialYRotation;
    private int frameToSkip = 2;
    private bool skipFirstFrame = true; // 最初のフレームのマウスの動きを無視

    [SerializeField]Transform player;

    void Start()
    {
        // マウスカーソルを画面に中央に固定
        Cursor.lockState = CursorLockMode.Locked;
        // マウスカーソルを画面上に表示しない
        Cursor.visible = false;
        initialXRotation = 0f;
        xRotation = initialXRotation;
        lastReplayVersion = input.replayVersion;

        initialYRotation = player.eulerAngles.y;
        yRotation = initialYRotation;
    }

    void Update()
    {
        
        // 再生開始時カメラを初期角度に戻す
        if (lastReplayVersion != input.replayVersion)
        {
            xRotation = initialXRotation;
            yRotation = initialYRotation;

            lastReplayVersion = input.replayVersion;
        }       

        if (input.IsReplay)
        {
            // 合算せず、記録の順番で適用
            foreach (Vector2 delta in input.replayMouseSteps)
            {
                ApplyMouseDelta(delta);
            }
        }
        else
        {
            if (frameToSkip > 0)
            {
                frameToSkip--;
            }
            else
            {
                ApplyMouseDelta(input.mouseDelta);
            }
        }
        

        transform.localRotation =
        Quaternion.Euler(xRotation, 0f, 0f);

        player.rotation =
        Quaternion.Euler(0f, yRotation, 0f);
    }

    void ApplyMouseDelta(Vector2 delta)
    {
        delta = Vector2.ClampMagnitude(delta, 50f);

        float mouseX = delta.x * mouseSens;
        float mouseY = delta.y * mouseSens;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Transformから読み直さず、変数へ加算する
        yRotation += mouseX;
    }
}