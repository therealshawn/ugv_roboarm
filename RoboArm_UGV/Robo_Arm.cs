using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dynamixel_sdk;

namespace RoboArm_UGV
{
    class Robo_Arm
    {
       

        
        /// <summary>
        /// Driver function to test class methods
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //PID_Contoller elbow_pi = new PID_Contoller(0.5, 0.1, 0.6, 200, -150);  // random values. Do not use.
            Robo_Arm test = new Robo_Arm();
            DXL_Servo servo = new DXL_Servo();
            

        }


        
    }
}
