using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using DG.Tweening;

public class AdvancedArcadeCarController : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float m_maxSpeed = 60f;
    [SerializeField] private float m_speedChangeFactor = 2f;

    [Header("Steering")]
    [SerializeField] private float m_steerSpeed = 100f;
    [SerializeField] private float m_driftSteerFactor = 1f;

    [Header("Tries")]
    [SerializeField] private Transform m_tireFL;
    [SerializeField] private Transform m_tireFR;
    [SerializeField] private Transform m_tireBL;
    [SerializeField] private Transform m_tireBR;

    [Header("Attack")]
    [SerializeField] private Transform m_projectile_vfx;
    [SerializeField] private Transform m_projectile_impact_vfx;
    [SerializeField] private Transform m_projectile_start;
    [SerializeField] private float m_projectile_speed = 100;
    [SerializeField] private int m_projectile_damage = 25;

    [Header("VFX")]
    [SerializeField] private Transform m_smoke;

    [HideInInspector] public float m_input_acceleration = 0;
    [HideInInspector] public float m_input_drift = 0;
    [HideInInspector] public float m_input_steer = 0;

    private Vector3 m_driveDirection;
    private Vector3 m_velocity;

    private Rigidbody m_rigidbody;

    private Transform m_smokeR;
    private Transform m_smokeL;

    private VisualEffect m_smokeR_vfx;
    private VisualEffect m_smokeL_vfx;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        m_smokeL = GameObject.Instantiate(m_smoke, m_tireBL.transform.position, Quaternion.identity, transform);
        m_smokeR = GameObject.Instantiate(m_smoke, m_tireBR.transform.position, Quaternion.identity, transform);

        m_smokeL_vfx = m_smokeL.GetComponent<VisualEffect>();
        m_smokeR_vfx = m_smokeR.GetComponent<VisualEffect>();

        m_smokeL_vfx.Stop();
        m_smokeR_vfx.Stop();
    }

    private void Update()
    {
        HandleMovement();
        SteerTires();
    }

    private void SteerTires()
    {
        var steerAngle = m_input_steer * 20;

        m_tireFL.localRotation = Quaternion.Euler(0, 180 + steerAngle, 0);
        m_tireFR.localRotation = Quaternion.Euler(0, steerAngle, 0);
    }

    private void HandleMovement()
    {
        #region drift
        m_driveDirection = Vector3.Lerp(transform.forward, m_driveDirection, m_input_drift);
        #endregion

        #region move
        m_velocity = Vector3.Lerp(m_velocity, m_driveDirection * m_maxSpeed * m_input_acceleration, m_speedChangeFactor * Time.deltaTime);
        m_velocity.y = m_rigidbody.velocity.y;

        m_rigidbody.velocity = m_velocity;
        #endregion

        #region steering
        var realTurnSpeed = m_input_steer * m_steerSpeed * (1 + m_driftSteerFactor * m_input_drift);
        var rotation = Vector3.up * realTurnSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + rotation);
        #endregion

        #region smoke
        float driftAngle = Vector3.Angle(m_velocity, transform.forward);
        if (driftAngle > 20 && m_velocity.magnitude > 25)
        {
            m_smokeL_vfx.Play();
            m_smokeR_vfx.Play();
        }
        else
        {
            m_smokeL_vfx.Stop();
            m_smokeR_vfx.Stop();
        }

        #endregion
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

    private void OnFire(InputValue value)
    {
        var projectile = GameObject.Instantiate(m_projectile_vfx, m_projectile_start.position, transform.rotation);

        projectile.gameObject.GetComponent<Rigidbody>().velocity = transform.forward * m_projectile_speed;
        projectile.gameObject.GetComponent<Projectile>().OnCollision.AddListener((collision) => OnProjectileCollision(projectile.gameObject, collision));

        Destroy(projectile.gameObject, 3);
    }

    public void OnProjectileCollision(GameObject gameObject, Collision collision)
    {
        var impact = GameObject.Instantiate(m_projectile_impact_vfx, gameObject.transform.position, Quaternion.identity);

        var damageable = collision.gameObject.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.TakeDamage(m_projectile_damage);
        }

        Destroy(gameObject);
        Destroy(impact.gameObject, 2);
    }
}
