using UnityEngine;
using UnityEngine.InputSystem;  // 新Input System

/// <summary>
/// 一人称視点のプレイヤー操作（新Input System版）。
/// WASDで移動、マウスで視点の上下左右、Spaceでジャンプ。
/// カメラはPlayerの子として目の高さに配置し、Playerの正面を見る。
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    // 視点の上下回転を行うカメラ（Playerの子オブジェクト）
    [SerializeField] private Transform _cameraTransform;

    // 親（Player）から見たカメラの位置（目の高さ）
    [SerializeField] private Vector3 _eyeLocalPosition = new Vector3(0.0f, 0.6f, 0.0f);

    // 移動速度（m/s）
    [SerializeField] private float _moveSpeed = 5.0f;

    // マウス感度（新Input Systemのマウス移動量はピクセル単位なので小さめにする）
    [SerializeField] private float _mouseSensitivity = 0.1f;

    // 重力加速度（負の値）
    [SerializeField] private float _gravity = -20.0f;

    // ジャンプの高さ（m）
    [SerializeField] private float _jumpHeight = 1.2f;

    // 上下視点の制限角度（度）
    [SerializeField] private float _pitchLimit = 80.0f;

    private CharacterController _characterController;
    private float _verticalVelocity;  // 縦方向（重力・ジャンプ）の速度
    private float _cameraPitch;       // カメラの現在の上下角度

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        // _cameraTransformが未設定なら、MainCameraタグのカメラを使う
        if (_cameraTransform == null)
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("MainCameraタグのカメラが見つかりません。", this);
                enabled = false;  // カメラがないと動かせないので、このスクリプトを停止する
                return;
            }
            _cameraTransform = mainCamera.transform;
        }

        // 診断用: 原因が分かったらこの1行と下のLogCameraDiagnosticsメソッドを削除する
        LogCameraDiagnostics();

        // カメラがPlayerの直接の子でなければ、子にして目の高さに配置する
        if (_cameraTransform.parent != transform)
        {
            _cameraTransform.SetParent(transform);
            _cameraTransform.localPosition = _eyeLocalPosition;
            _cameraTransform.localRotation = Quaternion.identity;
        }

        // マウスカーソルを画面中央に固定して非表示にする
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Look();
        Move();
    }

    /// <summary>マウスの移動量で視点を回転させる。</summary>
    private void Look()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null)
        {
            return;  // マウスが接続されていない場合は何もしない
        }

        Vector2 delta = mouse.delta.ReadValue() * _mouseSensitivity;

        // 左右: Player本体をY軸回転させる（子のカメラも一緒に回る）
        transform.Rotate(0.0f, delta.x, 0.0f);

        // 上下: カメラのみローカルのX軸回転させ、上下の角度を制限する
        _cameraPitch = Mathf.Clamp(_cameraPitch - delta.y, -_pitchLimit, _pitchLimit);
        _cameraTransform.localRotation = Quaternion.Euler(_cameraPitch, 0.0f, 0.0f);
    }

    /// <summary>WASD入力で移動し、重力とジャンプを適用する。</summary>
    private void Move()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;  // キーボードが接続されていない場合は何もしない
        }

        // WASDを -1〜1 の入力値に変換する
        float horizontal = 0.0f;
        float vertical = 0.0f;
        if (keyboard.dKey.isPressed) horizontal += 1.0f;
        if (keyboard.aKey.isPressed) horizontal -= 1.0f;
        if (keyboard.wKey.isPressed) vertical += 1.0f;
        if (keyboard.sKey.isPressed) vertical -= 1.0f;

        // 向いている方向を基準に移動方向を決める（斜めで速くならないよう正規化）
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        move = Vector3.ClampMagnitude(move, 1.0f);

        // 接地中は縦速度を小さな負の値にして地面に吸着させる
        if (_characterController.isGrounded && _verticalVelocity < 0.0f)
        {
            _verticalVelocity = -2.0f;
        }

        // 接地中にSpaceでジャンプ
        if (_characterController.isGrounded && keyboard.spaceKey.wasPressedThisFrame)
        {
            _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
        }

        // 重力を加算
        _verticalVelocity += _gravity * Time.deltaTime;

        // 水平移動と縦移動をまとめて適用
        Vector3 velocity = move * _moveSpeed + Vector3.up * _verticalVelocity;
        _characterController.Move(velocity * Time.deltaTime);
    }

    /// <summary>
    /// 診断用: カメラの親子関係とカメラ数をConsoleに出力する。
    /// 画面に映るカメラと_cameraTransformのカメラが別物なら警告する。
    /// </summary>
    private void LogCameraDiagnostics()
    {
        // このスクリプトが付いているオブジェクトの名前
        Debug.Log("スクリプトが付いているオブジェクト: " + name, this);

        // _cameraTransformに入っているカメラの名前と、その親の名前
        Debug.Log("_cameraTransform: " + _cameraTransform.name + " / 親: "
            + (_cameraTransform.parent != null ? _cameraTransform.parent.name : "なし"), this);

        // 有効なカメラの数と、Camera.main（MainCameraタグ）の名前
        Debug.Log("有効なカメラ数: " + Camera.allCamerasCount + " / Camera.main: "
            + (Camera.main != null ? Camera.main.name : "なし"), this);

        // 画面に映るカメラと、_cameraTransformのカメラが別物なら警告する
        Camera targetCamera = _cameraTransform.GetComponent<Camera>();
        if (targetCamera == null || targetCamera != Camera.main)
        {
            Debug.LogWarning("_cameraTransformのカメラが、Camera.main（画面に映るカメラ）と別物の可能性があります。", this);
        }
    }
}