using System;
using System.Collections;
using System.Collections.Generic;

namespace SelfMailer.Library
{
    // 1 - Créez une nouvelle classe ReportChangeList<T> dans le dossier Library du projet :
    // 2 - Ajoutez l’interface IReportChildrenChange dans la déclaration de la classe :
    // 3 - Ajoutez une contrainte sur l’interface IReportChange sur la classe générique ReportChangeList :
    // 4 - Comme pour les listes, il serait intéressant de pouvoir effectuer une boucle foreach sur les éléments de notre type générique. Pour cela il suffit d’implémenter l’interface générique IEnumerable<T> :
    // 5 - Dans un second temps, ajoutez une contrainte sur la classe ReportChangeList de manière à obliger les types à implémenter l’interface IKey :
    public class ReportChangeList<T> : IReportChildrenChange, IEnumerable<T> where T : IReportChange, IKey
    {
        //La classe peut contenir des membres qui vont utiliser le type spécifié à l’instanciation grâce au paramètre T. 
        //Ajoutez le membre children de type List<T> à la classe :
        //Le paramètre T accepte un type qui sera spécifié au moment de l’instanciation :
        protected List<T> children;
        protected bool hasChanged;

        // Puisque la classe hérite de l’interface IReportChildrenChange, il faut ajouter les membres définis dans cette interface :
        public bool HasChanged
        {
            // Dans le code précédent, les accesseurs de la propriété HasChanged réalisent une boucle sur les éléments de la liste children afin de faire la mise à jour des objets pour l’accesseur set, ou de déterminer si l’objet lui-même ou l’un des éléments de la liste a été modifié pour l’accesseur get.
            get
            {
                bool result = false;
                foreach (IReportChange child in this.children)
                    if (child.HasChanged)
                    {
                        result = true;
                        break;
                    }
                return this.hasChanged || result;
            }

             // Pour déterminer si l’objet a été modifié, le principe est que si au moins un élément de la liste a été modifié ou l’objet lui même, l’objet entier est considéré comme ayant été modifié. La mise à jour de la propriété HasChanged des éléments de la liste ne se fait que si l’objet reçoit la valeur false pour sa propriété HasChanged car l’objet peut être modifié mais pas ses enfants alors que si les enfants sont modifiés, l’objet parent est par conséquent modifié puisque les enfants font partie de l’objet.
            set
            {
                if (this.HasChanged != value)
                {
                    this.hasChanged = value;
                    if (this.Changed != null)
                        this.Changed(this, new ChangedEventArgs(this.HasChanged));
                }
                if (!value)
                {
                    foreach (IReportChange child in this.children)
                        child.HasChanged = value;
                }
            }
        }

        // Ajoutez un indexeur à la classe ReportChangeList :
        // Nous avons créé un indexeur basé sur un paramètre de type string. Pour l’accesseur get, ce paramètre est comparé avec tous les éléments de la liste afin de déterminer si l’un d’eux contient la même clé et ainsi le retourner. Si l’élément n’est pas trouvé, la valeur par défaut lui sera retournée. L’accesseur set fonctionne de la même manière mais lorsque l’élément a été trouvé, il est mis à jour.
        public T this[string Key]
        {
            get
            {
                foreach (T aChild in this.children)
                {
                    if (((IKey)aChild).Key.Equals(Key))
                    {
                        return aChild;
                    }
                }

                // En étudiant de près l’accesseur get de l’indexeur 
                //de la classe ReportChangeList, vous pouvez remarquer 
                //que si aucun élément de la liste ne correspond à la 
                // clé passée en paramètre, la valeur de retour est :

                return default(T);

                // Un type générique peut concerner un type valeur ou un 
                // type référence, les types valeur n’étant pas nullable 
                //(ne pouvant pas avoir de valeur null) il est impossible de 
                //retourner null pour l’accesseur get. Le mot-clé default est
                //utilisé pour obtenir la valeur par défaut du paramètre type. 
                //Ainsi pour un paramètre de type référence, la valeur null sera retournée 
                //et pour un type valeur, il s’agira de sa valeur par défaut. 
                //Si T représente le type int, la valeur par défaut retournée sera zéro.
            }
            set
            {
                for (int i = 0; i < this.children.Count; i++)
                {
                    IKey aChild = (IKey)this.children[i];
                    if (aChild.Key.Equals(Key))
                    {
                        this.children[i] = value;
                        this.HasChanged = true;
                        break;
                    }
                }
            }
        }

        public event EventHandler<ChangedEventArgs> Changed;

        //Comme List<T> est un type référence, il doit être instancié dans le constructeur pour ne pas être null.
        //Ajoutez le constructeur qui instancie la variable children :
        public ReportChangeList()
        {
            this.children = new List<T>();
        }

        public void ChildChanged(object sender, ChangedEventArgs e)
        {
            if (this.Changed != null)
                this.Changed(sender, e);
        }

        // 1 - Ajoutez la méthode Add à la classe ReportChangeList :
        // 2 - La méthode Add prend en paramètre un type répondant aux
        //contraintes de la classe. L’objet passé en paramètre à la
        //méthode implémente donc l’interface IReportChange et il peut
        // être converti dans le type de l’interface ce qui permet à 
        //l’objet de type ReportChangeList de s’abonner à l’évènement 
        //Changed de l’objet passé en paramètre avant de l’ajouter 
        //à la liste children.
        // 3 -Il n’y a plus qu’à modifier la méthode Add pour déterminer que 
        // la clé est unique parmi les éléments de la liste avant de faire l’ajout : 

        public void Add(T Child)
        {
            IKey childKey = (IKey)Child;
            if (this[childKey.Key] == null)
            {
                IReportChange child = (IReportChange)Child;
                child.Changed += new EventHandler<ChangedEventArgs>(ChildChanged);
                this.children.Add(Child);
            }
        }

        // Afin de compléter la classe, il manque une méthode de 
        //suppression des éléments. La liste children étant protégée, 
        //les méthodes de suppression d’un élément de liste ne sont
        //pas disponibles.

        //Créez une méthode Remove acceptant un paramètre de type
        // string qui sera comparé aux clés des éléments de la liste 
        // afin de supprimer l’élément correspondant :


        public void Remove(string Key)
        {
            if (this[Key] != null)
            {
                this.children.Remove(this[Key]);
                this.HasChanged = true;
            }
        }

        // Ajoutez les membres GetEnumerator() et IEnumerable.GetEnumerator() suivant pour implémenter l’interface dans la classe générique ReportChangeList :
        // L’implémentation d’un énumérateur consiste à effectuer une boucle sur les éléments de la liste et à retourner chacun d’eux à l’appelant. Le mot-clé yield return permet de faire un retour à l’appelant avec la valeur à retourner en fonction de la précédente valeur retournée. L’état de la méthode est maintenu de telle manière qu’elle peut continuer son exécution au prochain appel. La durée de vie de cet état est liée à l’énumérateur, l’état de la méthode est supprimé lorsque l’énumération est terminée.
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.children.Count; i++)
            {
                yield return this.children[i];
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
