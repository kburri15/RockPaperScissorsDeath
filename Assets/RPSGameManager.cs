using System.Collections;
using UnityEngine;
using TMPro;

public class RPSGameManager : MonoBehaviour
{
    public GameObject rockPrefab, paperPrefab, scissorsPrefab;
    public GameObject monster;
    public Transform weaponSpawnPoint;
    public TMP_Text playerChoiceText, monsterChoiceText, resultText, instructionText;

    public GameObject currentPreviewWeapon;

    public Animator monsterAnimator;
    public AudioSource bgmAudio, rockHitAudio, paperSlapAudio, scissorsSnipAudio, monsterHitAudio, monsterAttackAudio;
    public AudioSource playerHitAudio;

    public int playerWins = 0;
    public int monsterWins = 0;

    public GameObject damageFlash;
    public Camera mainCamera;

    private string playerChoice = "";
    private string monsterChoice = "";

    private bool canChoose = true;

    void Start()
    {
        ResetGame();
    }

    void Update()
    {
        if (canChoose)
        {
            if (Input.GetKeyDown(KeyCode.A)) SelectWeapon("Rock");
            if (Input.GetKeyDown(KeyCode.S)) SelectWeapon("Paper");
            if (Input.GetKeyDown(KeyCode.D)) SelectWeapon("Scissors");
            if (Input.GetKeyDown(KeyCode.Space) && playerChoice != "")
            {
                StartCoroutine(PlayRound());
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    void SelectWeapon(string choice)
    {
        playerChoice = choice;

        if (currentPreviewWeapon)
        {
            Destroy(currentPreviewWeapon);
        }

        GameObject prefab = choice == "Rock" ? rockPrefab :
                            choice == "Paper" ? paperPrefab :
                            scissorsPrefab;

        currentPreviewWeapon = Instantiate(prefab, weaponSpawnPoint.position, Quaternion.identity);
        Rigidbody rb = currentPreviewWeapon.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    IEnumerator PlayRound()
    {
        canChoose = false;

        if (currentPreviewWeapon)
        {
            Destroy(currentPreviewWeapon); // Immediately remove the preview weapon before throwing
            currentPreviewWeapon = null;
        }

        monsterChoice = GetRandomChoice();
        playerChoiceText.text = "Player: " + playerChoice;
        monsterChoiceText.text = "Monster: " + monsterChoice;

        bool playerWinsRound = DoesPlayerWin(playerChoice, monsterChoice);
        bool isTie = playerChoice == monsterChoice;

        if (!playerWinsRound && !isTie)
        {
            monsterAnimator.Play("Attack");
            monsterAttackAudio.Play();
            playerHitAudio.Play();
            StartCoroutine(ShakeCamera());
            StartCoroutine(FlashRed());
        }
        else if (playerWinsRound)
        {
            PlayMonsterReaction(playerChoice, playerWins + 1 >= 2);

            GameObject weapon = Instantiate(
                playerChoice == "Rock" ? rockPrefab :
                playerChoice == "Paper" ? paperPrefab :
                scissorsPrefab,
                weaponSpawnPoint.position,
                Quaternion.identity
            );

            Rigidbody rb = weapon.GetComponent<Rigidbody>();
            rb.velocity = (monster.transform.position - weaponSpawnPoint.position).normalized * 10f;

            Destroy(weapon, 3f); // Despawn thrown weapon after 3 seconds

            if (playerChoice == "Rock") rockHitAudio.Play();
            if (playerChoice == "Paper") paperSlapAudio.Play();
            if (playerChoice == "Scissors") scissorsSnipAudio.Play();

            monsterHitAudio.Play();
        }

        yield return new WaitForSeconds(2f);

        if (playerWinsRound)
            playerWins++;
        else if (!isTie)
            monsterWins++;

        if (playerWins >= 2)
        {
            resultText.text = "You Win the Game!";
            instructionText.text = "Press R to Restart";
        }
        else if (monsterWins >= 2)
        {
            resultText.text = "You Lose the Game!";
            instructionText.text = "Press R to Restart";
        }
        else
        {
            resultText.text = isTie ? "It's a Tie!" : (playerWinsRound ? "You Win the Round!" : "You Lose the Round!");
            instructionText.text = "Choose your next weapon.";

            if (!isTie && playerWinsRound && playerWins < 2)
            {
                monsterAnimator.Play("Idle1");
            }

            canChoose = true;
        }
    }

    void PlayMonsterReaction(string choice, bool isFinal)
    {
        if (choice == "Rock")
        {
            if (isFinal)
                monsterAnimator.Play("Crushed");
            else
                monsterAnimator.Play("Slashed");
        }
        else if (choice == "Paper")
        {
            if (isFinal)
                monsterAnimator.Play("Suffocate");
            else
                monsterAnimator.Play("Slashed");
        }
        else if (choice == "Scissors")
        {
            if (isFinal)
                monsterAnimator.Play("Die");
            else
                monsterAnimator.Play("Slashed");
        }
    }

    string GetRandomChoice()
    {
        int rand = Random.Range(0, 3);
        return rand == 0 ? "Rock" : rand == 1 ? "Paper" : "Scissors";
    }

    bool DoesPlayerWin(string player, string monster)
    {
        return (player == "Rock" && monster == "Scissors") ||
               (player == "Paper" && monster == "Rock") ||
               (player == "Scissors" && monster == "Paper");
    }

    void ResetGame()
    {
        playerWins = 0;
        monsterWins = 0;
        playerChoice = "";
        monsterChoice = "";
        playerChoiceText.text = "";
        monsterChoiceText.text = "";
        resultText.text = "";
        instructionText.text = "Press A/S/D to Choose";

        monsterAnimator.Play("Idle1");

        if (currentPreviewWeapon)
        {
            Destroy(currentPreviewWeapon);
            currentPreviewWeapon = null;
        }

        canChoose = true;
    }

    IEnumerator ShakeCamera()
    {
        Vector3 originalPos = mainCamera.transform.position;
        float duration = 0.3f;
        float magnitude = 0.15f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            mainCamera.transform.position = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalPos;
    }

    IEnumerator FlashRed()
    {
        if (damageFlash != null)
        {
            damageFlash.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            damageFlash.SetActive(false);
        }
    }
}



