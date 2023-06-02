using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avrora.Core.Settings.ChatSettings
{
    public class ChatSettings
    {
        private string path_fileChat;

        public ChatSettings(string path_fileChat)
        {
            this.path_fileChat = path_fileChat + "chat.json";

            try
            {
                using (FileStream stream = new FileStream(this.path_fileChat, FileMode.Open)) { }
            }
            catch (FileNotFoundException)
            {
                Serializer();
            }

            Deserialize();
        }

        private void Serializer()
        {

        }

        private void Deserialize()
        {

        }
    }
}
