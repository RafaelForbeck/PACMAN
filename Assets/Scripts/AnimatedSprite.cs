using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[0];  // Array de sprites para a animação.
    public float animationTime = 0.25f;  // Tempo entre cada frame da animação.
    public bool loop = true;  // Indica se a animação deve reiniciar após terminar.

    private SpriteRenderer spriteRenderer;  // Referência ao componente SpriteRenderer.
    private int animationFrame;  // Índice do frame atual na animação.

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // Obtém a referência ao SpriteRenderer no início.
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;  // Habilita o SpriteRenderer quando o objeto é ativado.
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;  // Desabilita o SpriteRenderer quando o objeto é desativado.
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);  // Chama o método Advance repetidamente a cada 'animationTime' segundos.
    }

    private void Advance()
    {
        if (!spriteRenderer.enabled)
        {
            return;  // Se o SpriteRenderer não está habilitado, não faz nada.
        }

        animationFrame++;  // Avança para o próximo frame.

        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;  // Se o frame atual é maior que o número de sprites e loop está habilitado, reinicia a animação.
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];  // Atualiza o sprite do SpriteRenderer para o próximo frame.
        }
    }

    public void Restart()
    {
        animationFrame = -1;  // Reseta o frame atual.

        Advance();  // Avança para o primeiro frame.
    }
}