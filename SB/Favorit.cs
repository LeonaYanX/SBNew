using System;
using System.Collections.Generic;

namespace SB;

public partial class Favorit
{
    public int? IdBook { get; set; }

    public int? IdUser { get; set; }

    public virtual Book? IdBookNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
