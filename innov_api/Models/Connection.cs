using System.ComponentModel.DataAnnotations.Schema;

namespace innov_api.Models
{
    public class Connection
    {
        public int  Id { get; set; }
        public string Server { get; set; }
        public string DbName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool IntegratedSecurity { get; set; }
        public bool TrustedConnection { get; set; }

        [ForeignKey("GroupId")]
        public Group  Group { get; set; }
        public int GroupId { get; set; }
    }
}
