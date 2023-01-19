using System;

namespace TrollSmasher.DataTypes
{

    public class CountdownGameTimer
    {
        private DateTime gameStartedTime;
        private DateTime pauseStartTime;
        private DateTime pauseEndTime;
        private Int64 totalSeconds;
        private bool timerActive = false;
        private bool pauseState = false;

        public CountdownGameTimer(DateTime gameStart, Int64 playSeconds)
        {
            gameStartedTime = gameStart;
            totalSeconds = playSeconds;
        }

        public void Start()
        {
            timerActive = true;
        }

        public bool IsRunning()
        {
            return timerActive;
        }

        public Int64 TotalRunningTime(bool includePause = false)
        {
            Int64 res = 0;
            var timeDiff = gameStartedTime - DateTime.Now;
            return res;
        }

        public bool Pause(bool pause)
        {
            if (pause == pauseState)
                return false;
            else
            {
                pauseState = pause;
                if (pauseState)
                    pauseStartTime = DateTime.Now;
                else
                    pauseEndTime = DateTime.Now;
                return true;
            }
        }
    }
}
