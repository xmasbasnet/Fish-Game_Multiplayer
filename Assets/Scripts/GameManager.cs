using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;


public class GameManager : NetworkBehaviour
{
    public static GameManager instance { set; get; }
    public Transform ProjectileParent;
    Camera Cam;
    CanonController controller;


    private void Awake()
    {
        instance = this;

        Cam = Camera.main;
        Cam.enabled = true;

        //controller = GameObject.FindGameObjectWithTag("Cannon").GetComponent<CanonController>();

        //print(controller.gameObject.name);
    }

  

    //public override void OnStartClient() {
    //    base.OnStartClient();
    //    if (base.IsOwner)
    //    {
            
    //    }

        
    //}

   

}
