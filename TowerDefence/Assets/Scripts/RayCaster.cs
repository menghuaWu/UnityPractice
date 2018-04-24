using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour {

    Ray ray;
    RaycastHit hit;
    Vector3 inDirection;
    
    int lay;
	// Use this for initialization
	void Start () {
        lay = LayerMask.GetMask("wall");
	}
	
	// Update is called once per frame
	void Update () {

        ray = new Ray(transform.position,transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        
        for (int i = 0;i<3;i++)
        {

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 10, lay))
            {
                inDirection = Vector3.Reflect(ray.direction, hit.normal);
                ray = new Ray(hit.point, inDirection);
                Debug.DrawRay(hit.point, inDirection*10, Color.red);
            }
        }
        
	}
}
