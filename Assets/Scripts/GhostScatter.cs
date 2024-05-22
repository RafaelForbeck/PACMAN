using UnityEngine;

public class GhostScatter : GhostBehavior
{
    // M�todo chamado quando o comportamento � desativado
    private void OnDisable()
    {
        // Quando o modo Scatter � desativado, ativa o modo Chase
        ghost.chase.Enable();
    }

    // M�todo chamado ao colidir com outro collider (n�)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Obt�m o componente Node do objeto com o qual colidiu
        Node node = other.GetComponent<Node>();

        // Verifica se o n� n�o � nulo, o comportamento Scatter est� habilitado,
        // e o fantasma n�o est� no modo Frightened
        if (node != null && enabled && !ghost.frightened.enabled)
        {
            // Escolhe uma dire��o aleat�ria dispon�vel
            int index = Random.Range(0, node.availableDirections.Count);

            // Prefere n�o voltar na mesma dire��o, ent�o incrementa o �ndice para a pr�xima dire��o dispon�vel
           
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -ghost.movement.direction)
            {
                index++;

                // Se o �ndice exceder o n�mero de dire��es dispon�veis, volta para o in�cio
                if (index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            // Define a dire��o do movimento para a dire��o escolhida
            ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}