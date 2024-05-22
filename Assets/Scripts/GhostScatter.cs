using UnityEngine;

public class GhostScatter : GhostBehavior
{
    // Método chamado quando o comportamento é desativado
    private void OnDisable()
    {
        // Quando o modo Scatter é desativado, ativa o modo Chase
        ghost.chase.Enable();
    }

    // Método chamado ao colidir com outro collider (nó)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Obtém o componente Node do objeto com o qual colidiu
        Node node = other.GetComponent<Node>();

        // Verifica se o nó não é nulo, o comportamento Scatter está habilitado,
        // e o fantasma não está no modo Frightened
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            // Escolhe uma direção aleatória disponível
            int index = Random.Range(0, node.availableDirections.Count);

            // Prefere não voltar na mesma direção, então incrementa o índice para a próxima direção disponível
           
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.movement.direction)
            {
                index++;

                // Se o índice exceder o número de direções disponíveis, volta para o início
                if (index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            // Define a direção do movimento para a direção escolhida
            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}