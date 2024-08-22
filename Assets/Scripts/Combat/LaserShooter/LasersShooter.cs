using System;
using System.Collections.Generic;
using UnityEngine;

public class LasersShooter : MonoBehaviour
{
    [SerializeField] GameObject laserCanonPrefab;
    [SerializeField] float rotationSpeed = 100f;

    int numberOfCanons = 8;
    BoxCollider2D collider;
    Bounds bounds;
    List<LaserCanon> laserCanons = new List<LaserCanon>();

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        bounds = collider.bounds;
    }

    void Update()
    {
        transform.position = PlayerController.Instance.transform.position;
        transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);

        UpdateLaserCanons();
    }

    void Start()
    {
        GenerateLaserCanons();
    }

    void GenerateLaserCanons()
    {
        for (int i = 0; i < numberOfCanons; i++)
        {
            Quaternion rotation;
            Vector2 canonPosition = bounds.center;
            rotation = Quaternion.Euler(0, 0, i * 45);
            GameObject laserGO = Instantiate(laserCanonPrefab, canonPosition, rotation, transform);
            LaserCanon laser = laserGO.GetComponent<LaserCanon>();
            laserCanons.Add(laser);
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
