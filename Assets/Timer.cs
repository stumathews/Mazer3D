using UnityEngine;
using UnityEngine.UI;

// taken from https://answers.unity.com/questions/1179131/how-do-i-display-text-with-countdown-timer-minutes.html


namespace Assets
{
    public class Timer : MonoBehaviour
    {
        public int Minutes = 0;
        public int Seconds = 0;

        private string m_text;
        private float m_leftTime;

        

        private void Awake()
        {
            //m_text = GetComponent<Text>();
            m_leftTime = GetInitialTime();
        }


        private void Update()
        {
            if (m_leftTime > 0f)
            {
                //  Update countdown clock
                m_leftTime -= Time.deltaTime;
                Minutes = GetLeftMinutes();
                Seconds = GetLeftSeconds();

                //  Show current clock
                if (m_leftTime > 0f)
                {
                    Main.Time = "Time : " + Minutes + ":" + Seconds.ToString("00");
                }
                else
                {
                    //  The countdown clock has finished
                    Main.Time = "Time : 0:00";
                    Main.timeUp = true;
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