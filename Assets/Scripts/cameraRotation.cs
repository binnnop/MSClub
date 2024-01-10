using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour
{
    public Transform core;  // ���Ķ���
    public Vector3 nowLook;
    public float rotationSpeed = 5f;
    private float mouseX, mouseY;
    public float distance = 5.0f;

    public float zoomSpeed = 5f;
    public float minZoomDistance = 5f;
    public float maxZoomDistance = 20f;

    void Start()
    {
        nowLook = core.transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        // ����Ҽ������
        if (Input.GetMouseButton(1))
        {
            // ��ȡˮƽ��ת����
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float verticalRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
            // �ƺ��Ľ���ˮƽ��ת
            RotateAroundCore(horizontalRotation, verticalRotation);

        }
      



        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        //float newDistance = transform.position.z + scrollWheelInput * zoomSpeed;

       float distanceToCore = Vector3.Distance(transform.position, core.position);

        if (distanceToCore < maxZoomDistance && distanceToCore > minZoomDistance)
        {
            transform.Translate(Vector3.forward * scrollWheelInput * Time.fixedDeltaTime * zoomSpeed);
        }
        else if (distanceToCore > maxZoomDistance && scrollWheelInput > 0)
        {
            transform.Translate(Vector3.forward * scrollWheelInput * Time.fixedDeltaTime * zoomSpeed);
        }
        else if (distanceToCore < minZoomDistance && scrollWheelInput < 0)
        {
            transform.Translate(Vector3.forward * scrollWheelInput * Time.fixedDeltaTime * zoomSpeed);
        }
        


        transform.LookAt(nowLook);
    }

    void RotateAroundCore(float horizontal, float vertical)
    {
        // ������ƺ��Ľ���ˮƽ��ת
        transform.RotateAround(core.position, Vector3.up, horizontal * Time.fixedDeltaTime);
        transform.RotateAround(core.position, Vector3.left, vertical * Time.fixedDeltaTime);
    }

}