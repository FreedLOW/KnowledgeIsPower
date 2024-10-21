using CodeBase.Data;
using UnityEngine;

namespace CodeBase.Extensions
{
    public static class DataExtension
    {
        public static Vector3Data AsVectorData(this Vector3 position) =>
            new Vector3Data(position.x, position.y, position.z);

        public static Vector3 AsUnityVector(this Vector3Data position) =>
            new Vector3(position.X, position.Y, position.Z);

        public static Vector3 AddY(this Vector3 vector, float y)
        {
            vector.y += y;
            return vector;
        }

        public static string ToJson(this object obj) =>
            JsonUtility.ToJson(obj);
        
        public static T ToDeserialized<T>(this string data) =>
            JsonUtility.FromJson<T>(data);
    }
}