using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 With(this Vector3 vector, float x = 0f, float y = 0f, float z = 0f){
        return new Vector3(
            vector.x + x,
            vector.y + y,
            vector.z + z
        );
    }

    public static Vector3 WithX(this Vector3 vector, float amount) {
        vector.x += amount;
        return vector;
    }
    public static Vector3 WithY(this Vector3 vector, float amount) {
        vector.y += amount;
        return vector;
    }
    public static Vector3 WithZ(this Vector3 vector, float amount) {
        vector.z += amount;
        return vector;
    }


}