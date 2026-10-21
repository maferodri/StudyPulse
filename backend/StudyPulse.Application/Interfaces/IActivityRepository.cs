using StudyPulse.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace StudyPulse.Application.Interfaces;

public interface IActivityRepository
{
    Task<IEnumerable<Activity>> GetAllAsync();
    Task<Activity> AddAsync(Activity activity);
    //EL ? permite que el método devuelva un valor nulo
    Task<Activity?> GetByIdAsync(int id);
    Task<Activity> UpdateAsync(Activity activity);
    Task DeleteAsync(Activity activity);
    //Ya que la validación se necesita datos de la BD, y el repositorio los consultara 
    Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null);
}