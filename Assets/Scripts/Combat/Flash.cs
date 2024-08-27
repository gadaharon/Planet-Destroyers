using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material flashMaterial;
    [SerializeField] float flashTime = 0.1f;
    [SerializeField] SpriteRenderer spriteRenderer;

    public void StartFlash()
    {
        if (spriteRenderer == null)
        {
            Debug.Log($"Sprite renderer not found on Flash script of gameobject: {gameObject.name}");
            return;
        }
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(flashTime);
        SetDefaultMaterial();
    }

    void SetDefaultMaterial()
    {
        spriteRenderer.material = defaultMaterial;
    }
}
