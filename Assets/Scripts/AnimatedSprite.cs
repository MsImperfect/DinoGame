using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;
    private int frame;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        Invoke(nameof(Animate), 0f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void Animate()
    {
        frame++;
        if (frame == sprites.Length)
        {
            frame = 0;
        }
        if (frame >= 0 && frame < sprites.Length)
        {
            Color currentColor = spriteRenderer.color;
            spriteRenderer.sprite = sprites[frame];
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a);
        }
        Invoke(nameof(Animate),1f/GameManager.Instance.gameSpeed);
    }
}

