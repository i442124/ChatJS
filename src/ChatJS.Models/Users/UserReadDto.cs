using System;

namespace ChatJS.Models.Users
{
    public class UserReadDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string NameCaption { get; set; }
    }
}
