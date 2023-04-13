using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance { set; get; }
    public Transform ProjectileParent;
    public Transform CoinParent;
    public Transform ExplosionParent;
    Camera Cam;
    CanonController controller;

    public bool AutoShoot = false;
    public bool Lock = false;

    private void Awake()
    {
        instance = this;

        Cam = Camera.main;
        Cam.enabled = true;

       
    }

    public void PressedAutoFire() {
        AutoShoot = !AutoShoot;
        updateAutoandLock();
    }

    public void PressedLock()
    {
        Lock = !Lock;
        updateAutoandLock();
    }

    public void PressedWeapon() {

    }

    public void PressedSpecies()
    {

    }

    void updateAutoandLock() {
        controller.SetAutoOrLock(AutoShoot, Lock);
    }

    public void SetCannon(CanonController c) {
        controller = c;
    }




}
