using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        ghost.scatter.Enable();  // Ativa o comportamento "Scatter" quando o comportamento "Chase" é desativado.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // Não faz nada enquanto o fantasma está no estado "Frightened"
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // Encontra a direção disponível que se move mais perto de Pac-Man
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Se a distância nesta direção for menor que a distância mínima atual,
                // esta direção se torna a nova mais próxima
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);  // Define a direção do movimento do fantasma.
        }
    }
}