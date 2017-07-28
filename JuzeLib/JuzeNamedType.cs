using System;
using System.Collections.Generic;
using System.Text;

namespace paujo.juze {
  public class JuzeNamedType : JuzeBaseType {
    /// <summary>
    /// The human-readable name for the element.
    /// </summary>
    public virtual string Name {
      get; set;
    }
  }
}
