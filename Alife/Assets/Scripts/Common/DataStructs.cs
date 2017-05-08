using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Common
{

    public struct VoiceStruct
    {
        public int ID;
        public List<int> voiceMessage;

        public VoiceStruct(int id, List<int> vm)
        {
            ID = id;
            voiceMessage = vm;
        }
    }
}
