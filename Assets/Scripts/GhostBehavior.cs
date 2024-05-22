using UnityEngine;

[RequireComponent(typeof(Ghost))]
public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; }  // Refer�ncia ao componente Ghost associado.
    public float duration;  // Dura��o do comportamento.

    private void Awake()
    {
        ghost = GetComponent<Ghost>();  // Inicializa a refer�ncia ao componente Ghost.
    }

    public void Enable()
    {
        Enable(duration);  // Ativa o comportamento com a dura��o padr�o.
    }

    public virtual void Enable(float duration)
    {
        enabled = true;  // Habilita o componente, permitindo que Update e outros m�todos do ciclo de vida do Unity sejam chamados.

        CancelInvoke();  // Cancela qualquer invoca��o pendente de m�todos.
        Invoke(nameof(Disable), duration);  // Agenda a desativa��o do comportamento ap�s o tempo especificado.
    }

    public virtual void Disable()
    {
        enabled = false;  // Desabilita o componente, impedindo que Update e outros m�todos do ciclo de vida do Unity sejam chamados.

        CancelInvoke();  // Cancela qualquer invoca��o pendente de m�todos.
    }
}