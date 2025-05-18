using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BioProjekt.Shared.ClientDtos
{
    public class ScreeningCreateDto
    {
        public DateTime Date {  get; set; }
        public TimeSpan Time { get; set; }
        public string LanguageVersion { get; set; }
        public bool Is3D { get; set; }
        public string SoundSystem { get; set; }
        public int AuditoriumId { get; set; }
    }
}
