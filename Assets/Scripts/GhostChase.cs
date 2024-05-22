using UnityEngine;

public class GhostChase : GhostBehavior
{
    private void OnDisable()
    {
        ghost.scatter.Enable();  // Ativa o comportamento "Scatter" quando o comportamento "Chase" � desativado.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        // N�o faz nada enquanto o fantasma est� no estado "Frightened"
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // Encontra a dire��o dispon�vel que se move mais perto de Pac-Man
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                // Se a dist�ncia nesta dire��o for menor que a dist�ncia m�nima atual,
                // esta dire��o se torna a nova mais pr�xima
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (ghost.target.position - newPosition).sqrMagnitude;

                if (distance < minDistance)
                {
                    direction = availableDirection;
                    minDistance = distance;
                }
            }

            ghost.movement.SetDirection(direction);  // Define a dire��o do movimento do fantasma.
        }
    }
}