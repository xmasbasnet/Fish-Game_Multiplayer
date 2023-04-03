using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

using FishNet.Component.Animating;
using FishNet.Object.Synchronizing;

public class CanonController : NetworkBehaviour
{



    private Animator anim;
    private NetworkAnimator networkAnim;


    [SerializeField] GameObject Projectile;
    [SerializeField] float ProjectileSpeed = 1;
    Transform ProjectileParent;


    [SyncVar] public float ProjectileDamage = 1;


    Camera Cam;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<CanonController>().enabled = false;

        }
        else {

            Cam = Camera.main;
            Cam.enabled = true;
            //print(Cam.name);

            if (transform.position.y > 0)
            {
                print("Is Up");
                Cam.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else {
                Cam.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
            Vector3 c = Cam.transform.position;
            c.x = transform.position.x;
            Cam.transform.position = c;
        }
    }

    void Awake()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        networkAnim = GetComponent<NetworkAnimator>();
        ProjectileParent = GameManager.instance.ProjectileParent;

    }

    // Update is called once per frame
    void Update()
    {
        if (!base.IsOwner)
        {
            return;
        }

#if UNITY_EDITOR
        EditorControl();
#endif

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        MobileControl();
#endif



    }

    void EditorControl() {
        LookTowardmouse();
        
    }

    void MobileControl() {
        LookTowardsTouch();
    }

    void LookTowardmouse() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Set the z-coordinate to zero to ensure that the object stays on the same plane
        mousePos.z = 0;

        // Calculate the direction from the object to the mouse position
        Vector3 direction = mousePos - transform.position;

        // Calculate the angle between the direction and the y-axis
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90;

        // Rotate the object to face the mouse position on the y-axis
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Input.GetMouseButtonDown(0))
        {
            Shoot(transform.rotation, transform.position + transform.up * 2.25f);

        }
    }

    void LookTowardsTouch() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Player has touched the screen
            Vector3 touchPosition = Input.GetTouch(0).position;
            touchPosition.z = 10f; // Set the z-coordinate to the distance from the camera
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            // Do something with the worldPosition here, such as spawning an object

            Vector3 direction = worldPosition - transform.position;

            // Calculate the angle between the direction and the y-axis
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rot;

            Vector2 BulletPos = transform.position + transform.up * 2.25f;
            // Rotate the object to face the mouse position on the y-axis

            Shoot(rot, BulletPos);
        }
    }

    void Shoot(Quaternion r,Vector2 p) {


        SpawnBullets(r,p);
        SetShoot_anim();

    }

    void SetShoot_anim() {
        networkAnim.SetTrigger("Shoot");
    }




    [ServerRpc]
    void SpawnBullets(Quaternion rot, Vector2 pos) {
        //ProjectileDamage  +=1;
        //LookTowardmouse();
        GameObject go = Instantiate(Projectile,pos , rot, ProjectileParent);


        //go.transform.position = ;
        //go.transform.rotation = ;

        //yield return new WaitForEndOfFrame();
        print(NetworkObject.OwnerId + " Shot");

        ServerManager.Spawn(go);
        go.GetComponent<ProjectileController>().StartShoot(ProjectileSpeed);

    }

    [ServerRpc]
    void NotifyServer() {
        //NetworkObject.Spawn(go);
    }

    //[ServerRpc]
    //public void UpdateDmg() {
    //    ProjectileDamage = 1;
    //}


}
