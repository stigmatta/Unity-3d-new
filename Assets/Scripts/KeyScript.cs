using UnityEngine;
using UnityEngine.UI;

public class KeyScript : MonoBehaviour
{
    private GameObject content;
    private Image indicatorImage;
    [SerializeField] private int keyNumber;
    [SerializeField] private float timeout = 5f;
    [SerializeField] private string description = "відповідні";
    private float leftTime;
    private bool isInTime = true;
    void Start()
    {
        content = this.transform.Find("Content").gameObject;
        indicatorImage = this.transform.Find("Indicator/Canvas/Foreground").GetComponent<Image>();
        indicatorImage.fillAmount = 1f;
        leftTime = timeout;
    }

    void Update()
    {
        content.transform.Rotate(0, Time.deltaTime * 40f, 0);
        if (leftTime >= 0)
        {
            indicatorImage.fillAmount = leftTime / timeout;
            indicatorImage.color = new Color(
                 Mathf.Clamp01(2f * (1f - indicatorImage.fillAmount)),
                Mathf.Clamp01(2f * indicatorImage.fillAmount),
                0f);
            leftTime -= Time.deltaTime;
            if (leftTime < 0)
            {
                isInTime = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameEventSystem.EmitEvent(new GameEvent
            {
                type = $"isKey{keyNumber}Collected",
                payload = isInTime,
                toast = $"Ключ {keyNumber} знайдено. {description} двері відчинені",
                sound=isInTime?
                EffectsSounds.KeyCollectedInTime : EffectsSounds.KeyCollectedOutOfTime
            });

            Destroy(this.gameObject);
        }
    }
}
