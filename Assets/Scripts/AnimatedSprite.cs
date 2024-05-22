using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[0];  // Array de sprites para a anima��o.
    public float animationTime = 0.25f;  // Tempo entre cada frame da anima��o.
    public bool loop = true;  // Indica se a anima��o deve reiniciar ap�s terminar.

    private SpriteRenderer spriteRenderer;  // Refer�ncia ao componente SpriteRenderer.
    private int animationFrame;  // �ndice do frame atual na anima��o.

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();  // Obt�m a refer�ncia ao SpriteRenderer no in�cio.
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;  // Habilita o SpriteRenderer quando o objeto � ativado.
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;  // Desabilita o SpriteRenderer quando o objeto � desativado.
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);  // Chama o m�todo Advance repetidamente a cada 'animationTime' segundos.
    }

    private void Advance()
    {
        if (!spriteRenderer.enabled)
        {
            return;  // Se o SpriteRenderer n�o est� habilitado, n�o faz nada.
        }

        animationFrame++;  // Avan�a para o pr�ximo frame.

        if (animationFrame >= sprites.Length && loop)
        {
            animationFrame = 0;  // Se o frame atual � maior que o n�mero de sprites e loop est� habilitado, reinicia a anima��o.
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length)
        {
            spriteRenderer.sprite = sprites[animationFrame];  // Atualiza o sprite do SpriteRenderer para o pr�ximo frame.
        }
    }

    public void Restart()
    {
        animationFrame = -1;  // Reseta o frame atual.

        Advance();  // Avan�a para o primeiro frame.
    }
}