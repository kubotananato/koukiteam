using UnityEngine;

/// <summary>
/// 敵のPrefabに付けるコンポーネント。
/// 敵が倒されて破棄されたとき、リスポーン管理に通知する。
/// 生成された位置と向きを覚えておき、そこへ再生成できるようにする。
/// </summary>
public class EnemyRespawnNotifier : MonoBehaviour
{
    // 生成時の位置と向き(リスポーン先に使う)
    private Vector3 _spawnPosition;
    private Quaternion _spawnRotation;

    // ゲーム終了中かどうか(終了時のOnDestroyでは通知しないため)
    private static bool _isQuitting = false;

    private void Awake()
    {
        // 生成された瞬間の位置を記録する
        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation;
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

        // 管理スクリプトがあれば、リスポーンを依頼する
        if (EnemyRespawnManager.Instance != null)
        {
            EnemyRespawnManager.Instance.RequestRespawn(_spawnPosition, _spawnRotation);
        }
    }
}