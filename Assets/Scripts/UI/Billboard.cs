using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Transform m_cam;

    void Update()
    {
        transform.LookAt(transform.position + m_cam.forward);
    }
}
