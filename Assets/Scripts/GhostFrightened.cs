using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    // Referências aos SpriteRenderers dos diferentes estados visuais do fantasma
    public SpriteRenderer body;  // SpriteRenderer do corpo normal do fantasma
    public SpriteRenderer eyes;  // SpriteRenderer dos olhos do fantasma
    public SpriteRenderer blue;  // SpriteRenderer para o estado "frightened" (azul)
    public SpriteRenderer white; // SpriteRenderer para o estado "flash" (branco)

    private bool eaten;  // Estado para verificar se o fantasma foi comido

    // Método para ativar o estado frightened com uma duração específica
    public override void Enable(float duration)
    {
        base.Enable(duration);

        // Configura os SpriteRenderers para o estado frightened
        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        // Inicia o "flash" na metade da duração do estado frightened
        Invoke(nameof(Flash), duration / 2f);
    }

    // Método para desativar o estado frightened
    public override void Disable()
    {
        base.Disable();

        // Restaura os SpriteRenderers para o estado normal
        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Método para tratar o fantasma sendo comido por Pac-Man
    private void Eaten()
    {
        eaten = true;
        ghost.SetPosition(ghost.home.inside.position);  // Move o fantasma para dentro da casa
        ghost.home.Enable(duration);  // Ativa o estado de casa do fantasma

        // Configura os SpriteRenderers para o estado "eaten"
        body.enabled = false;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // Método para iniciar o "flash" quando o estado frightened está quase acabando
    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();  // Reinicia a animação do estado "flash"
        }
    }

    // Método chamado quando o estado frightened é ativado
    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart();  // Reinicia a animação do estado "frightened"
        ghost.movement.speedMultiplier = 0.5f;  // Reduz a velocidade do fantasma
        eaten = false;  // Reseta o estado de comido
    }

    // Método chamado quando o estado frightened é desativado
    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f;  // Restaura a velocidade normal do fantasma
        eaten = false;  // Reseta o estado de comido
    }

    // Método chamado quando o fantasma colide com um nó
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            // Encontra a direção que se afasta mais de Pac-Man
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);
        }
    }

    // Método chamado quando o fantasma colide com outro objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled)
            {
                Eaten();  // Tratar a colisão com Pac-Man enquanto está no estado frightened
            }
        }
    }
}