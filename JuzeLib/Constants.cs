using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class Constants {

    /// <summary>
    /// Gravity of PG, in g/mL
    /// </summary>
    public const float PG_GRAVITY = 1.038f;

    /// <summary>
    /// Gravity of VG, in g/mL
    /// </summary>
    public const float VG_GRAVITY = 1.26f;

    /// <summary>
    /// Gravity of pure nicotine, in g/mL
    /// </summary>
    public const float NICOTINE_GRAVITY = 1.01f;

    /// <summary>
    /// The version of the interface for JuzeLib.
    /// Only incremented on interface change.
    /// </summary>
    public const int SCHEMA_VERSION = 1;
  }
}
