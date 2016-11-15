using System;

namespace SelfMailer.Library
{

    //Créez une nouvelle classe ChangedEventArgs dérivant de la classe de base EventArgs et exposant un membre HasChanged :
    public class ChangedEventArgs : EventArgs
    {
        public bool HasChanged { get; protected set; }
        public ChangedEventArgs(bool hasChanged)
        {
            this.HasChanged = hasChanged;
        }
    }
}
