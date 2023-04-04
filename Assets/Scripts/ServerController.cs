using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Managing;
using FishNet.Object;

public class ServerController : NetworkBehaviour
{
    //public GameObject Fish;

    [SerializeField] Transform FishParent;
    [SerializeField]float radius, rate;

    float amt = 0;

    GameObject[] Fishes;
    public FishData fishData;

    void Start()
    {
        //if (!base.IsOwner)
        //{
        //    return;
        //}
        if (!IsServer)
        {

            Destroy(gameObject);

        }
        else {
            Fishes = fishData.getFishes();
        }
        



    }

    private void Update()
    {
        if (!IsServer)
        {


            return;
        }

        amt += Time.deltaTime * rate;
        while (amt > 0)
        {
            amt--;

            print("Spawned");
            //SpawnRandom();
            //print(fishData.GetRandomFishIndex());

            SpawnFish(fishData.GetRandomFishIndex());
        }
    }

    void SpawnFish(int index) {

        Vector3 point = getRandomPoint();

        FishController FC = Fishes[index].GetComponent<FishController>();

        switch (FC.spawnPattern.Type)
        {
            case SpawnPattern.Pattern.single:
                GameObject go = Instantiate(Fishes[index], point, Quaternion.identity, FishParent);
                go.transform.up = getUpDir(point);
                base.Spawn(go);
                go.transform.GetComponent<FishController>().StartMove();

                break;

            case SpawnPattern.Pattern.school:
                int count = Random.Range(FC.spawnPattern.mincount, FC.spawnPattern.maxcount + 1);

                for (int i = 0; i < count; i++)
                {
                    Vector3 newpoint = (Vector2)point + (RandomDir() * Random.value * FC.spawnPattern.spread_radius);
                    GameObject go1 = Instantiate(Fishes[index], newpoint, Quaternion.identity, FishParent);
                    go1.transform.up = getUpDir(point);
                    base.Spawn(go1);
                    go1.transform.GetComponent<FishController>().StartMove();
                }

                break;

            case SpawnPattern.Pattern.stream:

                StartCoroutine(streamSpawn(FC, index, point, getUpDir(point)));


                break;

            
        }

        
    }

    IEnumerator streamSpawn(FishController fish,int index, Vector3 position, Vector3 direction)
    {
        float dur;
        int count = 0;
        SpawnPattern pattern = fish.spawnPattern;

        if (fish.spawnPattern.duration > 0)
        {
            dur = pattern.duration;
            count = (int)(dur * pattern.rate);
        }
        else
        {
            count = Random.Range(pattern.mincount, pattern.maxcount + 1);
        }

        float amt = 0;
        while (count > 0)
        {
            amt += fish.spawnPattern.rate * Time.deltaTime;
            if (amt > 1)
            {
                amt--;
                count--;

                GameObject go = Instantiate(Fishes[index], position, Quaternion.identity, FishParent);
                go.transform.up = direction;
                base.Spawn(go);
                go.transform.GetComponent<FishController>().StartMove();
            }
            yield return null;
        }
    }
    //Vector2 getRandomDirection() {
    //    return Random.insideUnitSphere.normalized;
    //}

    //Quaternion AngleTowardsPoint(Vector2 point, Vector2 selfPos)
    //{



    //    Vector2 direction = point - selfPos;

    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

    //    return Quaternion.AngleAxis(angle, Vector3.forward); ;


    //}

    Vector3 getRandomPoint()
    {
        float angle = 0;
        Vector3 random = Vector3.zero;
        while (angle < 20)
        {
            random = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            angle = Vector3.Angle(Vector3.down, random);
        }
        return random * radius;
    }

    Vector3 getUpDir(Vector3 pos)
    {

        return ((transform.position + (Random.Range(-1f, 1f) * transform.right * 10)) - pos).normalized;
    }

    Vector2 RandomDir()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
