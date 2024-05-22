using System.Collections;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    // Posições de referência para dentro e fora da casa do fantasma
    public Transform inside;  // Posição dentro da casa do fantasma
    public Transform outside; // Posição fora da casa do fantasma

    // Método chamado quando o comportamento é ativado
    private void OnEnable()
    {
        StopAllCoroutines();  // Para todas as corrotinas em execução
    }

    // Método chamado quando o comportamento é desativado
    private void OnDisable()
    {
        // Verifica se o objeto ainda está ativo na hierarquia para evitar erros ao destruir o objeto
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(ExitTransition());  // Inicia a corrotina para a transição de saída da casa
        }
    }

    // Método chamado quando há uma colisão
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Inverte a direção toda vez que o fantasma colide com uma parede,
        // criando o efeito de "quicar" dentro da casa
        if (enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            ghost.movement.SetDirection(-ghost.movement.direction);
        }
    }

    // Corrotina para gerenciar a transição de saída da casa do fantasma
    private IEnumerator ExitTransition()
    {
        // Desativa o movimento enquanto a posição é animada manualmente
        ghost.movement.SetDirection(Vector2.up, true);
        ghost.movement.rigidbody.isKinematic = true;
        ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0f;

        // Animação para mover até o ponto inicial (dentro da casa)
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(position, inside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;

        // Animação para sair da casa
        while (elapsed < duration)
        {
            ghost.SetPosition(Vector3.Lerp(inside.position, outside.position, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Define uma direção aleatória (esquerda ou direita) e reativa o movimento
        ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1f : 1f, 0f), true);
        ghost.movement.rigidbody.isKinematic = false;
        ghost.movement.enabled = true;
    }
}