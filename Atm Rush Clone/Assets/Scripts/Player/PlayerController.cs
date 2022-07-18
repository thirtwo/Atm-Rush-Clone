using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float limit;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private Transform player;
    [SerializeField] private LootController lootController;
    private InputHandler inputHandler;
    void Start()
    {
        inputHandler = GetComponent<InputHandler>();
    }

    void Update()
    {
        if (!GameManager.isGameStarted || GameManager.isGameFinished) return;
        float horizontal = 0;
        if (!IsLimitNeeded(player.position.x, inputHandler.SwerveInput, limit))
        {
            horizontal = horizontalSpeed * inputHandler.SwerveInput * Time.deltaTime;
            lootController.MoveLootOneByOne();
        }
        transform.Translate(new Vector3(0, 0, forwardSpeed * Time.deltaTime));
        player.Translate(new Vector3(horizontal, 0, 0));
    }
    private static bool IsLimitNeeded(float x, float input, float limit)
    {
        if (input == 0) return true;
        if (x < -limit && input < 0) return true;
        if (x > limit && input > 0) return true;
        return false;
    }
}
