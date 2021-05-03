using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RockCooldown : MonoBehaviour
{
    [SerializeField]
    private Image imageCooldown;
    [SerializeField]
    private Text textCooldown;

    private bool isCooldown = false;
    private float cooldownTime = 4.0f;
    private float cooldownTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCooldown)
        {
            return;
        }
        if (cooldownTimer < 0.0f)
        {
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
            isCooldown = false;
            return;
        }
        cooldownTimer -= Time.deltaTime;
        textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
        imageCooldown.fillAmount = cooldownTimer / cooldownTime;
    }

    public bool StartCooldown()
    {
        if (isCooldown)
        {
            return false;
        }
        isCooldown = true;
        textCooldown.gameObject.SetActive(true);
        cooldownTimer = cooldownTime;
        return true;
    }
}
