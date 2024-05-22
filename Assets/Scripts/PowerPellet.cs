using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f; // A duração da Power Pellet em segundos

    // Chamado quando a Power Pellet é comida
    protected override void Eat()
    {
        // Notifica o GameManager que a Power Pellet foi comida e passa esta instância da Power Pellet como parâmetro
        GameManager.Instance.PowerPelletEaten(this);
    }
}