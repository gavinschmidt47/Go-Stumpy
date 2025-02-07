using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject horrizontalAtt;
    public float minFirstAttackTime = 2.0f;
    public float maxFirstAttackTime = 5.0f;
    public float minSecondAttackTime = 2.0f;
    public float maxSecondAttackTime = 5.0f;
    public float minThirdAttackTime = 2.0f;
    public float maxThirdAttackTime = 5.0f;

    private int attackCount = 0;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(HorizontalAttack());
    }

    /*IEnumerator HorizontalAttack()
    {
        yield return new WaitForSeconds(Random.Range(minFirstAttackTime, maxFirstAttackTime));
        Instantiate(horrizontalAtt, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(Random.Range(minSecondAttackTime, maxSecondAttackTime));
        Instantiate(horrizontalAtt, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(Random.Range(minThirdAttackTime, maxThirdAttackTime));
        Instantiate(horrizontalAtt, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }


    void OnDestroy()
    {
        
    }*/
}
