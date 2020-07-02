using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entailing : MonoBehaviour
{
    public Transform Entailed;

    private void Update()
    {
        if (Entailed != null)
        {
            Entailed.position = transform.position;
            Entailed.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }
}
