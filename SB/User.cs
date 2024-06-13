using System;
using System.Collections.Generic;

namespace SB;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public byte[] ?Photo { get; set; }

    public string Phone { get; set; }

    public string City { get; set; }

    public int Age { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
