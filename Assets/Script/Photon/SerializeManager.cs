using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
public class SerializeManager 
{
    public static string Serialize<T>(T obj)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            serializer.WriteObject(memoryStream, obj);
            return System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
        }
    }

    // XML 문자열을 객체로 역직렬화하는 함수
    public static T Deserialize<T>(string xml)
    {
        using (MemoryStream memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xml)))
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(memoryStream);
        }
    }
}
