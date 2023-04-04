namespace API_Population.Models
{
    public class Pays
    {
        // id du pays pour l'identifier de mamière unique
        public int Id { get; set; } 

        // Le nom du pays 
        public string Country { get; set; }

        // Le continent auquel il appartient 
        public string Continent { get; set; }

        //La liste de population de ce pays, nous permet d'avoir accés à la classe population
        public ICollection<Population> Populations { get; set; }
    }
}
