using UnityEngine;

[RequireComponent(typeof(Movement))] // Requer um componente Movement para funcionar corretamente
public class Pacman : MonoBehaviour
{
    [SerializeField] // Permite que o campo abaixo seja editado no Editor Unity
    private AnimatedSprite deathSequence; // Sequ�ncia de anima��o da morte do Pac-Man
    private SpriteRenderer spriteRenderer; // Renderizador de sprites do Pac-Man
    private Movement movement; // Componente de movimento do Pac-Man
    private new Collider2D collider; // Collider do Pac-Man (oculta o collider herdado)

    // Chamado quando o script � inicializado
    private void Awake()
    {
        // Obt�m os componentes necess�rios do Pac-Man
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        collider = GetComponent<Collider2D>();
    }

    // Chamado a cada frame
    private void Update()
    {
        // Define a nova dire��o com base na entrada atual do jogador
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right);
        }

        // Rotaciona o Pac-Man para enfrentar a dire��o do movimento
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    // Reseta o estado do Pac-Man
    public void ResetState()
    {
        // Ativa o Pac-Man e seus componentes, desativados durante a morte
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        deathSequence.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    // Executa a sequ�ncia de morte do Pac-Man
    public void DeathSequence()
    {
        // Desativa o Pac-Man e seus componentes para a anima��o de morte
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }
}