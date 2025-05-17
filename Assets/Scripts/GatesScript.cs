using UnityEngine;

public class GatesScript : MonoBehaviour
{
    [SerializeField] Vector3 openDirection;
    [SerializeField] private int keyNumber;
    [SerializeField] private float size = 0.68f;
    private float openTime;
    private float openTime1 = 3.0f;
    private float openTime2 = 10.0f;
    private bool isKeyInserted;
    private bool isKeyCollected;
    private bool isKeyInTime;
    private bool isOpened =false;
    private int hitCount;
    private AudioSource openingSound1;
    private AudioSource openingSound2;

    void Start()
    {
        isKeyInserted = false;
        isKeyCollected = false;
        isKeyInTime = true;
        hitCount = 0;
        AudioSource[] openingSounds = GetComponents<AudioSource>();
        if (openingSounds.Length > 0)
        {
            openingSound1 = openingSounds[0];
            if (openingSounds.Length > 1)
            {
                openingSound2 = openingSounds[1];
            }
        }
        GameEventSystem.Subscribe(OnGameEvent);
        GameState.AddListener(OnGameStateChanged);

    }
    void Update()
    {
        if (!isOpened && isKeyInserted && transform.localPosition.magnitude < size)
        {
            transform.Translate(size * Time.deltaTime / openTime * openDirection);
            if (transform.localPosition.magnitude >= size)
            {
                isOpened = true;
                (isKeyInTime ? openingSound1 : openingSound2).Stop();
            }

        }

        if (openingSound1 != null && openingSound1.isPlaying)
        {
            openingSound1.volume = Time.timeScale == 0 ? 0 : GameState.longEffectsVolume;
        }
        if (openingSound2 != null && openingSound2.isPlaying)
        {
            openingSound2.volume = Time.timeScale == 0 ? 0 : GameState.longEffectsVolume;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (isKeyCollected)
            {
                if (!isKeyInserted)
                {
                    isKeyInserted = true;
                    openTime = isKeyInTime ? openTime1 : openTime2;

                    AudioSource soundToPlay = isKeyInTime ? openingSound1 : openingSound2;
                    if (soundToPlay != null)
                    {
                        soundToPlay.Play();
                    }
                }

            }
            else
            {
                hitCount += 1;
                if (hitCount == 1)
                {
                    GameEventSystem.EmitEvent(new GameEvent
                    {
                        type = "PlayerHitGates",
                        toast = $"Знайдіть ключ {keyNumber} для відчинення"
                    });
                }
                else
                {
                    GameEventSystem.EmitEvent(new GameEvent
                    {
                        type = "PlayerHitGates",
                        payload = hitCount,
                        toast = $"Я кажу {hitCount}й раз, знайдіть ключ {keyNumber} для відчинення"
                    });
                }
            }
        }

        Debug.Log(collision.gameObject.name);
    }

    private void OnGameEvent(GameEvent gameEvent) {
        if(gameEvent.type == $"isKey{keyNumber}Collected")
        {
            isKeyCollected = true;
            isKeyInTime = (bool)gameEvent.payload;
        }
    }
    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == null || fieldName == nameof(GameState.longEffectsVolume))
        {
            if(openingSound1!=null)
                openingSound1.volume = GameState.longEffectsVolume;
            if (openingSound2 != null)
                openingSound2.volume = GameState.longEffectsVolume;
        }
    }
    private void OnDestroy()
    {
        GameEventSystem.Unsubscribe(OnGameEvent);
        GameState.RemoveListener(OnGameStateChanged);
    }
}
