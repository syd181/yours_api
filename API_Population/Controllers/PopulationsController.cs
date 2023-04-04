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
    public class PopulationsController : ControllerBase
    {
        private readonly API_PopulationContext _context;

        public PopulationsController(API_PopulationContext context)
        {
            _context = context;
        }

        // GET: api/Populations
        // Nous permet d'avoir la liste des populations presente dans la BDD
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Population>>> GetPopulation()
        {
          if (_context.Population == null)
          {
              return NotFound();
          }
            return await _context.Population.ToListAsync();
        }

        // GET: api/Populations/5
        // Avec cette methode on peut avoir la liste des populations d'un Pays spécifique en fournissant son Id
        [HttpGet("{paysId}")]
        public async Task<ActionResult<IEnumerable<Population>>> GetPopulation(int paysId)
        {

            var populations = await _context.Population
                .Where(p => p.PaysId == paysId)
                .ToListAsync();

            if (populations == null || populations.Count == 0)
  
            {
              return NotFound();
            }

            return populations;
        }


        // GET: api/Population/5/2022
        // Obtenir la population d'un pays d'une année donnée
        [HttpGet("{paysId}/{annee}")]
        public async Task<ActionResult<Population>> GetPopulation(int paysId, int annee)
        {
            var population = await _context.Population
                .SingleOrDefaultAsync(p => p.PaysId == paysId && p.Annee == annee);


            if (population == null)
            {
                return NotFound();
            }
            return population;

        }

        

        // Mettre juste dans le route le put sans param
        // PUT: api/Populations/5/2022
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/{annee}")]

        public async Task<IActionResult> PutPopulation(int id, int annee, [FromBody] Population population)
        {

            if (annee != population.Annee)
            {
                return BadRequest();
            }
            
            var existingPopulation = await _context.Population.FirstOrDefaultAsync(p => p.Id == id && p.Annee == annee );

            if (existingPopulation == null)
            {
                return NotFound();
            }

            existingPopulation.NbrHabitants = population.NbrHabitants;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PopulationExists(id, annee))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();


        }

        





        /*
        [HttpPut("{paysId}/{annee}")]
        public async Task<IActionResult> PutPopulation(int paysId, int annee, Population population)
        {
            if (paysId != population.PaysId || annee != population.Annee)
            {
                return BadRequest();
            }

            var existingPopulation = await _context.Population.FindAsync(paysId, annee);

            if (existingPopulation == null)
            {
                // Ajouter une nouvelle entité Population
                _context.Population.Add(population);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPopulation), new { paysId = population.PaysId, annee = population.Annee }, population);
            }
            else
            {
                // Mettre à jour l'entité Population existante
                _context.Entry(existingPopulation).CurrentValues.SetValues(population);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }


*/





        // Creer plusieurs population en meme temps 
        [HttpPost]
        public async Task<ActionResult<List<Population>>> PostPopulations(List<Population> populations)
        {
            if (populations == null || !populations.Any())
            {
                return BadRequest("La liste des populations ne peut pas être nulle ou vide.");
            }

            foreach (var population in populations)
            {
                if (population.Annee <0)
                {
                    return BadRequest("L'année doit etre supérieure à 0");
                }

                if(population.NbrHabitants <0)
                {
                    return BadRequest("Le nombre d'habitants doit etre supérieur ou égal à 0");
                }

                var pays =  await _context.Pays.FindAsync(population.PaysId);
                if(pays == null)
                {
                    return BadRequest("Le pays spécifié n'existe pas.");
                }

                population.PaysId = pays.Id;
                _context.Population.Add(population);

                
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPopulation), new { }, populations);
        }






        // DELETE: api/Populations/5/2022
        [HttpDelete("{paysId}/{annee}")]
        public async Task<IActionResult> DeletePopulation(int paysId, int annee)
        {
            var pays = await _context.Pays.FindAsync(paysId); 
            if (pays == null)

            {
                return NotFound();
            }

            var population = await _context.Population
                .SingleOrDefaultAsync(p => p.PaysId == pays.Id &&  p.Annee == annee);


            if (population == null)
            {
                return NotFound();
            }

            _context.Population.Remove(population);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PopulationExists(int paysId , int annee)
        {
            return _context.Population.Any(p => p.PaysId == paysId && p.Annee == annee);
        }
    }
}
