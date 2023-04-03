using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class FishController : NetworkBehaviour
{
    private Rigidbody2D body;
    [SerializeField] float MoveSpeed = 5;

    [SyncVar] public float FishHealth = 10;



    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }


    public void StartMove()
    {
        body.velocity = transform.up * MoveSpeed;
        StartCoroutine(DisableFish());
    }


    

    IEnumerator DisableFish()
    {
        yield return new WaitForSeconds(30f);
        ServerManager.Despawn(gameObject);
    }
    [ServerRpc(RequireOwnership = false)]
    public void GetDamage(float dmg, int ID, Vector2 playerPos,CanonController canon, Vector2 contactPoint) {
        FishHealth -= dmg;

        if (FishHealth<=0)
        {

            print("Fish killed by : Player " + ID + "\n PlayerPos : " + playerPos);
            canon.SpawnCoins(10, contactPoint, playerPos);
            StopAllCoroutines();
            //GameManager.instance.SpawnCoin(5,transform.position, playerPos);
            ServerManager.Despawn(gameObject);

        }
    }

}
