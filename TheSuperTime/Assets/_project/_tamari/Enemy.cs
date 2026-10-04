using System;
using UnityEngine;

/// <summary>
/// 敵本体。倒されたときに全体へ通知する。
/// SUPERHOT風の「一撃で倒れる」想定のため、体力は持たない。
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>いずれかの敵が倒されたときに呼ばれるイベント。</summary>
    public static event Action<Enemy> Defeated;

    // 二重に倒されるのを防ぐフラグ
    private bool _isDefeated;

    /// <summary>
    /// 敵を倒す。弾やパンチの当たり判定から呼び出す。
    /// </summary>
    public void Defeat()
    {
        if (_isDefeated)
        {
            return;
        }
        _isDefeated = true;

        // StageManagerなどに通知してから、自身を消す
        Defeated?.Invoke(this);
        Destroy(gameObject);
    }
}