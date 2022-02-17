using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    [Header("Motor")]
    [SerializeField] private float m_maxMotorTorque = 400;
    [SerializeField] private float m_maxSteeringAngle = 30;

    [Header("Wheels")]
    [SerializeField] private List<AxleInfo> m_axleInfos;

    [Header("Physics")]
    [SerializeField] private Transform m_centerOfMass;

    private float m_input_acceleration = 0;
    private float m_input_drift = 0;
    private float m_input_steer = 0;

    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_rigidbody.centerOfMass = m_centerOfMass.localPosition;
    }

    private void Update()
    {
        var motor = m_maxMotorTorque * m_input_acceleration;
        var steer = m_maxSteeringAngle * m_input_steer;

        foreach (var axleInfo in m_axleInfos)
        {
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steer;
                axleInfo.rightWheel.steerAngle = steer;
            }

            var friction = axleInfo.leftWheel.forwardFriction;

            // friction.asymptoteValue = (1 - m_input_drift) * 1;
            // friction.extremumValue = (1 - m_input_drift) * 2;

            // axleInfo.leftWheel.forwardFriction = friction;
            // axleInfo.rightWheel.sidewaysFriction = friction;
        }
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
