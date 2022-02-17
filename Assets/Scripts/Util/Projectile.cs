using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    public UnityEvent<Collision> OnCollision = new UnityEvent<Collision>();

    private void OnCollisionEnter(Collision other)
    {
        OnCollision.Invoke(other);
    }

}
