using System;

namespace SelfMailer.Library
{
    public class ProjectSettings : IReportChange
    {
        protected bool hasChanged;

        public bool HasChanged
        {
            get { return hasChanged; }


            //La classe ProjectSettings déclenche l’évènement Changed à chaque fois que la valeur de la variable hasChanged est modifiée. Avant de déclencher l’évènement, il faut s’assurer qu’il n’est pas null, en d’autres termes, on s’assure que l’évènement possède des souscripteurs. Si vous essayez de déclencher un évènement qui n’a pas de souscripteur, une exception sera levée à l’exécution de l’application.

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

        public event EventHandler<ChangedEventArgs> Changed;
    }
}
