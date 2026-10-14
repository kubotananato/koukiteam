using UnityEngine;

/// <summary>
/// 敵のPrefabに付けるコンポーネント。
/// 敵が倒されて破棄されたとき、リスポーン管理に通知する。
/// 再生成先(生成元の固定座標)は、管理側が覚えている。
/// </summary>
public class EnemyRespawnNotifier : MonoBehaviour
{
    // ゲーム終了中かどうか(終了時のOnDestroyでは通知しないため)
    private static bool _isQuitting;

    /// <summary>
    /// Play開始時にフラグを初期化する。
    /// Domain Reloadを無効にしていると、staticな値が前回のPlayから残るため。
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        _isQuitting = false;
    }

    private void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    private void OnDestroy()
    {
        // ゲーム終了時やシーン切り替え時の破棄は、倒されたわけではないので通知しない
        if (_isQuitting || !gameObject.scene.isLoaded)
        {
            return;
        }

        // 管理スクリプトがあれば、この敵の生成元への再生成を依頼する
        if (EnemyRespawnManager.Instance != null)
        {
            EnemyRespawnManager.Instance.RequestRespawn(gameObject);
        }
    }
}