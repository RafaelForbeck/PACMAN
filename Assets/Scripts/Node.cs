using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Camada de obstáculos para detecção de colisão
    public LayerMask obstacleLayer;
    // Lista de direções disponíveis a partir deste nó
    public readonly List<Vector2> availableDirections = new();

    // Chamado quando o script é iniciado
    private void Start()
    {
        // Limpa a lista de direções disponíveis
        availableDirections.Clear();

        // Determina se a direção está disponível realizando um box cast para verificar se
        // atingimos uma parede. A direção é adicionada à lista se estiver disponível.
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    // Verifica se a direção especificada está disponível a partir deste nó
    private void CheckAvailableDirection(Vector2 direction)
    {
        // Lança um box cast na direção especificada para verificar colisões com obstáculos
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

        // Se nenhum collider for atingido, não há obstáculo naquela direção
        if (hit.collider == null)
        {
            availableDirections.Add(direction);
        }
    }
}