using AdoptApi.Database;
using AdoptApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AdoptApi.Repositories;

public class PetRepository
{
    private Context _context;

    public PetRepository(Context context)
    {
        _context = context;
    }

    public async Task<Pet> GetPetById(int id)
    {
        return await _context.Pets.Include(nameof(Pet.Pictures)).Include(nameof(Pet.Needs)).SingleAsync(p => p.Id == id);
    }

    public async Task<Pet> CreatePet(Pet pet)
    {
        await _context.Pets.AddAsync(pet);
        await _context.SaveChangesAsync();
        return pet;
    }

    public async Task<List<Need>> GetAvailableNeeds()
    {
        return await _context.Needs.Where(n => n.IsActive == true).ToListAsync();
    }

    public async Task<List<Need>> GetAvailableNeedsByIds(int[] ids)
    {
        return await _context.Needs.Where(n => ids.Contains(n.Id) && n.IsActive).ToListAsync();
    }

    public async Task<Pet> GetAvailablePet(int petId)
    {
        return await _context.Pets.Include(nameof(Pet.Pictures)).Include(nameof(Pet.Needs)).Where(p => p.IsActive == true && p.Id == petId).SingleAsync();
    }

    public async Task<List<Pet>> GetRegisteredPets(int userId)
    {
        return await _context.Pets.Include(nameof(Pet.Pictures)).Include(nameof(Pet.Needs)).Where(p => p.UserId == userId).OrderByDescending(p => p.Id).ToListAsync();
    }
}