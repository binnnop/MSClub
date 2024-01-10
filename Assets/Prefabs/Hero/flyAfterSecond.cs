using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyAfterSecond : MonoBehaviour
{
    public float waitSecond;
    public float count = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        count += Time.deltaTime;
        if (count > waitSecond)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 3f);
            if (transform.position.y > 30)
            {

                Destroy(gameObject);

            }
        
        }
    }
}
