namespace innov_web.Models.DTO
{
    public class GroupCreateDto
    {
        public string Name { get; set; }
        public string DbType { get; set; }
        public ConnectionDto ConnectionDto { get; set; }
    }
}
