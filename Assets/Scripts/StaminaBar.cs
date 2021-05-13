using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;

    private const float MAXStamina = 100f;
    private float _currentStamina;
    private readonly WaitForSeconds _tick = new WaitForSeconds(0.005f);
    private Coroutine _regen;

    [SerializeField] private float cooldown = 2f;
    [SerializeField] private float regenAmount = 1;
    // Start is called before the first frame update
    void Start()
    {
        staminaBar = GetComponentInChildren<Slider>();
        _currentStamina = MAXStamina;
        staminaBar.maxValue = MAXStamina;
        staminaBar.value = MAXStamina;
    }
    

    public bool UseStamina(float amount)
    {
        if (_currentStamina - amount < 0) return false;
        
        _currentStamina -= amount;
        staminaBar.value = _currentStamina;
        if(_regen != null)
            StopCoroutine(_regen);
        _regen = StartCoroutine(RegenStamina());
        return true;
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(cooldown);
        while (_currentStamina < MAXStamina)
        {
            _currentStamina += MAXStamina * 0.002f * regenAmount;
            staminaBar.value = _currentStamina;
            yield return _tick;
        }

        _regen = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            UseStamina(25);
    }
}
