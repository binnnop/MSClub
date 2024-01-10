using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Jsonת��������
/// <para>���JsonUtilityת������ʧ�ܵ�����</para>
/// <para>ZhangYu 2018-05-09</para>
/// </summary>

public static class JsonUtil
{
    /// <summary> �Ѷ���ת��ΪJson�ַ��� </summary>
    /// <param name="obj">����</param>
    public static string ToJson<T>(T obj)
    {
        if (obj == null) return "null";

        if (typeof(T).GetInterface("IList") != null)
        {
            Pack<T> pack = new Pack<T>();
            pack.data = obj;
            string json = JsonUtility.ToJson(pack);
            return json.Substring(8, json.Length - 9);
        }

        return JsonUtility.ToJson(obj);
    }

    /// <summary> ����Json </summary>
    /// <typeparam name="T">����</typeparam>
    /// <param name="json">Json�ַ���</param>
    public static T FromJson<T>(string json)
    {
        if (json == "null" && typeof(T).IsClass) return default(T);

        if (typeof(T).GetInterface("IList") != null)
        {
            json = "{\"data\":{data}}".Replace("{data}", json);
            Pack<T> Pack = JsonUtility.FromJson<Pack<T>>(json);
            return Pack.data;
        }

        return JsonUtility.FromJson<T>(json);
    }

    /// <summary> �ڲ���װ�� </summary>
    private class Pack<T>
    {
        public T data;
    }

}
