using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotManager.Webots {
    public class WeEntity {

        public class Data {

            public Color EntityColor;
            public string Name;
            public int ID;

        }

        public class Structures {

            public enum EntityCommands :byte {

                //Send
                ECMD_Tick,
                ECMD_AddFlag,
                ECMD_Stop,
                ECMD_Resume,

                //Recieve
                ECMD_RobotData,
                ECMD_FlagDelivered


            }








        }

    }
}
