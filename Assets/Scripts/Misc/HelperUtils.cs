using UnityEngine;

public static class HelperUtils
{
    public static Vector2 GetProjectileSizeByType(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Star:
                return new Vector2(0.8f, 0.8f);
            case ProjectileType.Diamond:
                return new Vector2(0.4f, 1f);
            default:
                return Vector2.one;
        }
    }
}
