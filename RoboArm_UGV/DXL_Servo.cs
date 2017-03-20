using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dynamixel_sdk;

namespace RoboArm_UGV
{
    class DXL_Servo
    {
        #region Private Properties
        #region Constants
        //********* SDK Components ************//
        // Control table address
        private const int ADDR_MX_TORQUE_ENABLE = 24;                  // Control table address is different in Dynamixel model
        private const int ADDR_MX_GOAL_POSITION = 30;
        private const int ADDR_MX_PRESENT_POSITION = 36;

        // Protocol version
        private const int PROTOCOL_VERSION = 1;                   // See which protocol version is used in the Dynamixel

        // Default setting
        private Int16 DXL_ID = 001;                 // Dynamixel ID: 1
        private const int BAUDRATE = 8000;
        private const string DEVICENAME = "COM8";      // Check which port is being used on your controller

        private const int TORQUE_ENABLE = 1;                   // Value for enabling the torque
        private const int TORQUE_DISABLE = 0;                   // Value for disabling the torque
        private int DXL_MINIMUM_POSITION_VALUE = -1000;                 // Dynamixel will rotate between this value
        private int DXL_MAXIMUM_POSITION_VALUE = 400;                // and this value (note that the Dynamixel would not move when the position value is out of movable range. Check e-manual about the range of the Dynamixel you use.)
        private int DXL_MOVING_STATUS_THRESHOLD = 10;                  // Dynamixel moving status threshold

        private const byte ESC_ASCII_VALUE = 0x1b;

        private const int COMM_SUCCESS = 0;                   // Communication Success result value
        private const int COMM_TX_FAIL = -1001;               // Communication Tx Failed
        #endregion Constants
        // class properties
        private int dxl_comm_result = COMM_TX_FAIL;
        private Int16 dxl_present_position = 0;         // chang to what the starting position will be
        private Int16[] dxl_present_positions { get; set; } = new Int16[3];     // initialize the with the start values from testing
        private int port_num = 0;
        private byte dxl_error = 0;
        #endregion Private Properties
        #region Constructors
        // class constructors
        public DXL_Servo()
        {

        }

        // destructor
        ~DXL_Servo()
        {
            // Disable Dynamixel Torque
            dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL_ID, ADDR_MX_TORQUE_ENABLE, TORQUE_DISABLE);
            if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            {
                dynamixel.printTxRxResult(PROTOCOL_VERSION, dxl_comm_result);
            }
            else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
            {
                dynamixel.printRxPacketError(PROTOCOL_VERSION, dxl_error);
            }

            // Close port
            dynamixel.closePort(port_num);
        }
        #endregion Constructors
        #region Methods
        /// <summary>
        /// Open the com port and set the baudrate
        /// </summary>

        private void setup_port()
        {
            // Initialize PortHandler Structs
            // Set the port path
            // Get methods and members of PortHandlerLinux or PortHandlerWindows
            port_num = dynamixel.portHandler(DEVICENAME);

            // Initialize PacketHandler Structs
            dynamixel.packetHandler();

            // Open port
            if (dynamixel.openPort(port_num))
            {
                Console.WriteLine("Succeeded to open the port!");
            }
            else
            {
                Console.WriteLine("Failed to open the port!");
                Console.WriteLine("Press any key to terminate...");
                Console.ReadKey();
                return;
            }

            // Set port baudrate
            if (dynamixel.setBaudRate(port_num, BAUDRATE))
            {
                Console.WriteLine("Succeeded to change the baudrate!");
            }
            else
            {
                Console.WriteLine("Failed to change the baudrate!");
                Console.WriteLine("Press any key to terminate...");
                Console.ReadKey();
                return;
            }

            // Enable Dynamixel Torque
            dynamixel.write1ByteTxRx(port_num, PROTOCOL_VERSION, DXL_ID, ADDR_MX_TORQUE_ENABLE, TORQUE_ENABLE);
            if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            {
                dynamixel.printTxRxResult(PROTOCOL_VERSION, dxl_comm_result);
            }
            else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
            {
                dynamixel.printRxPacketError(PROTOCOL_VERSION, dxl_error);
            }
            else
            {
                Console.WriteLine("Dynamixel has been successfully connected");
            }
        }

        public bool Move_To(Int16 myId, Int16 my_goal_posi)
        {
            // Write a check to make sure the goal position is within range and allowable

            // Write goal position
            dynamixel.write2ByteTxRx(port_num, PROTOCOL_VERSION, myId, ADDR_MX_GOAL_POSITION, my_goal_posi);
            if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
            {
                dynamixel.printTxRxResult(PROTOCOL_VERSION, dxl_comm_result);
                return false;
            }
            else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
            {
                dynamixel.printRxPacketError(PROTOCOL_VERSION, dxl_error);
                return false;
            }

            do
            {
                // Read present position
                dxl_present_position = dynamixel.read2ByteTxRx(port_num, PROTOCOL_VERSION, DXL_ID, ADDR_MX_PRESENT_POSITION);
                if ((dxl_comm_result = dynamixel.getLastTxRxResult(port_num, PROTOCOL_VERSION)) != COMM_SUCCESS)
                {
                    dynamixel.printTxRxResult(PROTOCOL_VERSION, dxl_comm_result);
                }
                else if ((dxl_error = dynamixel.getLastRxPacketError(port_num, PROTOCOL_VERSION)) != 0)
                {
                    dynamixel.printRxPacketError(PROTOCOL_VERSION, dxl_error);
                }

                Console.WriteLine("[ID: {0}] GoalPos: {1}  PresPos: {2}", DXL_ID, my_goal_posi, dxl_present_position);

            } while ((Math.Abs(my_goal_posi - dxl_present_position) > DXL_MOVING_STATUS_THRESHOLD));

            return true;
        }

        public void Find_Payload()
        {
            // turn base towards the front face of the UGV

            // raise end-effecter and get payload in frame

            // center payload in the frame

            // move towards payload

            // grab payload

            // retract arm

        }
        #endregion Methods

    }
}
