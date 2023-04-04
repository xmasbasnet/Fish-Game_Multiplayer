using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class FishController : NetworkBehaviour
{
    //private Rigidbody2D body;
    //[SerializeField] float MoveSpeed = 5;
    public int WeightDistribution;
    public int Multiplier;


    public SpawnPattern spawnPattern;

    [Space(30)]
    [SyncVar] public int FishHealth = 10;

    public float speed, SineSpeed, SineAmount;
    float _randomz;
    Vector3 _position;
    Quaternion _Up;

    private void Awake()
    {
        //body = GetComponent<Rigidbody2D>();
    }

    bool move = false;
    public void StartMove()
    {
        _position = transform.position;
        _Up = transform.rotation;
        //body.velocity = transform.up * MoveSpeed;
        StartCoroutine(DisableFish());
        move = true;
    }


    private void Update()
    {
        if (!move)
        {
            return;
        }
        //checkDeath();
        Move(Time.deltaTime);
    }

    private void Move(float deltaTime)
    {
        transform.rotation = _Up;
        transform.RotateAroundLocal(Vector3.forward, Mathf.Deg2Rad * SineAmount * Mathf.Sin((Time.time + _randomz) * SineSpeed));
        _position += transform.up * speed * deltaTime;

        transform.position = _position + new Vector3(0, 0, _randomz);
    }

    IEnumerator DisableFish()
    {
        yield return new WaitForSeconds(25f);
        ServerManager.Despawn(gameObject);
    }
    public int GetDamage(int dmg) {
        FishHealth -= dmg;

        if (FishHealth<=0)
        {

            //print("Fish killed by : Player " + ID + "\n PlayerPos : " + playerPos);
            //canon.SpawnCoins(10, contactPoint, playerPos);
            StopAllCoroutines();
            //GameManager.instance.SpawnCoin(5,transform.position, playerPos);
            gameObject.SetActive(false);
            ServerManager.Despawn(gameObject);

        }
        return FishHealth;
    }

}
