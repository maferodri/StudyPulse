using StudyPulse.Domain.Entities;

namespace StudyPulse.Application.Interfaces;

public interface IStudyDetailRepository
{
    Task<IEnumerable<StudyDetail>> GetAllAsync();
    Task<StudyDetail?> GetByIdAsync(int id);
    Task<StudyDetail> AddAsync(StudyDetail studyDetail);
    Task <StudyDetail> UpdateAsync(StudyDetail studyDetail);
    Task DeleteAsync(StudyDetail studyDetail);
    
    Task<bool> ExistsByStudyIdAsync(int studyId);
    Task<bool> ExistsByStudyStatusIdAsync(int studyStatusId);
}