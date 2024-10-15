using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarbleFollow : MonoBehaviour
{
    [SerializeField] private Transform marbleTarget;
    [SerializeField] private Transform cameraPoint;

    // Update is called once per frame
    void Update()
    {
        transform.position = marbleTarget.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1);
    }
}
