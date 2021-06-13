
/// <summary>
/// A Timer object that tracks its own state
/// </summary>
public class Timer
{
    public float duration = 1.0f;
    public bool isActive;

    float timer = 0f;

    public Timer(float _duration)
    {
        duration = _duration;
        isActive = false;
    }

    public void StartTimer()
    {
        isActive = true;
        timer = 0f;
    }

    public void Tick(float delta)
    {
        if (isActive)
            timer += delta;

        if (timer >= duration)
        {
            isActive = false;
        }
    }

    public bool IsDone()
    {
        return !isActive;
    }
}