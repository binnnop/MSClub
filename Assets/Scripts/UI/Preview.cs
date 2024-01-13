using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    public GameObject objectToGenerate; // 指定要生成的物体
    public int numberOfObjectsToGenerate = 5; // 指定要生成的物体数量
    public float spawnInterval = 2f; // 指定生成物体的时间间隔（秒）

    private int generatedObjectsCount = 0;
    private float timer = 0f;

    public Transform[] pathPoints;


    void Update()
    {
        // 如果已经生成了足够数量的物体，则停止生成
        if (generatedObjectsCount >= numberOfObjectsToGenerate)
        {
            Debug.Log("已生成足够数量的物体，停止生成。");
            return;
        }

        // 更新计时器
        timer += Time.deltaTime;

        // 如果计时器超过生成间隔，生成物体并重置计时器
        if (timer >= spawnInterval)
        {
            GenerateObject();
            timer = 0f;
        }
    }
    private void GenerateObject()
    {
        // 生成物体
        GameObject line=Instantiate(objectToGenerate, transform.position, Quaternion.identity);
        line.GetComponent<PreviewPath>().pathPoints=pathPoints;
        // 增加生成的物体数量
        generatedObjectsCount++;

        Debug.Log("生成物体 #" + generatedObjectsCount);
    }
}
