using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject horrizontalAtt;
    public float minAttackTime = 2.0f;
    public float maxAttackTime = 5.0f;
    public Vector2 firstAttackPosition = new Vector2(0, 0);
    public Vector2 secondAttackPosition = new Vector2(0, 0);
    public Vector2 thirdAttackPosition = new Vector2(0, 0);
    public GameObject winPanel;
    public GameObject player;
    public GameObject gameController;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HorizontalAttack());
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    IEnumerator HorizontalAttack()
    {
        yield return new WaitForSeconds(Random.Range(minAttackTime, maxAttackTime));
        Instantiate(horrizontalAtt, firstAttackPosition, Quaternion.identity);

        yield return new WaitForSeconds(Random.Range(minAttackTime, maxAttackTime));
        Instantiate(horrizontalAtt, secondAttackPosition, Quaternion.identity);

        yield return new WaitForSeconds(Random.Range(minAttackTime, maxAttackTime));
        Instantiate(horrizontalAtt, thirdAttackPosition, Quaternion.identity);

        yield return new WaitForSeconds(5.0f);
        //Show Win Panel
        winPanel.SetActive(true);

        //Set Cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //Freeze Player Controller
        player.GetComponent<PlayerController>().enabled = false;

        //Freeze Game Controller
        gameController.GetComponent<GameController>().enabled = false;

        Destroy(gameObject);
    }
}
