using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class ProjectileController : NetworkBehaviour
{

    private Rigidbody2D body;
    int PlayerID;
    Vector2 PlayerPos;
    CanonController canonController;
    //public GameObject Coin;


    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        //StartShoot(30);
    }

    public void StartShoot(float speed, int id, Vector2 pos,CanonController c) {
        PlayerID = id;
        PlayerPos = pos;
        canonController = c;
        body.velocity = transform.up * speed;
        StartCoroutine(DisableProjectile());
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.transform.tag == "Fish")
        {
            print("Hit a FISH");
            
            HitAFish(collision.transform.GetComponent<FishController>(), collision.ClosestPoint(transform.position), collision.transform.position);

            body.velocity = Vector2.zero;
            gameObject.SetActive(false);
            
            NetworkObject.Despawn(gameObject);
        }
    }

    IEnumerator DisableProjectile() {
        yield return new WaitForSeconds(2f);
        ServerManager.Despawn(gameObject);
    }

    void HitAFish(FishController f,Vector2 contactPoint, Vector2 collisionPos) {
        print(NetworkObject.Owner);

        int h =  f.GetDamage(1);
        canonController.SpawnExplosion(contactPoint);
        if (h<=0)
        {
            canonController.SpawnCoins(10, collisionPos, PlayerPos);
        }
    }

    //[ServerRpc]
    //public void SpawnCoin(int amount, Vector3 _location, Vector2 playerPos)
    //{
    //    List<Vector3> targets = new List<Vector3>();
    //    List<GameObject> coins = new List<GameObject>();

    //    float intervals = 360f / amount;

    //    for (int i = 0; i < amount; i++)
    //    {
    //        targets.Add(_location + (Quaternion.Euler(0, 0, intervals * i) * Vector3.up));
    //        GameObject go = Instantiate(Coin, _location, Quaternion.identity, GameManager.instance.CoinParent);
    //        ServerManager.Spawn(go);

    //        coins.Add(go);
    //    }

    //    StartCoroutine(animateCoins(coins, targets, _location, playerPos));
    //}

    //IEnumerator animateCoins(List<GameObject> coins, List<Vector3> targets, Vector3 _location, Vector2 playerPos)
    //{
    //    float dur = 0;
    //    float duration = 0.2f;
    //    while (dur <= duration)
    //    {
    //        float delta = dur / duration;
    //        dur += Time.deltaTime;
    //        foreach (var coin in coins)
    //        {
    //            int index = coins.IndexOf(coin);
    //            coin.transform.position = Vector3.Slerp(_location, targets[index], delta * delta);
    //        }
    //        yield return null;
    //    }
    //    yield return new WaitForSeconds(0.1f);
    //    foreach (var coin in coins)
    //    {
    //        yield return new WaitForSeconds(0.05f);
    //        int index = coins.IndexOf(coin);
    //        StartCoroutine(Returnback(coin, targets[index], playerPos));
    //    }
    //}

    //IEnumerator Returnback(GameObject obj, Vector3 from, Vector3 to)
    //{
    //    float dur = 0;
    //    float duration = 1f;
    //    while (dur <= duration)
    //    {
    //        float delta = dur / duration;
    //        dur += Time.deltaTime;

    //        if (obj == null) continue;
    //        obj.transform.position = Vector3.Lerp(from, to, delta * delta * delta);
    //        yield return null;
    //    }
    //    obj.SetActive(false);
    //    ServerManager.Despawn(obj);

    //    //ObjectPool.ReturnObj(obj);
    //}

}
