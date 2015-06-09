using System;
using System.Text;

namespace PixelGrinch
{
    public abstract class PixelGrincher
    {
        public abstract void Interface();
        public abstract void Load();
        public abstract void Process_Images();
        public abstract void Save_Images();
        public abstract void Dispose();
    }
}
