using UnityEngine;
using ExitGames.Client.Photon;

public class ColorSerialization
{
    public static byte[] SerializeColor(object targetObject)
    {
        Color color = (Color)targetObject;

        Quaternion colorToQuaternion = new Quaternion(color.r, color.g, color.b, color.a);
        byte[] bytes = Protocol.Serialize(colorToQuaternion);

        return bytes;
    }

    public static object DeserializeColor(byte[] bytes)
    {
        Quaternion quaternion = (Quaternion)Protocol.Deserialize(bytes);

        Color color = new Color(quaternion.x, quaternion.y, quaternion.z, quaternion.w);

        return color;
    }
}
