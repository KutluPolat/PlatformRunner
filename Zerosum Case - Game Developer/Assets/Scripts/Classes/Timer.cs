using UnityEngine;

public class Timer
{
    private float _actionDelay, _timer;

    public Timer(float actionDelay)
    {
        _actionDelay = actionDelay;
    }

    /// <summary>
    /// Adds Time.deltaTime untill the value reaches to the actionDelay.
    /// </summary>
    /// <returns>True if timer flag is dirty. False if timer flag is clean.</returns>
    public bool HandleTimer()
    {
        _timer += Time.deltaTime;
        if(_timer >= _actionDelay)
        {
            ResetTimer();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetTimer() => _timer = 0;
}
