using System;
using System.Collections.Generic;

namespace SB;

public partial class Book
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Author { get; set; }

    public int? Price { get; set; }

    public int? Swap { get; set; }

    public int? IdCatalog { get; set; }

    public int? IdUser { get; set; }

    public string? Info { get; set; }

    public virtual ICollection<Galary> Galaries { get; set; } = new List<Galary>();

    public virtual Catalog? IdCatalogNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
