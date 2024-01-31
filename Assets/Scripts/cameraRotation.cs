using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotation : MonoBehaviour
{
    public Transform core;  // ���Ķ���
    public Vector3 nowLook;
    public float rotationSpeed = 5f;
    public float panSpeed = 5f;
    public float mouseBuff;
    private float mouseX, mouseY;
    public float distance = 5.0f;

    public float zoomSpeed = 5f;
    public float rotationSmoothTime = 0.2f;
    public float zoomSmoothTime = 0.2f;
    public float panSmoothTime = 0.2f;
    public float minZoomDistance = 5f;
    public float maxZoomDistance = 20f;

    private float targetRotationX;
    private float targetRotationY;
    private float rotationVelocityX;
    private float rotationVelocityY;

    private float targetDistance;
    private float zoomVelocity;

    private Quaternion initialRotation;
    private Vector3 initialPosition;
    private Vector3 panSmoothVelocity;
    public float maxPanDistance = 10f;

    public float zoomSmallFloat;
    public bool isDrag=false;


    void Start()
    {
        distance = Vector3.Distance(transform.position, core.position);
        nowLook = core.transform.position;
        targetDistance = distance;
        initialRotation = transform.rotation;
        initialPosition = core.position;
    }

    void Update()
    {

        HandleZoomInput();
        HandleRotationInput();  
        HandleTranslationInput();
          
       
    }

 

    void HandleRotationInput()
    {
        // ����Ҽ������
        if (Input.GetMouseButton(1))
        {
            // ��ȡˮƽ��ת����
            float horizontalRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            targetRotationY += horizontalRotation;
          

            float smoothRotationY = Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, targetRotationY, ref rotationVelocityY, rotationSmoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
            transform.rotation = Quaternion.Euler(initialRotation.eulerAngles.x, smoothRotationY, 0f);
            core.rotation= Quaternion.Euler(0, smoothRotationY, 0f);

        }
    }

    void HandleTranslationInput()
    {
        // ���������������WASD
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            ProcessTransition();
        }
        else if (Input.GetMouseButton(0)&&!isDrag)
        {
            ProcessTransition();
        }
    }

    void ProcessTransition (){

        float panX=0;
        float panZ= 0;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            panX = Input.GetAxis("Horizontal") * panSpeed;
           panZ = Input.GetAxis("Vertical") * panSpeed;
        }
        else if (Input.GetMouseButton(0))
        {
            panX = Input.GetAxis("Mouse X") * -panSpeed*mouseBuff;
            panZ = Input.GetAxis("Mouse Y") * -panSpeed*mouseBuff;
        }



        //print(panZ + "        " + panX);
        // ��ƽ��֮ǰ�����㵱ǰλ�õ����ĵľ���
        float currentDistance = Vector3.Distance(transform.position, core.position);

        // ����ƽ�ƺ����λ��
        Vector3 newPosition = core.position;
        newPosition += core.transform.right * panX;
        newPosition += core.transform.forward * panZ;

        // ����λ�ý������ƣ�ȷ�����������ƽ�ƾ��� 0
        float newDistance = Vector3.Distance(newPosition, initialPosition);
        if (newDistance <maxPanDistance)
        {
            core.position = Vector3.SmoothDamp(core.position, newPosition, ref panSmoothVelocity, panSmoothTime,Mathf.Infinity, Time.unscaledDeltaTime);


            nowLook = core.position;
            transform.position = core.position - transform.forward * distance;
            transform.LookAt(nowLook);

        }

        // ʹ�� SmoothDamp ƽ���ƶ�����λ��



    }

    void HandleZoomInput()
    {
        float scrollWheelInput=0;
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            print("scroll+1");
            scrollWheelInput = 1;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  
            RaycastHit hit;
            Vector3 targetPosition = core.position;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default")))
            {
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
                targetPosition = new Vector3(hit.point.x, core.position.y, hit.point.z);
                print(hit.transform.gameObject + "      " + targetPosition);
            }
            else
                print("ʲôҲû��");

            // ƽ�� core ��ָ��λ��
            core.position = Vector3.SmoothDamp(core.position, targetPosition, ref panSmoothVelocity, panSmoothTime, Mathf.Infinity, Time.unscaledDeltaTime*2);
            //core.position = targetPosition;

            // ���������λ��
            //distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, zoomSmoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
            //nowLook = core.position;
            // transform.position = core.position - transform.forward * distance;
            //transform.LookAt(nowLook);
            nowLook = core.position;
            transform.position = core.position - transform.forward * distance;
            transform.LookAt(nowLook);
        }


        else if (Input.GetAxisRaw("Mouse ScrollWheel") <0)
        {
            scrollWheelInput = -1;
        }
    


      
        targetDistance -= scrollWheelInput * zoomSpeed;
        //if(scrollWheelInput!=0)
        //print(scrollWheelInput);

        // �������ŷ�Χ
        targetDistance = Mathf.Clamp(targetDistance, minZoomDistance, maxZoomDistance);

        distance = Mathf.SmoothDamp(distance, targetDistance, ref zoomVelocity, zoomSmoothTime,Mathf.Infinity, Time.unscaledDeltaTime*2);

        transform.position = core.position - transform.forward * distance;
        transform.LookAt(nowLook);
    }

  
}





