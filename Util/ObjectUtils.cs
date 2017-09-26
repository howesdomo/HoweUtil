using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.ObjectUtils
{
    public static class ObjectUtilExtension
    {
        public static T DeepCloneByBinary<T>(this T obj)
        {
            using (System.IO.MemoryStream objStream = new System.IO.MemoryStream()) // 使用序列化来实现深拷贝
            {
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(objStream, obj); // 将对象或具有给定根的对象图形序列化为所提供的流。
                objStream.Seek(0, System.IO.SeekOrigin.Begin); // 将当前流中的位置设置为指定值。
                return (T)formatter.Deserialize(objStream);
            }
        }

        public static T DeepCloneByReflection<T>(this T obj)
        {
            if (obj is string || obj.GetType().IsValueType) // 如果是字符串或值类型则直接返回
            {
                return obj;
            }

            object retval = Activator.CreateInstance(obj.GetType());
            System.Reflection.FieldInfo[] fields = obj.GetType().GetFields(System.Reflection.BindingFlags.Public |
                                                                            System.Reflection.BindingFlags.NonPublic |
                                                                            System.Reflection.BindingFlags.Instance |
                                                                            System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCloneByReflection(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }

        public static System.Drawing.Bitmap DeepClone(this System.Drawing.Bitmap source)
        {
            System.Drawing.Bitmap result;
            // 不能随意关闭掉
            System.IO.MemoryStream stream = new System.IO.MemoryStream();

            source.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            result = new System.Drawing.Bitmap(stream);
             
            return result;
        }
    }
}
