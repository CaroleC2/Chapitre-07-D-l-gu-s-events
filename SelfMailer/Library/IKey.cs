namespace SelfMailer.Library
{
    // La liste va contenir de nombreux éléments et il faut donc implémenter une technique plus élaborée pour les différencier et assurer l’unicité de ceux-ci. Nous allons créer un indexeur basé sur une clé de type string. Cela implique dans un premier temps de créer une interface IKey dans le dossier Library :
    public interface IKey
    {
        string Key
        {
            get;
        }
    }
}
