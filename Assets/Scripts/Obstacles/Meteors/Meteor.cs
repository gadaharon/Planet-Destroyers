using UnityEngine;

public class Meteor : MonoBehaviour
{
    Transform[] waypoints;
    float speed;
    int damage;
    int currentWaypointIndex = 0;

    void Start()
    {
        if (waypoints != null || waypoints.Length > 0)
        {
            transform.position = waypoints[currentWaypointIndex].position;
        }
    }

    void Update()
    {
        FollowPath();
    }

    public void Initialize(Transform[] waypoints, float speed, int damage)
    {
        this.waypoints = waypoints;
        this.speed = speed;
        this.damage = damage;
    }

    void FollowPath()
    {
        if (currentWaypointIndex < waypoints.Length)
        {
            Vector3 targetWaypoint = waypoints[currentWaypointIndex].position;
            float delta = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint, delta);
            if (transform.position == targetWaypoint)
            {
                currentWaypointIndex++;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<IDamagable>() != null)
        {
            other.gameObject.GetComponent<IDamagable>().TakeDamage(gameObject.tag, damage);
        }
    }


}
