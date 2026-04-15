using System.Collections;
using UnityEngine;

public sealed class BulletExplosion : MonoBehaviour
{
    [SerializeField] private float timeDestroy = 5f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(timeDestroy);
        Destroy(gameObject);
    }
}