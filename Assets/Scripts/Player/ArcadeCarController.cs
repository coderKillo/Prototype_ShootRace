using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArcadeCarController : MonoBehaviour
{
    [SerializeField] private float m_speed = 600;
    [SerializeField] private float m_turnSpeed = 60;

    [SerializeField] private Rigidbody m_motorSphere;

    [SerializeField] private float m_drag = 3f;

    private float m_input_acceleration = 0;
    private float m_input_drift = 0;
    private float m_input_steer = 0;

    private void Awake()
    {
    }

    void Start()
    {
        m_motorSphere.transform.parent = null;
    }

    private void Update()
    {
        transform.position = m_motorSphere.transform.position;

        var eulerRotation = Vector3.up;
        eulerRotation *= m_input_steer;
        eulerRotation *= m_input_acceleration + m_input_drift;
        eulerRotation *= m_turnSpeed;
        eulerRotation *= Time.deltaTime;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + eulerRotation);

        m_motorSphere.drag = (1 - m_input_drift) * m_drag;
    }

    private void LateUpdate()
    {
        m_motorSphere.AddForce(transform.forward * m_input_acceleration * (1 - m_input_drift) * m_speed);
    }

    private void OnAccelerate(InputValue value)
    {
        m_input_acceleration = value.Get<float>();
    }

    private void OnDrift(InputValue value)
    {
        m_input_drift = value.Get<float>();
    }

    private void OnSteer(InputValue value)
    {
        m_input_steer = value.Get<float>();
    }
}
