using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyingUpObject : MonoBehaviour
{
    public bool isflyingUp = false;
    public float flyingSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isflyingUp)
        {
            transform.Translate(Vector3.up * flyingSpeed * Time.deltaTime);
            if (transform.position.y > 15)
                Destroy(gameObject);
        }
    }
}
