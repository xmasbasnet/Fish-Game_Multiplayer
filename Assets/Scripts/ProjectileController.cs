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
            HitAFish(collision.transform.GetComponent<FishController>(), collision.ClosestPoint(transform.position));

            body.velocity = Vector2.zero;
            gameObject.SetActive(false);
            
            NetworkObject.Despawn(gameObject);
        }
    }

    IEnumerator DisableProjectile() {
        yield return new WaitForSeconds(2f);
        ServerManager.Despawn(gameObject);
    }

    void HitAFish(FishController f,Vector2 contactPoint) {
        f.GetDamage(1, PlayerID, PlayerPos,canonController, contactPoint);
    }

}
