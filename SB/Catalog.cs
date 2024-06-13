using System;
using System.Collections.Generic;

namespace SB;

public partial class Catalog
{
    public int Id { get; set; }

    public string? Value { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
