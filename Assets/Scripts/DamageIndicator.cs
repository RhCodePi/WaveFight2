using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private Vector3 scale;
    [SerializeField] private Text text;
    [SerializeField] private float lifeTime = 0.6f, minDist = 2f,  maxDist = 3f;
    private Vector3 iniPos;
    private Vector3 targetPos;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        float direction = Random.rotation.eulerAngles.y;
        iniPos = transform.position;
        float dist = Random.Range(minDist,maxDist);
        targetPos = iniPos + (Quaternion.Euler(0,direction,0) * new Vector3(0,dist,0));
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float fraction = lifeTime / 2f;
        if (timer > lifeTime) Destroy(gameObject);
        else if (timer > fraction) text.color = Color.Lerp(text.color, Color.clear, (timer - fraction)/(lifeTime-fraction)); 
        transform.position = Vector3.Lerp(iniPos, targetPos, Mathf.Sin(timer / lifeTime));
        transform.localScale = Vector3.Lerp(Vector3.zero, scale, Mathf.Sin(timer / lifeTime));
    }
    public void SetDamageText(float damage)
    {
        text.text = damage.ToString();
    }
}
