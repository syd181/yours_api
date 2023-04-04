using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Population.Data;
using API_Population.Models;

namespace API_Population.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaysController : ControllerBase
    {
        private readonly API_PopulationContext _context;

        public PaysController(API_PopulationContext context)
        {
            _context = context;
        }

        // GET : api/Pays 
        // Cette methode GET va nous permettre d'afficher tout les pays de la BDD donc elle retourne une liste de Pays
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pays>>> GetPaysWithPopulations()
        {
            var paysWithPopulations = await _context.Pays
                .Include(p => p.Populations)
                .ToListAsync();

            if (paysWithPopulations == null)
            {
                return NotFound();
            }

            return paysWithPopulations;
        }


        
        // GET: api/Pays/5
        // On va pouvoir avec cette méthode récupérer juste un seul pays en fournissant son id 
        [HttpGet("{id}")]
        public async Task<ActionResult<Pays>> GetPays(int id)
        {
            var pays = await _context.Pays.Include(p => p.Populations).Where(p => p.Id == id).FirstOrDefaultAsync();

            if (pays == null)
            {
                return NotFound();
            }

            return pays;
        }


        // GET: api/Population/afrique/2022
        // Obtenir la population d'un continent d'une année donnée
        [HttpGet("{continent}/{annee}/pays")]

        public async Task<ActionResult<int>> GetPopulationContinent(string continent, int annee)
        {
            var pays = await _context.Pays
                .Include(p => p.Populations)
                .Where(p => p.Continent == continent)
                .ToListAsync();

            int sum = 0;

            foreach (var population in pays.SelectMany(p => p.Populations).Where(p => p.Annee == annee))
            {
                sum += population.NbrHabitants;
            }

            return sum;
        }




        // On pourra creer des pays en donnant dans postman le nom et le continent et les informations sur la population
        [HttpPost]
        public async Task<IActionResult> PostPays(IEnumerable<Pays> paysList)
        {
            if (paysList == null || !paysList.Any())
            {
                //return BadRequest("La liste des pays ne peut pas être vide.");
                return NotFound();
            }

            foreach (var pays in paysList)
            {
                if (pays.Populations == null || !pays.Populations.Any())
                {
                    return NotFound();
                    //return BadRequest("La liste des populations pour le pays " + pays.Country + " ne peut pas être vide.");
                }

                foreach (var population in pays.Populations)
                {
                    population.PaysId = pays.Id; // pour affecter l'Id du pays à chaque population correspondante
                }
            }

            _context.Pays.AddRange(paysList);
            await _context.SaveChangesAsync();

            return NoContent();//CreatedAtAction(nameof(GetPays), new { }, paysList);
            //NoContent();
        }


        /*
         * Frist

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPays(int id, Pays pays)
        {
            if (id != pays.Id)
            {
                return BadRequest("L'id du pays n'ai pas correct");
            }

            var existingPays = await _context.Pays.Include(p => p.Populations)
                                                  .FirstOrDefaultAsync(p => p.Id == id);
            if (existingPays == null)
            {
                return NotFound();
            }

            existingPays.Country = pays.Country;
            existingPays.Continent = pays.Continent;

            if (pays.Populations != null)
            {
                if (existingPays.Populations== null)
                {
                    existingPays.Populations = new List<Population>();
                }

                foreach (var population in pays.Populations)
                {
                    var existingPopulation = existingPays.Populations.FirstOrDefault(p => p.Id == population.Id);
                    if (existingPopulation == null)
                    {
                        existingPays.Populations.Add(population);
                    }
                    else
                    {
                            existingPopulation.Annee = population.Annee;
                            existingPopulation.NbrHabitants = population.NbrHabitants;
                    }
                    
                }
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }
        */

       
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPays(int id, Pays pays)
        {
            if (id != pays.Id)
            {
                return BadRequest("L'id du pays n'ai pas correct");
            }

            var existingPays = await _context.Pays.Include(p => p.Populations)
                                                  .FirstOrDefaultAsync(p => p.Id == id);
            if (existingPays == null)
            {
                return NotFound();
            }

            existingPays.Country = pays.Country;
            existingPays.Continent = pays.Continent;

            if (pays.Populations != null && pays.Populations.Any())
            {
                foreach (var population in pays.Populations)
                {
                    var existingPopulation = existingPays.Populations.FirstOrDefault(p => p.Id == population.Id);
                    if (existingPopulation == null)
                    {
                        var newPopulation = new Population { Annee = population.Annee, NbrHabitants = population.NbrHabitants };
                        existingPays.Populations.Add(newPopulation);
                    }
                    else
                    {
                            existingPopulation.Annee = population.Annee;
                            existingPopulation.NbrHabitants = population.NbrHabitants;
                        

                    }

                }
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/Pays/5
        // Methode qui va supprimer de la table un pays grace à son id 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePays(int id)
        {
            if (_context.Pays == null)
            {
                return NotFound();
            }
            var pays = await _context.Pays.FindAsync(id);
            if (pays == null)
            {
                return NotFound();
            }

            _context.Pays.Remove(pays);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool PaysExists(int id)
        {
            return (_context.Pays?.Any(e => e.Id == id)).GetValueOrDefault();
        }

      
        /* 
         * Conclusion : Le CRUD est bon pour L'API Pays 
         * Test réalisé le 13/03/23
         * Sydney A. + jeff et junior
         */

        
    }
}
