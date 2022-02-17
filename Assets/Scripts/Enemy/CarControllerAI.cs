using System.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerAI : MonoBehaviour
{
    public Transform m_checkpoints;

    private AdvancedArcadeCarController m_controller;
    private Rigidbody m_rigidbody;
    private Vector3 m_targetPos;
    private int m_checkpoint_idx = 0;

    private void Awake()
    {
        m_controller = GetComponent<AdvancedArcadeCarController>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        SetTargetPosition(m_checkpoints.GetChild(m_checkpoint_idx).position);
    }

    void Update()
    {
        Vector3 velocity = m_rigidbody.velocity;
        Vector3 targetDir = m_targetPos - transform.position;
        targetDir.y = 0;

        float acceleration = Vector3.Dot(targetDir.normalized, transform.forward);
        float steerAngle = Vector3.Dot(targetDir.normalized, transform.right);

        if (velocity != Vector3.zero)
        {
            float angle = Vector3.Angle(velocity, transform.forward);

            // break and steer more if the car doesn't move in expected direction
            if (angle > 30)
            {
                acceleration /= 2;
                steerAngle *= 2;
            }
        }

        m_controller.m_input_acceleration = acceleration;
        m_controller.m_input_steer = steerAngle;
    }

    public void SetTargetPosition(Vector3 pos)
    {
        m_targetPos = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Checkpoint")
        {
            return;
        }

        m_checkpoint_idx++;

        if (m_checkpoint_idx >= m_checkpoints.childCount)
        {
            m_checkpoint_idx = 0;
        }

        SetTargetPosition(m_checkpoints.GetChild(m_checkpoint_idx).position);
    }
}
