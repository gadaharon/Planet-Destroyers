using UnityEngine;

public class GameResources : Singleton<GameResources>
{
    public Sprite Planet_1_1 => planet_1_1;
    public Sprite Planet_1_2 => planet_1_2;
    public Sprite Planet_1_3 => planet_1_3;

    public Sprite Planet_2_1 => planet_2_1;
    public Sprite Planet_2_2 => planet_2_2;
    public Sprite Planet_2_3 => planet_2_3;

    [Header("PLANET SPRITES LEVEL 1")]
    [SerializeField] Sprite planet_1_1;
    [SerializeField] Sprite planet_1_2;
    [SerializeField] Sprite planet_1_3;

    [Header("PLANET SPRITES LEVEL 2")]
    [SerializeField] Sprite planet_2_1;
    [SerializeField] Sprite planet_2_2;
    [SerializeField] Sprite planet_2_3;

    [Header("PROJECTILES")]
    [SerializeField] Sprite starProjectile;
    [SerializeField] Sprite diamondProjectile;

    public Sprite GetProjectileSpriteByType(ProjectileType projectileType)
    {
        switch (projectileType)
        {
            case ProjectileType.Star:
                return starProjectile;
            case ProjectileType.Diamond:
                return diamondProjectile;
            default:
                return starProjectile;
        }
    }

}
