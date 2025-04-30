using BioProjektModels;
using System.Collections.Generic;

namespace BioProjekt.Api.BusinessLogic
{
    public interface IAuditoriumService
    {
        List<Auditorium> GetAllAuditoriums();
    }
}
