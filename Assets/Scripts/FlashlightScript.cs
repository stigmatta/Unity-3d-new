using UnityEngine;

public class FlashlightScript : MonoBehaviour
{
    private GameObject player;
    private Light _light;
    public static float charge;
    public static float chargeLifetime = 60f;
    private float currentAngleOffset = 0f;
    private float angleChangeSpeed = 20f;

    void Start()
    {
        player = GameObject.Find("Player");
        if(player == null)
        {
            Debug.Log("FlashlightScript: Player not found");
        }
        _light = GetComponent<Light>();
        charge = 1f;
    }

    void Update()
    {
        if (player == null) return;
        this.transform.position = player.transform.position;

        Vector3 forwardDirection = Camera.main.transform.forward;
        Vector3 rotatedDirection = Quaternion.Euler(0f, currentAngleOffset, 0f) * forwardDirection;
        this.transform.forward = rotatedDirection;

        if (GameState.isFpv && !GameState.isDay)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                currentAngleOffset = Mathf.Clamp(currentAngleOffset - angleChangeSpeed * Time.deltaTime, -45f, 45f);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                currentAngleOffset = Mathf.Clamp(currentAngleOffset + angleChangeSpeed * Time.deltaTime, -45f, 45f);
            }
            else
            {
                currentAngleOffset = Mathf.Lerp(currentAngleOffset, 0f, Time.deltaTime * 5f);
            }

            _light.intensity = Mathf.Clamp01(charge);


            charge = charge - Time.deltaTime / chargeLifetime;
        }
        else
        {
            _light.intensity = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Battery"))
        {
            Debug.Log("FlashlightScript:" + other.tag);
            charge += 1.0f;
            Destroy(other.gameObject);
            GameEventSystem.EmitEvent(new GameEvent
            {
                type="Battery",
                toast = $"Ви знайшли батарейку. Заряд ліхтарика поповнено до {charge:F1}",
                sound = EffectsSounds.BatteryCollected,
            });
        }
        else if (other.gameObject.CompareTag("MiniBattery"))
        {
            Debug.Log("FlashlightScript:" + other.tag);
            charge += 0.5f;
            Destroy(other.gameObject);
            GameEventSystem.EmitEvent(new GameEvent
            {
                type = "MiniBattery",
                toast = $"Ви знайшли міні-батарейку. Заряд ліхтарика поповнено до {charge:F1}",
                sound = EffectsSounds.BatteryCollected,
            });
        }
    }

}
