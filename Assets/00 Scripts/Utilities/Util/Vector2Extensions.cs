using UnityEngine;

public static class Vector2Extensions
{
    /// <summary>
    /// Rotates a Vector2 by a given angle in degrees.
    /// </summary>
    /// <param name="v">The vector to rotate.</param>
    /// <param name="degrees">The angle in degrees to rotate the vector.</param>
    /// <returns>The rotated vector.</returns>
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        float tx = v.x;
        float ty = v.y;

        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }
}
