using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Garante que o objeto tenha um Rigidbody2D
public class Movement : MonoBehaviour
{
    // Velocidade do movimento
    public float speed = 8f;
    // Multiplicador de velocidade para efeitos especiais (como Power Pellets)
    public float speedMultiplier = 1f;
    // Direção inicial do movimento
    public Vector2 initialDirection;
    // Camada de obstáculos para detecção de colisão
    public LayerMask obstacleLayer;

    // Propriedades públicas somente leitura dos componentes Rigidbody2D e direção do movimento
    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    // Posição inicial do objeto
    public Vector3 startingPosition { get; private set; }

    // Chamado quando o script é inicializado
    private void Awake()
    {
        // Obtém o componente Rigidbody2D e a posição inicial
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    // Chamado quando o script é iniciado
    private void Start()
    {
        // Reseta o estado inicial do movimento
        ResetState();
    }

    // Reseta o estado do movimento para os valores iniciais
    public void ResetState()
    {
        speedMultiplier = 1f;
        direction = initialDirection;
        nextDirection = Vector2.zero;
        transform.position = startingPosition;
        rigidbody.isKinematic = false;
        enabled = true;
    }

    // Chamado a cada frame
    private void Update()
    {
        // Tenta mover na próxima direção se ela estiver definida para tornar os movimentos mais responsivos
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    // Chamado em intervalos fixos de tempo
    private void FixedUpdate()
    {
        // Move o objeto na direção atual com a velocidade definida
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        rigidbody.MovePosition(position + translation);
    }

    // Define a direção de movimento do objeto
    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // Verifica se a direção é válida e se a próxima direção já está definida
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            nextDirection = Vector2.zero;
        }
        else
        {
            nextDirection = direction;
        }
    }

    // Verifica se a direção especificada está ocupada por um obstáculo
    public bool Occupied(Vector2 direction)
    {
        // Lança um raio na direção especificada para verificar colisões com obstáculos
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        // Retorna true se um obstáculo for atingido, caso contrário, retorna false
        return hit.collider != null;
    }
}