using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    // Posi��es de refer�ncia para dentro e fora da casa do fantasma
    public Transform inside;  // Posi��o dentro da casa do fantasma
    public Transform outside; // Posi��o fora da casa do fantasma

    // M�todo chamado quando o comportamento � ativado
    private void OnEnable()
    {
        StopAllCoroutines();  // Para todas as corrotinas em execu��o
    }

    // M�todo chamado quando o comportamento � desativado
    private void OnDisable()
    {
        // Verifica se o objeto ainda est� ativo na hierarquia para evitar erros ao destruir o objeto
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ExitTransition());  // Inicia a corrotina para a transi��o de sa�da da casa
        }
    }

    // M�todo chamado quando h� uma colis�o
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Inverte a dire��o toda vez que o fantasma colide com uma parede,
        // criando o efeito de "quicar" dentro da casa
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }

    // Corrotina para gerenciar a transi��o de sa�da da casa do fantasma
    private IEnumerator ExitTransition()
    {
        // Desativa o movimento enquanto a posi��o � animada manualmente
        ghost.movement.SetDirection(Vector2.up, true);
        ghost.movement.rigidbody.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0f;

        // Anima��o para mover at� o ponto inicial (dentro da casa)
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Anima��o para sair da casa
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Define uma dire��o aleat�ria (esquerda ou direita) e reativa o movimento
        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
        ghost.movement.rigidbody.isKinematic = false;
        ghost.movement.enabled = true;
    }
}