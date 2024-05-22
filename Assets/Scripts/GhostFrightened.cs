using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    // Refer�ncias aos SpriteRenderers dos diferentes estados visuais do fantasma
    public SpriteRenderer body;  // SpriteRenderer do corpo normal do fantasma
    public SpriteRenderer eyes;  // SpriteRenderer dos olhos do fantasma
    public SpriteRenderer blue;  // SpriteRenderer para o estado "frightened" (azul)
    public SpriteRenderer white; // SpriteRenderer para o estado "flash" (branco)

    private bool eaten;  // Estado para verificar se o fantasma foi comido

    // M�todo para ativar o estado frightened com uma dura��o espec�fica
    public override void Enable(float duration)
    {
        base.Enable(duration);

        // Configura os SpriteRenderers para o estado frightened
        body.enabled = false;
        eyes.enabled = false;
        blue.enabled = true;
        white.enabled = false;

        // Inicia o "flash" na metade da dura��o do estado frightened
        Invoke(nameof(Flash), duration / 2f);
    }

    // M�todo para desativar o estado frightened
    public override void Disable()
    {
        base.Disable();

        // Restaura os SpriteRenderers para o estado normal
        body.enabled = true;
        eyes.enabled = true;
        blue.enabled = false;
        white.enabled = false;
    }

    // M�todo para tratar o fantasma sendo comido por Pac-Man
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

    // M�todo para iniciar o "flash" quando o estado frightened est� quase acabando
    private void Flash()
    {
        if (!eaten)
        {
            blue.enabled = false;
            white.enabled = true;
            white.GetComponent<AnimatedSprite>().Restart();  // Reinicia a anima��o do estado "flash"
        }
    }

    // M�todo chamado quando o estado frightened � ativado
    private void OnEnable()
    {
        blue.GetComponent<AnimatedSprite>().Restart();  // Reinicia a anima��o do estado "frightened"
        ghost.movement.speedMultiplier = 0.5f;  // Reduz a velocidade do fantasma
        eaten = false;  // Reseta o estado de comido
    }

    // M�todo chamado quando o estado frightened � desativado
    private void OnDisable()
    {
        ghost.movement.speedMultiplier = 1f;  // Restaura a velocidade normal do fantasma
        eaten = false;  // Reseta o estado de comido
    }

    // M�todo chamado quando o fantasma colide com um n�
    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            // Encontra a dire��o que se afasta mais de Pac-Man
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

    // M�todo chamado quando o fantasma colide com outro objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (enabled)
            {
                Eaten();  // Tratar a colis�o com Pac-Man enquanto est� no estado frightened
            }
        }
    }
}