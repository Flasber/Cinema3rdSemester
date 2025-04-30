using BioProjektModels;
using System.Collections.Generic;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IScreeningService
    {
        List<Screening> GetAllScreenings();
        void AddScreening(Screening screening);
    }
}
