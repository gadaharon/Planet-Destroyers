using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Handler : EnemyBase
{
    [SerializeField] float stoppingPoint = 10f;
    [SerializeField] float closestPlayerPoint = 3f;
    [SerializeField] float boundsPadding = 3f;


    Bounds bounds;
    Vector3 playerPosition = Vector3.zero;

    void Start()
    {
        bounds = GameManager.Instance.GetCameraBounds();
    }

    public override void HandleDamage()
    {
    }

    Vector2 MoveTowardsClamp(Vector2 position)
    {
        position.x = Mathf.Clamp(position.x, bounds.min.x + boundsPadding, bounds.max.x - boundsPadding);
        position.y = Mathf.Clamp(position.y, bounds.min.y + boundsPadding, bounds.max.y - boundsPadding);

        return position;
    }

    public override void Move()
    {
        playerPosition = PlayerController.Instance.transform.position;

        if (Vector2.Distance(transform.position, playerPosition) > stoppingPoint)
        {
            Vector2 position = Vector2.MoveTowards(transform.position, playerPosition, movementSpeed * Time.deltaTime);
            transform.position = MoveTowardsClamp(position);
        }
        else if (Vector2.Distance(transform.position, playerPosition) < closestPlayerPoint)
        {
            Vector2 position = Vector2.MoveTowards(transform.position, playerPosition, -movementSpeed * Time.deltaTime);
            Vector2 clampedPosition = MoveTowardsClamp(position);
            if (IsAtBoundary(clampedPosition))
            {
                HandleCornerMovement(clampedPosition, playerPosition);
            }
            else
            {
                transform.position = clampedPosition;
            }
        }
    }

    bool IsAtBoundary(Vector2 position)
    {
        return position.x <= (bounds.min.x + 10) || position.x >= (bounds.max.x - 10) || position.y <= (bounds.min.y + 10) || position.y >= (bounds.max.y - 10);
    }

    void HandleCornerMovement(Vector2 position, Vector2 playerPosition)
    {
        Vector2 escapeDirection = Vector2.zero;


        // Check if the enemy is at the left or right boundary
        if (position.x <= (bounds.min.x + 10))
        {
            if (position.y > bounds.center.y) { escapeDirection = Vector2.down; }
            else { escapeDirection = Vector2.up; }
        }
        else if (position.x >= (bounds.max.x - 10))
        {
            if (position.y > bounds.center.y) { escapeDirection = Vector2.down; }
            else { escapeDirection = Vector2.up; }
        }

        // Check if the enemy is at the top or bottom boundary
        else if (position.y <= (bounds.min.y + 10))
        {
            // Stuck at the bottom boundary, move left or right depending on player's x position
            // escapeDirection = (position.x < playerPosition.x) ? Vector2.left : Vector2.right;
            if (position.x > bounds.center.x) { escapeDirection = Vector2.left; }
            else { escapeDirection = Vector2.right; }
        }
        else if (position.y >= bounds.max.y - 10)
        {
            // Stuck at the top boundary, move left or right depending on player's x position
            // escapeDirection = (position.x < playerPosition.x) ? Vector2.left : Vector2.right;
            if (position.x < bounds.center.x) { escapeDirection = Vector2.left; }
            else { escapeDirection = Vector2.right; }
        }

        // Apply movement in the chosen escape direction
        transform.position += (Vector3)(escapeDirection * movementSpeed * Time.deltaTime);
    }

    public override void TakeDamage(int damage)
    {
    }
}
