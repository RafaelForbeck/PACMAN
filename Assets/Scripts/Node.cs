using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // Camada de obst�culos para detec��o de colis�o
    public LayerMask obstacleLayer;
    // Lista de dire��es dispon�veis a partir deste n�
    public readonly List<Vector2> availableDirections = new();

    // Chamado quando o script � iniciado
    private void Start()
    {
        // Limpa a lista de dire��es dispon�veis
        availableDirections.Clear();

        // Determina se a dire��o est� dispon�vel realizando um box cast para verificar se
        // atingimos uma parede. A dire��o � adicionada � lista se estiver dispon�vel.
        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    // Verifica se a dire��o especificada est� dispon�vel a partir deste n�
    private void CheckAvailableDirection(Vector2 direction)
    {
        // Lan�a um box cast na dire��o especificada para verificar colis�es com obst�culos
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1f, obstacleLayer);

        // Se nenhum collider for atingido, n�o h� obst�culo naquela dire��o
        if (hit.collider == null)
        {
            availableDirections.Add(direction);
        }
    }
}