using API_Population.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.Metrics;
using System.Linq;

namespace API_Population.Models
{
    public static class SeedData // Creer la base puis ajouter quelques pays 
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new API_PopulationContext(
                serviceProvider.GetRequiredService<DbContextOptions<API_PopulationContext>>()))
            {
                // ca va nous permettre de supprimer la BDD à chaque fois qu'on compile 
                
                context.Database.EnsureDeleted();

                // ca creer la base de donnee 
                context.Database.EnsureCreated();
                
                // Vérifiez si la base de données contient déjà des pays et des populations
                if (context.Pays.Any())
                {
                    return;   // La base de données a déjà été pré-remplie donc on ne fait rien
                }

                // Sinon Ajoutez des pays à la base de données, j'ai ajouté quelques uns par continents

                context.Pays.AddRange(

                    new Pays

                    {
                        Country = "Benin",
                        Continent = "Afrique",
                        Populations = new List<Population>
                        {
                            
                            new Population { Annee = 2000, NbrHabitants = 5 },
                            new Population { Annee = 2010, NbrHabitants = 15 },
                            new Population { Annee = 2020, NbrHabitants = 31 }
                        }
                        

    

                    }

                    );


                // On va creer maintenant la table population 

                if (context.Population.Any())
                {
                    return;   // La base de données a déjà été pré-remplie donc on ne fait rien
                }

                // Sinon Ajoutez des Populations à la base de données

                context.Population.AddRange(

                    new Population

                    {
                        Annee = 1960,
                        NbrHabitants = 2,
                        PaysId = 1

                    }
                    );





                context.SaveChanges();










            }

        }
    }

}
