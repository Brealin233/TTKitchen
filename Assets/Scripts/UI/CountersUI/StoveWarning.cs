using UnityEngine;

public class StoveWarning : MonoBehaviour
{
    // todo: warning sound
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private Transform stoveWarningIcon;

    private Animator stoveWarningAnim;
    [SerializeField] private Animator stoveWarningIconAnim;
    private bool isWarning;

    private const string STOVE_WARNING = "STOVE_WARNING";
    private const string STOVE_WARNING_ICON = "STOVE_WARNING_ICON";

    private void Awake()
    {
        stoveWarningAnim = GetComponent<Animator>();
    }

    private void Start()
    {
        Hide();
        stoveCounter.counterVisualEvent += OnCounterVisualEvent;

        stoveWarningAnim.SetBool(STOVE_WARNING, false);
        stoveWarningIconAnim.SetBool(STOVE_WARNING_ICON, false);
    }

    private void OnCounterVisualEvent(object sender, IWasVisualCounter.counterVisualEventClass e)
    {
        float warningVisualTime = .3f;
        bool show = e.fillAmount >= warningVisualTime && stoveCounter.IsFried();
        Show(show);

        stoveWarningAnim.SetBool(STOVE_WARNING, show);
        stoveWarningIconAnim.SetBool(STOVE_WARNING_ICON, show);
    }

    private void Show(bool isShow)
    {
        if (isShow)
            stoveWarningIcon.gameObject.SetActive(true);
    }

    private void Hide()
    {
        stoveWarningIcon.gameObject.SetActive(false);
    }
}