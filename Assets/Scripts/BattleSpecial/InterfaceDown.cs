using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceDown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Descent()
    {
        print("����½�");
        float descentDistance = 4f;
        Vector3 targetPosition = transform.position - new Vector3(0f, descentDistance, 0f);
        float descentSpeed =12;


        while (transform.position.y > targetPosition.y)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * descentSpeed);
            print("�����½�����������������������������������������������������������������������������");
            yield return null;
        }

    }
}
