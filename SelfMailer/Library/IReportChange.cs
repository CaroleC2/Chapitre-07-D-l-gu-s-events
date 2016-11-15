using System;

namespace SelfMailer.Library
{
    public interface IReportChange
    {
        bool HasChanged
        {
            get;
            set;
        }

        // 1 - La manière la plus simple de déclarer un évènement est d’ajouter le mot-clé event devant un membre délégué. L’évènement Changed dans l’interface IReportChange a été créé précédemment de cette manière :
        // 2 - Modifiez la déclaration de l’évènement Changed dans l’interface IReportChange comme suit :
        event EventHandler<ChangedEventArgs> Changed;
    }
}
