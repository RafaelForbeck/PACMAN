using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyes : MonoBehaviour
{
    // Sprites para diferentes direções
    public Sprite up;  // Sprite dos olhos olhando para cima
    public Sprite down;  // Sprite dos olhos olhando para baixo
    public Sprite left;  // Sprite dos olhos olhando para a esquerda
    public Sprite right;  // Sprite dos olhos olhando para a direita

    // Referências internas aos componentes
    private SpriteRenderer spriteRenderer;  // Referência ao componente SpriteRenderer
    private Movement movement;  // Referência ao componente Movement do objeto pai

    private void Awake()
    {
        // Inicializa as referências aos componentes
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<Movement>();  // Obtém o componente Movement do objeto pai
    }

    private void Update()
    {
        // Atualiza o sprite dos olhos com base na direção do movimento
        if (movement.direction == Vector2.up)
        {
            spriteRenderer.sprite = up;  // Define o sprite dos olhos para cima
        }
        else if (movement.direction == Vector2.down)
        {
            spriteRenderer.sprite = down;  // Define o sprite dos olhos para baixo
        }
        else if (movement.direction == Vector2.left)
        {
            spriteRenderer.sprite = left;  // Define o sprite dos olhos para a esquerda
        }
        else if (movement.direction == Vector2.right)
        {
            spriteRenderer.sprite = right;  // Define o sprite dos olhos para a direita
        }
    }
}