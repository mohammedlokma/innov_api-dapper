﻿namespace innov_web.Models.DTO
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DbType { get; set; }
        public bool Deleted { get; set; }
        public ConnectionDto Connection { get; set; }
    }
}
