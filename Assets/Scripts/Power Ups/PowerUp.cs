using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material highlightMaterial;

    SpriteRenderer sr;
    bool isHighlight = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandlePowerUp();
    }

    void HandlePowerUp()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int layerMask = 1 << gameObject.layer; // Convert the layer to a mask
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.down, .2f, layerMask);
        if (hit.collider != null)
        {
            if (!isHighlight)
            {
                isHighlight = true;
                sr.material = highlightMaterial;
            }
            if (Input.GetMouseButtonDown(0))
            {
                PowerUpController.Instance.HandleSelectedPowerUp(hit.collider.gameObject.tag);
            }
        }
        else
        {
            if (isHighlight)
            {
                isHighlight = false;
                sr.material = defaultMaterial;
            }
        }
    }
}
