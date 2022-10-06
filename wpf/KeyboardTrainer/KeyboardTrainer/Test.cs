using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardTrainer
{
    class Test
    {
        public bool IsStarted { get; set; } // button start was pressed but we didn't type
        public bool IsGoing { get; set; } // button start was pressed and we typed
        public int Chars { get; set; }
        public int Fails { get; set; }
        public DateTime startTime { get; set; }
        public int CharsPerMin  // chars per minut
        { 
            get
            {
                double t = (DateTime.Now - startTime).TotalSeconds ;
                int res = (int)((Chars * 60) / (DateTime.Now - startTime).TotalSeconds);
                if(res < 0) // for first symbol
                {
                    return 0;
                }
                return res;
            }
        }

        public void Stop()
        {
            IsStarted = false;
            IsGoing = false;
            Chars = 0;
            Fails = 0;
        }
    }
}
