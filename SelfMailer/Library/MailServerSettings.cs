using System;

namespace SelfMailer.Library
{
    public class MailServerSettings : IReportChange
    {
        protected bool hasChanged;

        public bool HasChanged
        {
            get { return hasChanged; }
            set
            {
                if (this.hasChanged != value)
                {
                    this.hasChanged = value;
                    if (this.Changed != null)
                        this.Changed(this, new ChangedEventArgs(this.HasChanged));
                }
            }
        }

        // Cette modification entraîne des erreurs de compilation. D’une part parce que les types qui implémentent l’interface IReportChange ne possèdent plus la bonne signature pour l’évènement Changed. Il faut donc remplacer la signature des évènements dans les classes qui implémentent l’interface IReportChange par :
        public event EventHandler<ChangedEventArgs> Changed;
    }
}
