public class LoadingOverlay : SingletonMonoBehaviour<LoadingOverlay>
{
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}