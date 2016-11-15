using System;

namespace SelfMailer.Library
{
    internal interface IReportChildrenChange : IReportChange
    {
        // D’autre part, la signature de la méthode ChildChanged de l’interface IReportChildrenChange doit être modifiée dans l’interface et dans les types qui l’implémentent :
        void ChildChanged(object sender, ChangedEventArgs e);
    }
}
