using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // Instância singleton do GameManager.

    [SerializeField] private Ghost[] ghosts;  // Array de fantasmas no jogo.
    [SerializeField] private Pacman pacman;  // Referência ao objeto Pacman.
    [SerializeField] private Transform pellets;  // Referência ao objeto que contém todos os pellets.
    [SerializeField] private Text gameOverText;  // Texto de "Game Over" na UI.
    [SerializeField] private Text scoreText;  // Texto que exibe a pontuação na UI.
    [SerializeField] private Text livesText;  // Texto que exibe o número de vidas na UI.
    [SerializeField] private Text playGameText;  // Texto que exibe o número de vidas na UI.

    private int ghostMultiplier = 1;  // Multiplicador de pontos ao comer fantasmas.
    private int lives = 3;  // Número inicial de vidas.
    private int score = 0;  // Pontuação inicial.

    public int Lives => lives;  // Propriedade pública para obter o número de vidas.
    public int Score => score;  // Propriedade pública para obter a pontuação.

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);  // Se uma instância já existe, destrói este objeto.
        }
        else
        {
            Instance = this;  // Define a instância singleton.
            DontDestroyOnLoad(gameObject);  // Impede que o objeto seja destruído ao carregar uma nova cena.
        }
    }

    private void Start()
    {
        NewGame();  // Inicia um novo jogo quando o script começa.
        playGameText.enabled = false;  // Desativa o texto de "Play Game".
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown)
        {
            NewGame();  // Reinicia o jogo se todas as vidas foram perdidas e qualquer tecla é pressionada.
        }
    }

    private void NewGame()
    {
        SetScore(0);  // Reseta a pontuação.
        SetLives(3);  // Reseta o número de vidas.
        NewRound();  // Inicia uma nova rodada.
    }

    private void NewRound()
    {
        gameOverText.enabled = false;  // Desativa o texto de "Game Over".
      

        foreach (Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);  // Ativa todos os pellets.
        }

        ResetState();  // Reseta o estado dos personagens.
    }

    private void ResetState()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].ResetState();  // Reseta o estado de cada fantasma.
        }

        pacman.ResetState();  // Reseta o estado do Pacman.
    }

    private void GameOver()
    {
        gameOverText.enabled = true;  // Ativa o texto de "Game Over".

        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);  // Desativa todos os fantasmas.
        }

        pacman.gameObject.SetActive(false);  // Desativa o Pacman.
    }

    private void PlayGame()
    {
        playGameText.enabled = true;  // Ativa o texto de "Play Game".

    }

    private void SetLives(int lives)
    {
        this.lives = lives;  // Atualiza o número de vidas.
        livesText.text = "x" + lives.ToString();  // Atualiza o texto de vidas na UI.
    }

    private void SetScore(int score)
    {
        this.score = score;  // Atualiza a pontuação.
        scoreText.text = score.ToString().PadLeft(2, '0');  // Atualiza o texto de pontuação na UI.
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();  // Inicia a sequência de morte do Pacman.

        SetLives(lives - 1);  // Reduz uma vida.

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3f);  // Reseta o estado após 3 segundos se ainda houver vidas.
        }
        else
        {
            GameOver();  // Termina o jogo se não houver mais vidas.
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;  // Calcula os pontos com o multiplicador.
        SetScore(score + points);  // Atualiza a pontuação.

        ghostMultiplier++;  // Aumenta o multiplicador de pontos.
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);  // Desativa o pellet.

        SetScore(score + pellet.points);  // Atualiza a pontuação.

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);  // Desativa o Pacman.
            Invoke(nameof(NewRound), 3f);  // Inicia uma nova rodada após 3 segundos.
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(pellet.duration);  // Ativa o modo assustado para cada fantasma.
        }

        PelletEaten(pellet);  // Trate o Power Pellet como um Pellet regular.
        CancelInvoke(nameof(ResetGhostMultiplier));  // Cancela o reset do multiplicador.
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);  // Reseta o multiplicador após a duração do Power Pellet.
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;  // Retorna true se ainda houver pellets ativos.
            }
        }

        return false;  // Retorna false se todos os pellets foram comidos.
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;  // Reseta o multiplicador de pontos.
    }
}