namespace API_Population.Models
{
    public class Population
    {
        // id qui identifie une population de facon unique
        public int Id { get; set; }

        // Variable annee qui est un int
        public int Annee { get; set; }

        // Le nombre d'habitant enrégistré au cours de cette année la 
        public int NbrHabitants { get; set; }

        // int PaysId qui represente la clé étrangère et cela va beaucoup nous aider 
        public int PaysId { get; set; }
        

    }
}
