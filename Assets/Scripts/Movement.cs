using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Garante que o objeto tenha um Rigidbody2D
public class Movement : MonoBehaviour
{
    // Velocidade do movimento
    public float speed = 8f;
    // Multiplicador de velocidade para efeitos especiais (como Power Pellets)
    public float speedMultiplier = 1f;
    // Dire��o inicial do movimento
    public Vector2 initialDirection;
    // Camada de obst�culos para detec��o de colis�o
    public LayerMask obstacleLayer;

    // Propriedades p�blicas somente leitura dos componentes Rigidbody2D e dire��o do movimento
    public new Rigidbody2D rigidbody { get; private set; }
    public Vector2 direction { get; private set; }
    public Vector2 nextDirection { get; private set; }
    // Posi��o inicial do objeto
    public Vector3 startingPosition { get; private set; }

    // Chamado quando o script � inicializado
    private void Awake()
    {
        // Obt�m o componente Rigidbody2D e a posi��o inicial
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
    }

    // Chamado quando o script � iniciado
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
        // Tenta mover na pr�xima dire��o se ela estiver definida para tornar os movimentos mais responsivos
        if (nextDirection != Vector2.zero)
        {
            SetDirection(nextDirection);
        }
    }

    // Chamado em intervalos fixos de tempo
    private void FixedUpdate()
    {
        // Move o objeto na dire��o atual com a velocidade definida
        Vector2 position = rigidbody.position;
        Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;
        rigidbody.MovePosition(position + translation);
    }

    // Define a dire��o de movimento do objeto
    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // Verifica se a dire��o � v�lida e se a pr�xima dire��o j� est� definida
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

    // Verifica se a dire��o especificada est� ocupada por um obst�culo
    public bool Occupied(Vector2 direction)
    {
        // Lan�a um raio na dire��o especificada para verificar colis�es com obst�culos
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0f, direction, 1.5f, obstacleLayer);
        // Retorna true se um obst�culo for atingido, caso contr�rio, retorna false
        return hit.collider != null;
    }
}