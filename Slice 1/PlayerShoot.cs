using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

    Lazer[] lazers;

    private bool pressingShoot;

    [SerializeField]
    private bool canShoot;

    [SerializeField]
    private float delay;

    public GameObject projectile;

    // Use this for initialization
    void Start()
    {
        lazers = GetComponentsInChildren<Lazer>();

        canShoot = true;

    }

    // Update is called once per frame
    void Update()
    {
        pressingShoot = Input.GetButton("Fire1");

        if (pressingShoot && canShoot)
        {
            for (int i = 0; i < lazers.Length; i++)
            {
                lazers[i].Shoot(projectile, gameObject);
            }

            canShoot = false;

            StartCoroutine(Delay());

        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }
}
