using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public string enemyChoice = "";

    public void MakeChoice()
    {
        int r = Random.Range(0, 3);
        if (r == 0) enemyChoice = "Rock";
        else if (r == 1) enemyChoice = "Paper";
        else enemyChoice = "Scissors";

        Debug.Log("Enemy chooses: " + enemyChoice);
    }

    public string GetChoice()
    {
        return enemyChoice;
    }
}
