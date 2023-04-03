using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;


public class CoinSpawner : NetworkBehaviour
{
    public GameObject Coin;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<CoinSpawner>().enabled = false;

        }
    }



    //[ServerRpc]
    public void SpawnCoins(int amount, Vector2 _location, Vector2 playerPos)
    {
        //SevenGame.SoundManager.playSound("Coin", true);
        //if (amount < 50)
        //{
        //    if (amount > 5)
        //        //SevenGame.SoundManager.playSound("WN", true);
        //        StartCoroutine(SpawnAndAnimateCoins(amount, _location, playerPos));
        //}
        //else
        //{
        //    //SevenGame.SoundManager.playSound("WB", true);
        //    StartCoroutine(SpawnAndAnimateCoins(30, _location, playerPos));
        //    //ParticleManager.SpawnParticle("CoinBurst", _location);
        //}

        SpawnCoin(amount, _location, playerPos);

    }


    //[ObserversRpc]
    public void SpawnCoin(int amount, Vector3 _location, Vector2 playerPos) {
        List<Vector3> targets = new List<Vector3>();
        List<GameObject> coins = new List<GameObject>();

        float intervals = 360f / amount;

        for (int i = 0; i < amount; i++)
        {
            targets.Add(_location + (Quaternion.Euler(0, 0, intervals * i) * Vector3.up));
            GameObject go = Instantiate(Coin, _location, Quaternion.identity, GameManager.instance.CoinParent);
            ServerManager.Spawn(go);

            coins.Add(go);
        }

        StartCoroutine(animateCoins(coins, targets, _location, playerPos));
    }

    IEnumerator animateCoins(List<GameObject> coins, List<Vector3> targets, Vector3 _location, Vector2 playerPos) {
        float dur = 0;
        float duration = 0.2f;
        while (dur <= duration)
        {
            float delta = dur / duration;
            dur += Time.deltaTime;
            foreach (var coin in coins)
            {
                int index = coins.IndexOf(coin);
                coin.transform.position = Vector3.Slerp(_location, targets[index], delta * delta);
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        foreach (var coin in coins)
        {
            yield return new WaitForSeconds(0.05f);
            int index = coins.IndexOf(coin);
            StartCoroutine(Returnback(coin, targets[index], playerPos));
        }
    }

    IEnumerator Returnback(GameObject obj, Vector3 from, Vector3 to)
    {
        float dur = 0;
        float duration = 1f;
        while (dur <= duration)
        {
            float delta = dur / duration;
            dur += Time.deltaTime;

            if (obj == null) continue;
            obj.transform.position = Vector3.Lerp(from, to, delta * delta * delta);
            yield return null;
        }
        obj.SetActive(false);
        ServerManager.Despawn(obj);

        //ObjectPool.ReturnObj(obj);
    }

}
