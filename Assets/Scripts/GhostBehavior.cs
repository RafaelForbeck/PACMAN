using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; }  // Referência ao componente Ghost associado.
    public float duration;  // Duração do comportamento.

    private void Awake()
    {
        ghost = GetComponent<Ghost>();  // Inicializa a referência ao componente Ghost.
    }

    public void Enable()
    {
        Enable(duration);  // Ativa o comportamento com a duração padrão.
    }

    public virtual void Enable(float duration)
    {
        enabled = true;  // Habilita o componente, permitindo que Update e outros métodos do ciclo de vida do Unity sejam chamados.

        CancelInvoke();  // Cancela qualquer invocação pendente de métodos.
        Invoke(nameof(Disable), duration);  // Agenda a desativação do comportamento após o tempo especificado.
    }

    public virtual void Disable()
    {
        enabled = false;  // Desabilita o componente, impedindo que Update e outros métodos do ciclo de vida do Unity sejam chamados.

        CancelInvoke();  // Cancela qualquer invocação pendente de métodos.
    }
}