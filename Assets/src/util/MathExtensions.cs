using UnityEngine;

public static class MathExtensions {
    /// <summary>
    /// Obtains a random point given the bounds
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        // 0.5f is used so we can get randoms in both signs. Gets balanced because size of bounds is double the value
        // of extents.
        return bounds.center + new Vector3(
            (Random.value - 0.5f) * bounds.size.x,
            (Random.value - 0.5f) * bounds.size.y,
            (Random.value - 0.5f) * bounds.size.z
        );
    }
}
