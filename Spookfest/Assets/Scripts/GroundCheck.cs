using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool touching_ground = true;
    private HashSet<GameObject> ground_objects = new HashSet<GameObject>();
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Ground" && (ground_objects.Count() == 0 || !ground_objects.Contains(collision.gameObject)))
        {
            ground_objects.Add(collision.gameObject);
            touching_ground = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            ground_objects.Remove(collision.gameObject);
            if (ground_objects.Count() == 0)
            {
                touching_ground = false;
            }
        }
    }
}
