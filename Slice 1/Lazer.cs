using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour {

    public void Shoot(GameObject projectile, GameObject ship) {

        RaycastHit hit;

        Vector3 position = gameObject.transform.position + ship.transform.forward * 5;

        GameObject proj = Instantiate(projectile, gameObject.transform.position, ship.transform.rotation);

        Projectile projectileScript = proj.GetComponent<Projectile>();

        projectileScript.ship = ship;

        if (!Physics.Raycast(position, ship.transform.forward, out hit))
        {
            projectileScript.direction = transform.TransformDirection(Vector3.forward);
            return;
        }

        projectileScript.destination = hit.point;
    }
}
