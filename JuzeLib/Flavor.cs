using SQLite;

namespace paujo.juze
{
  public class Flavor {


    /// <summary>
    /// Unique ID for the flavor.
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int ID {
      get; set;
    }


    /// <summary>
    /// Human-readable name of the flavor.
    /// </summary>
    public string Name {
      get; set;
    }


    /// <summary>
    /// Is the flavor PG-based?
    /// </summary>
    public bool PG {
      get; set;
    }


    /// <summary>
    /// Recommended starting percentge for new recipes.
    /// </summary>
    public float RecommendedPercentage {
      get; set;
    }


    /// <summary>
    /// The weight of the flavoring, in g/mL.
    /// </summary>
    public float Weight {
      get {
        return PG ? Constants.PG_GRAVITY : Constants.VG_GRAVITY;
      }
    }
  }
}
