// using System.Collections;
// using Game.Interfaces;
// using UnityEngine;
//
// public sealed class BulletExplosion : MonoBehaviour
// {
//     private float timeReturn;
//     private IPool<BulletExplosion> pool;
//     
//     public void SetData(IPool<BulletExplosion> pool, float time)
//     {
//         this.pool = pool;
//         this.timeReturn = time;
//         StartCoroutine(ReturnTimer());
//     }
//
//     private IEnumerator ReturnTimer()
//     {
//         yield return new WaitForSeconds(timeReturn);
//         pool.Return(this);
//     }
// }
