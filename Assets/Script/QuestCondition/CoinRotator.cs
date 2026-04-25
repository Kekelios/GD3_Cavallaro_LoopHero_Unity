using UnityEngine;

/// <summary>
/// Fait tourner la pièce sur son axe Y, style Mario.
/// Ajouter ce script sur chaque GameObject pièce dans CoinMiniGameScene.
/// </summary>
public class CoinRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
