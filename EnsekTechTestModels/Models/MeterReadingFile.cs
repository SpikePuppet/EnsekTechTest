using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnsekTechTestModels.Models
{
    /// <summary>
    /// Not a DB entity
    /// </summary>
    public class MeterReadingFile
    {
        public byte[] Content { get; set; }
    }
}