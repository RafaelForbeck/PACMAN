using UnityEngine;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }  // Componente de movimento do fantasma.
    public GhostHome home { get; private set; }  // Componente que controla o comportamento "Home" do fantasma.
    public GhostScatter scatter { get; private set; }  // Componente que controla o comportamento "Scatter" do fantasma.
    public GhostChase chase { get; private set; }  // Componente que controla o comportamento "Chase" do fantasma.
    public GhostFrightened frightened { get; private set; }  // Componente que controla o comportamento "Frightened" do fantasma.
    public GhostBehavior initialBehavior;  // Comportamento inicial do fantasma.
    public Transform target;  // Alvo que o fantasma persegue.
    public int points = 200;  // Pontos ganhos ao comer este fantasma.

    private void Awake()
    {
        // Inicializa as referências aos componentes de comportamento.
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
    }

    private void Start()
    {
        ResetState();  // Reseta o estado do fantasma ao iniciar.
    }

    public void ResetState()
    {
        gameObject.SetActive(true);  // Ativa o GameObject do fantasma.
        movement.ResetState();  // Reseta o estado do movimento.

        frightened.Disable();  // Desativa o comportamento "Frightened".
        chase.Disable();  // Desativa o comportamento "Chase".
        scatter.Enable();  // Ativa o comportamento "Scatter".

        if (home != initialBehavior)
        {
            home.Disable();  // Desativa o comportamento "Home" se não for o comportamento inicial.
        }

        if (initialBehavior != null)
        {
            initialBehavior.Enable();  // Ativa o comportamento inicial, se existir.
        }
    }

    public void SetPosition(Vector3 position)
    {
        // Mantém a posição z atual para preservar a profundidade de desenho.
        position.z = transform.position.z;
        transform.position = position;  // Define a nova posição do fantasma.
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled)
            {
                GameManager.Instance.GhostEaten(this);  // Notifica o GameManager que o fantasma foi comido se estiver no estado "Frightened".
            }
            else
            {
                GameManager.Instance.PacmanEaten();  // Notifica o GameManager que Pacman foi comido se o fantasma não estiver no estado "Frightened".
            }
        }
    }
}