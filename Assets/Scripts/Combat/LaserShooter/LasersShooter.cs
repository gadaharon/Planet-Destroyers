using System.Collections.Generic;
using UnityEngine;

public class LasersShooter : MonoBehaviour, ISpecialAttack
{
    [SerializeField] GameObject laserCanonPrefab;
    [SerializeField] List<Transform> canonsPositions;
    [SerializeField] bool laserOn = false;
    [SerializeField] float shootingTime = 5f;
    [SerializeField] string targetLayerName;
    [SerializeField] string damageDealerTag;

    List<LaserCanon> laserCanons = new List<LaserCanon>();
    Transform lasersParent;

    void Update()
    {
        if (laserOn)
        {
            UpdateLaserCanons();
        }
    }

    void Start()
    {
        lasersParent = GameObject.Find("Lasers").transform;
        if (lasersParent == null) { lasersParent = transform; }
    }

    public void FireSpecialAttack()
    {
        if (laserOn) { return; }

        if (laserCanons.Count == 0)
        {
            GenerateLaserCanons(targetLayerName, damageDealerTag);
        }
        laserOn = true;
        ToggleLasers(true);

        Invoke(nameof(TurnLaserOff), shootingTime);
    }

    void ToggleLasers(bool isActive)
    {
        foreach (LaserCanon canon in laserCanons)
        {
            canon.gameObject.SetActive(isActive);
        }
    }

    void TurnLaserOff()
    {
        laserOn = false;
        ToggleLasers(false);
    }

    void GenerateLaserCanons(string targetLayerName, string damageDealerTag)
    {
        for (int i = 0; i < canonsPositions.Count; i++)
        {
            Quaternion rotation;
            rotation = Quaternion.Euler(0, 0, 180);
            GameObject laserGO = Instantiate(laserCanonPrefab, canonsPositions[i].position, rotation, lasersParent);
            laserGO.GetComponent<LaserCanon>().Init(targetLayerName, damageDealerTag, canonsPositions[i]);
            LaserCanon laser = laserGO.GetComponent<LaserCanon>();
            laserCanons.Add(laser);
            laserGO.SetActive(false);
        }
    }

    void UpdateLaserCanons()
    {
        foreach (LaserCanon laser in laserCanons)
        {
            laser.ShootLaser();
        }
    }
}
