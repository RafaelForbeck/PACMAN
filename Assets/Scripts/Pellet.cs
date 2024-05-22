using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Requer um componente Collider2D para funcionar corretamente
public class Pellet : MonoBehaviour
{
    public int points = 10; // Os pontos que o jogador ganha ao comer as bolinhas

    // Chamado quando a bolinha é comida
    protected virtual void Eat()
    {
        // Notifica o GameManager que a bolinha foi comida e passa esta instância da bolinha como parâmetro
        GameManager.Instance.PelletEaten(this);
    }

    // Chamado quando outro collider entra em contato com o collider desta bolinha
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se o collider que entrou em contato é do Pac-Man
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            // Se for, chama o método Eat()
            Eat();
        }
    }
}