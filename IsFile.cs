using Avrora.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Avrora
{
    public enum FileIs
    {
        File = 0,
        Photo = 1,
    }
    public static class IsFile
    {
        private static Collection<string> collectionForPhotoExt = new Collection<string>() { ".png", ".jpg"};
        public static FileIs Is(string name)
        {
            string ext = Path.GetExtension(name);

            if (collectionForPhotoExt.Contains(ext))
                return FileIs.Photo;

            return FileIs.File;
        }

        public static IsSendMessage ConvertInIsSendMessage(FileIs fileis)
        {
            switch (fileis)
            {
                case FileIs.Photo:
                    return IsSendMessage.Photo;
            }

            return IsSendMessage.File;
        }
    }
}
