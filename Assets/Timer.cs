using System;
using UnityEngine;
using UnityEngine.UI;

// taken from https://answers.unity.com/questions/1179131/how-do-i-display-text-with-countdown-timer-minutes.html


namespace Assets
{
    public class Timer : MonoBehaviour
    {
        private int Minutes;
        private int Seconds;
        

        private string m_text;
        private float m_leftTime;
        private bool timerStopped = false;

        public void ApplyTimeDisadvantage()
        {
            if (m_leftTime > 10)
                m_leftTime -= Main.digTimeDisadvantage;
            TimeUp = true;
            TimeString = "Time is up!";
        }
        public string TimeString { get; set; }
        public bool TimeUp { get; set; }


        public Timer(int mins, int secs)
        {
            TimeString = String.Empty;
            TimeUp = false;
            Minutes = mins;
            Seconds = secs;
            m_leftTime = GetInitialTime();
        }

        /// <summary>
        ///   <para>Returns the name of the game object.</para>
        /// </summary>
        public override string ToString()
        {
            return TimeString;
        }

        public void StopTimer()
        {
            timerStopped = true;
        }


        public void Update()
        {
            if (timerStopped)
                return;
            if (m_leftTime > 0f)
            {
                //  Update countdown clock
                m_leftTime -= Time.deltaTime;
                Minutes = GetLeftMinutes();
                Seconds = GetLeftSeconds();

                //  Show current clock
                if (m_leftTime > 0f)
                {
                    TimeString = "Time : " + Minutes + ":" + Seconds.ToString("00");
                    TimeUp = false;
                }
                else
                {
                    //  The countdown clock has finished
                    TimeString = "Time : 0:00";
                    TimeUp = true;
                }
            }
        }

        private float GetInitialTime()
        {
            return Minutes * 60f + Seconds;
        }

        private int GetLeftMinutes()
        {
            return Mathf.FloorToInt(m_leftTime / 60f);
        }

        private int GetLeftSeconds()
        {
            return Mathf.FloorToInt(m_leftTime % 60f);
        }
    }
}