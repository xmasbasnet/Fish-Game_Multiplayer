using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing;
using FishNet.Object;

public class ServerController : NetworkBehaviour
{
    public GameObject Fish;

    [SerializeField] Transform FishParent;

    void Start()
    {
        //if (!base.IsOwner)
        //{
        //    return;
        //}
        if (IsServer)
        {

            
            InvokeRepeating("SpawnFish", 0.0f, 4.0f);
        }
        else
        {

            Destroy(gameObject);

        }



    }

    void SpawnFish() {

        Vector2 t = getRandomDirection() * 30f;
        GameObject go = Instantiate(Fish,t , AngleTowardsPoint(Vector2.zero,t), FishParent);
        base.Spawn(go);
        go.GetComponent<FishController>().StartMove();
    }

    Vector2 getRandomDirection() {
        return Random.insideUnitSphere.normalized;
    }

    Quaternion AngleTowardsPoint(Vector2 point, Vector2 selfPos)
    {
       

       
        Vector2 direction = point - selfPos;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        return Quaternion.AngleAxis(angle, Vector3.forward); ;
       
        
    }

    


}
